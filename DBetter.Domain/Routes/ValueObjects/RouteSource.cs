namespace DBetter.Domain.Routes.ValueObjects;

public enum RouteSource
{
    /// <summary>
    /// Route is possibly only partially available
    /// </summary>
    Connection,
    /// <summary>
    /// Full route is available
    /// </summary>
    TrainRun,
    
    /// <summary>
    /// Route is generated based on previous route information
    /// </summary>
    Historical
}