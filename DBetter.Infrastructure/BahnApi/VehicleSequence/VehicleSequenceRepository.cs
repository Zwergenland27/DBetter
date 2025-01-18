using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using DBetter.Contracts.Journeys.DTOs;
using DBetter.Infrastructure.BahnApi.Journey;
using DBetter.Infrastructure.BahnApi.VehicleSequence.Parameters;
using DBetter.Infrastructure.BahnApi.VehicleSequence.Responses;
using DBetter.Infrastructure.Monitoring;
using HtmlAgilityPack;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;

namespace DBetter.Infrastructure.BahnApi.VehicleSequence;

public class VehicleSequenceRepository(HttpClient http, BahnApiMetrics metrics)
{
    public async Task<VehicleDto?> GetPlannedVehiclesAsync(string lineNr, string startId, DateTime startTime, string endId, DateTime endTime)
    {
        var request = new Root
        {
            Displayinformation = new(),
            Buchungskontext = new()
            {
                BuchungsKontextDaten = new()
                {
                    Zugnummer = lineNr,
                    AbfahrtHalt = new StartHalt
                    {
                        LocationId = startId,
                        AbfahrtZeit = startTime.ConvertToBahnTime(),
                    },
                    AnkunftHalt = new EndHalt()
                    {
                        LocationId = endId,
                        AnkunftZeit = endTime.ConvertToBahnTime(),
                    }
                }
            }
        };
        
        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        
        var uri = HttpUtility.UrlPathEncode(requestJson);
        var response = await http.GetAsync($"gsd/gsd_v3?data={uri}");
        metrics.IncreaseVehicleRequestCount(false, response.StatusCode);
        if (response.IsSuccessStatusCode)
        {
            var html = await response.Content.ReadAsStringAsync();
            var document = new HtmlDocument();
            document.LoadHtml(html);
            var scriptNode = document.DocumentNode.SelectSingleNode("//script[@type='application/json' and @id='ssr_data']");
            if (scriptNode is not null)
            {
                string json = scriptNode.InnerText;
                var sequence = JsonSerializer.Deserialize<PlannedSequence>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                List<string> vehicles = [];
                foreach (var zugteil in sequence!.Zugfahrt.Zugteile)
                {
                    var vehicle = GetData(zugteil.Wagen.Select(v => v.Wagentyp).ToList());
                    vehicles.AddRange(vehicle);
                }

                return new VehicleDto
                {
                    Coaches = vehicles,
                    RealTime = false
                };
            }

            return null;
        }

        return null;
    }
    public async Task<VehicleDto?> GetVehicles(
        string category,
        string lineNumber,
        string departureStation,
        DateTime departureTime,
        string arrivalStation,
        DateTime arrivalTime)
    {
        var now = DateTime.UtcNow;

        if (departureTime < now.AddHours(-24))
        {
            return null;
        }
        
        var inFuture = departureTime > now.AddHours(24);
        
        var administrationId = 80;
        var date = departureTime.ToString("yyyy-MM-dd");
        var time = departureTime.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var url = $"administrationId={administrationId}&date={date}&time={time}&category={category}&number={lineNumber}&evaNumber={departureStation}";
        VehicleSequenceResult? response = null;
        if (!inFuture)
        {
            try
            {
                response = await http.GetFromJsonAsync<VehicleSequenceResult>($"reisebegleitung/wagenreihung/vehicle-sequence?{url}");
                metrics.IncreaseVehicleRequestCount(true, HttpStatusCode.OK);
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == HttpStatusCode.InternalServerError)
                {
                    metrics.IncreaseVehicleRequestCount(true, HttpStatusCode.NotFound);
                    inFuture = true;
                }
                else
                {
                    metrics.IncreaseVehicleRequestCount(true, e.StatusCode!.Value);
                    throw;
                }
            }
        }

        if (inFuture)
        {
            return await GetPlannedVehiclesAsync(lineNumber, departureStation, departureTime, arrivalStation, arrivalTime);
        }

        if (response is null) return null;

        List<string> vehicles = [];
        foreach (var responseGroup in response.Groups)
        {
            var vehicle = GetData(responseGroup.Vehicles.Select(v => v.Type).ToList());
            if (vehicle is not null)
            {
                vehicles.Add(vehicle);
            }
        }

        if (vehicles.Contains("IC1"))
        {
            vehicles.Remove("LOCOMOTIVE");
        }

