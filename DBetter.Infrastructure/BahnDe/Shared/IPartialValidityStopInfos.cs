namespace DBetter.Infrastructure.BahnDe.Shared;

public interface IPartialValidityStopInfos
{
    public string Name { get; set; }
    
    public int RouteIdx { get; set; }
}