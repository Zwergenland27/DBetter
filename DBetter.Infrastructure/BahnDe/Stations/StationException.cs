namespace DBetter.Infrastructure.BahnDe.Stations;

public class StationException(string message) : BahnDeException("Station", message);