        return new()
        {
            Coaches = vehicles,
            RealTime = true
        };
    }

    private static Dictionary<string, string> _typeFromPlanned = new()
    {
        { "EPA_4000|EPA_4001|EPA_4002|EPA_4003|EPA_4004|EPA_4005|EPA_4006|EPA_4007", "ICE 1" },
        { "EPA_800|EPA_664|EPA_663|EPA_660|EPA_669|EPA_667", "ICE2" },
        { "EPA_1600|EPA_1602|EPA_1603|EPA_1605|EPA_1608|EPA_1609|EPA_1612", "ICE 3" },
        { "EPA_301|EPA_302|EPA_210|EPA_818|EPA_305|EPA_306|EPA_308|EPA_309", "ICE 3 Velaro" },
        { "EPA_4081|EPA_4082|EPA_4083|EPA_4084|EPA_4085|EPA_5086|EPA_4088|EPA_4089", "ICE 3neo" },
        { "EPA_480|EPA_44|EPA_909|EPA_482|EPA_484|EPA_49|EPA_487", "ICE T lang" },
        { "EPA_935|EPA_934|EPA_930|EPA_911|EPA_903", "ICE T kurz" },
        { "EPA_1111|EPA_1112|EPA_1113|EPA_1114|EPA_1115|EPA_1116|EPA_1117", "ICE 4 (7)" },
        {
            "EPA_292|EPA_353|EPA_353|EPA_854|EPA_854|EPA_357|EPA_853|EPA_342|EPA_222|EPA_358|EPA_863|EPA_208",
            "ICE 4 (12)"
        },
        {
            "EPA_292|EPA_353|EPA_353|EPA_854|EPA_854|EPA_357|EPA_853|EPA_345|EPA_342|EPA_222|EPA_358|EPA_863|EPA_208",
            "ICE 4 (13)"
        },
        { "EPA_1200|EPA_1204|EPA_1205|EPA_1206", "IC 2 KISS kurz"},
        { "EPA_1801|EPA_1802|EPA_1803|EPA_1804|EPA_1805|EPA_1806", "IC 2 KISS lang"},
        { "EPA_977|EPA_975|EPA_972|EPA_979|EPA_981", "IC 2 Twindexx"}
    };

    private static Dictionary<string, string> _typeFromSequence = new()
    {
        //IC KISS
        { "DABpzfa|DBpdkza|DBpbkza|DBpzfa", "IC2 KISS kurz" },
        { "DBpzfa|DBpbkza|DBpdkza|DABpzfa", "IC2 KISS kurz" },
        { "DApzfa|DBpdkza|DBpbkza|DBpkza|DBpkza|DBpzfa", "IC2 KISS lang"},
        { "DBpzfa|DBpkza|DBpkza|DBpbkza|DBpdkza|DApzfa", "IC2 KISS lang"},
        
        //IC Twindexx
        { "DBpbzfa|DBpza|DBpza|DBpza|DApza|LOCOMOTIVE", "IC2 Twindexx"},
        { "LOCOMOTIVE|DApza|DBpza|DBpza|DBpza|DBpbzfa", "IC2 Twindexx"},
        
        //ICE 1
        { "LOCOMOTIVE|Bvmz|Bvmz|Bvmz|Bvmz|Bvmz|Bpmbsz|WRmz|Avmz|Avmz|LOCOMOTIVE", "ICE 1 modernisiert"},
        { "LOCOMOTIVE|Avmz|Avmz|WRmz|Bpmbsz|Bvmz|Bvmz|Bvmz|Bvmz|Bvmz|LOCOMOTIVE", "ICE 1 modernisiert"},
        { "LOCOMOTIVE|Bvmz|Bvmz|Bvmz|Bvmz|Bvmz|Bpmbsz|WSmz|Avmz|Avmz|LOCOMOTIVE", "ICE 1 modernisiert (Bahn.de)"},
        { "LOCOMOTIVE|Avmz|Avmz|WSmz|Bpmbsz|Bvmz|Bvmz|Bvmz|Bvmz|Bvmz|LOCOMOTIVE", "ICE 1 modernisiert (Bahn.de)"},
        
        //ICE 2
        { "Bpmzf|Bpmz|Bpmz|Bpmbz|WRmbsz|Apmz|Apmz|LOCOMOTIVE", "ICE 2"},
        { "LOCOMOTIVE|Apmz|Apmz|WRmbsz|Bpmbz|Bpmz|Bpmz|Bpmzf", "ICE 2"},
        
        //ICE 3
        { "Apmzf|Avmz|Bvmz|BRmz|Bpmbz|Bpmz|Bpmz|Bpmzf", "ICE 3"},
        { "Bpmzf|Bpmz|Bpmz|Bpmbz|BRmz|Bvmz|Avmz|Apmzf", "ICE 3"},
        { "Apmzf|Avmz|Bvmz|WRmz|Bpmbz|Bpmz|Bpmz|Bpmzf", "ICE 3 Redesign"},
        { "Bpmzf|Bpmz|Bpmz|Bpmbz|WRmz|Bvmz|Avmz|Apmzf", "ICE 3 Redesign"},
        { "Apmzf|Avmz|Bpmz|WRmz|Bpmbz|Bpmz|Bpmz|Bpmzf", "ICE 3 Redesign"},
        { "Bpmzf|Bpmz|Bpmz|Bpmbz|WRmz|Bpmz|Avmz|Apmzf", "ICE 3 Redesign"},
        
        //ICE 3 Velaro
        { "Bpmzf|Bpmz|Bpmz|Bpmz|Bpmbsz|ARmz|Apmz|Apmzf", "ICE 3 Velaro"},
        { "Apmzf|Apmz|ARmz|Bpmbsz|Bpmz|Bpmz|Bpmz|Bpmzf", "ICE 3 Velaro"},
        
        //ICE 3neo
        { "Bpmdzf|Bpmz|Bpmz|Bpmz|Bpmbsz|BRpmz|Apmz|Apmzf", "ICE 3neo"},
        { "Apmzf|Apmz|BRpmz|Bpmbsz|Bpmz|Bpmz|Bpmz|Bpmdzf", "ICE 3neo"},
        
        //ICE T
        { "Apmzf|ABpmz|WRmz|Bpmdz|Bpmz|Bpmbz|Bpmzf", "ICE T lang"},
        { "Bpmzf|Bpmbz|Bpmz|Bpmz|WRmz|ABpmz|Apmzf", "ICE T lang"},
        { "Apmzf|Bpmkz|Bpmz|Bpmbz|Bpmzf", "ICE T kurz"},
        { "Bpmzf|Bpmbz|Bpmz|Bpmkz|Apmzf", "ICE T kurz"},
        { "Apmzf|BRpmz|Bpmz|Bpmbz|Bpmzf", "ICE T kurz"},
        { "Bpmzf|Bpmbz|Bpmz|BRpmz|Apmzf", "ICE T kurz"},
        //ICE 4
        { "Apmzf|Apmz|Apmz|ARmz|Bpmbsz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmdzf", "ICE 4 lang (13)"},
        { "Bpmdzf|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmbsz|ARmz|Apmz|Apmz|Apmzf", "ICE 4 lang (13)"},
        { "Apmzf|Apmz|Apmz|ARmz|Bpmbsz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmdzf", "ICE 4 lang (12)"},
        { "Bpmdzf|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmz|Bpmbsz|ARmz|Apmz|Apmz|Apmzf", "ICE 4 lang (12)"},
        { "Apmzf|ARmz|Bpmbsz|Bpmz|Bpmz|Bpmz|Bpmdzf", "ICE 4 kurz"},
        { "Bpmdzf|Bpmz|Bpmz|Bpmz|Bpmbsz|ARmz|Apmzf", "ICE 4 kurz"},
    };

    private List<string> GetData(List<string> coaches)
    {
        List<string> vehicles = [];
        var currentSequence = coaches[0];

        for (var i = 1; i < coaches.Count; i++)
        {
            if (_typeFromPlanned.TryGetValue(currentSequence, out var name))
            {
                vehicles.Add(name);
                currentSequence = coaches[i];
                continue;
            }
            currentSequence += "|" + coaches[i]; 
        }
        
        if (_typeFromPlanned.TryGetValue(currentSequence, out var lastName))
        {
            vehicles.Add(lastName);
        }

        return vehicles;
    }


    private string? GetData(List<VehicleType> vehicles)
    {
        var sequence = "";
        foreach (var vehicleType in vehicles)
        {
            if (vehicleType.ConstructionType is "Bpmmbdzf")
            {
                return "IC1";
            }
            if (vehicleType.Category is "LOCOMOTIVE" or "POWERCAR")
            {
                sequence += "LOCOMOTIVE|";
            }
            else
            {
                sequence += vehicleType.ConstructionType;
                sequence += "|";   
            }
        }

        if (sequence.EndsWith('|'))
        {
            sequence = sequence.Remove(sequence.Length - 1);
        }

        return _typeFromSequence.GetValueOrDefault(sequence, sequence);
    } 
}