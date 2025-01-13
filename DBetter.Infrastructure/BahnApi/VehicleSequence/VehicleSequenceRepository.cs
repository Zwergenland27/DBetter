using System.Net;
using System.Net.Http.Json;
using DBetter.Contracts.Journeys.DTOs;
using DBetter.Infrastructure.BahnApi.VehicleSequence.Responses;

namespace DBetter.Infrastructure.BahnApi.VehicleSequence;

public class VehicleSequenceRepository(HttpClient http)
{
    public async Task<VehicleDto?> GetVehicles(string category, string lineNumber, DateTime when, string stationEva)
    {
        var now = DateTime.UtcNow;

        bool realTime = true;
        while (when > now.AddHours(24))
        {
            realTime = false;
            when = when.AddDays(-7);
        }
        
        var administrationId = 80;
        var date = when.ToString("yyyy-MM-dd");
        var time = when.ToString("yyyy-MM-ddTHH:mm:ssZ");
        var url = $"administrationId={administrationId}&date={date}&time={time}&category={category}&number={lineNumber}&evaNumber={stationEva}";
        VehicleSequenceResult? response = null;
        try
        {
            response = await http.GetFromJsonAsync<VehicleSequenceResult>($"reisebegleitung/wagenreihung/vehicle-sequence?{url}");
        }
        catch (HttpRequestException e)
        {
            if (e.StatusCode == HttpStatusCode.InternalServerError)
            {
                while (when > now.AddHours(24))
                {
                    realTime = false;
                    when = when.AddDays(-7);
                    date = when.ToString("yyyy-MM-dd");
                    time = when.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    url = $"administrationId={administrationId}&date={date}&time={time}&category={category}&number={lineNumber}&evaNumber={stationEva}";
                    try
                    {
                        response = await http.GetFromJsonAsync<VehicleSequenceResult>(url);
                    }
                    catch (HttpRequestException inner)
                    {
                        if (inner.StatusCode != HttpStatusCode.InternalServerError) throw;
                    }
                }
            }
            else
            {
                throw;
            }
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
            RealTime = realTime,
        };
    }

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
        { "Apmzf|ABpmz|WRmz|Bpmz|Bpmz|Bpmbz|Bpmzf", "ICE T lang, 1. Serie"},
        { "Bpmzf|Bpmbz|Bpmz|Bpmz|WRmz|ABpmz|Apmzf", "ICE T lang, 1. Serie"},
        { "Apmzf|ABpmz|WRmz|Bpmdz|Bpmz|Bpmbz|Bpmzf", "ICE T lang, 2. Serie"},
        { "Bpmzf|Bpmbz|Bpmz|Bpmdz|WRmz|ABpmz|Apmzf", "ICE T lang, 2. Serie"},
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