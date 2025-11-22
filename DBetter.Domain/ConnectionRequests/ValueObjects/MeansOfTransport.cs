using DBetter.Domain.Routes.ValueObjects;

namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record MeansOfTransport(
    bool HighSpeedTrains,
    bool FastTrains,
    bool RegionalTrains,
    bool SuburbanTrains,
    bool UndergroundTrains,
    bool Trams,
    bool Busses,
    bool Boats)
{
    public bool AnySelected => HighSpeedTrains || FastTrains || RegionalTrains || SuburbanTrains || UndergroundTrains ||
                               Trams || Busses || Boats;

    public List<TransportCategory> AsList()
    {
        List<TransportCategory> selectedCategories = [];
        if(HighSpeedTrains) selectedCategories.Add(TransportCategory.HighSpeedTrain);
        if(FastTrains) selectedCategories.Add(TransportCategory.FastTrain);
        if(RegionalTrains) selectedCategories.Add(TransportCategory.RegionalTrain);
        if(SuburbanTrains) selectedCategories.Add(TransportCategory.SuburbanTrain);
        if(UndergroundTrains) selectedCategories.Add(TransportCategory.UndergroundTrain);
        if(Trams) selectedCategories.Add(TransportCategory.Tram);
        if(Busses) selectedCategories.Add(TransportCategory.Bus);
        if(Boats) selectedCategories.Add(TransportCategory.Boat);
        
        return selectedCategories;
    }
}