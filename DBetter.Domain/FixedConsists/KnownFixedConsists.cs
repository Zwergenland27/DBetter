using DBetter.Domain.Consists.ValueObjects;
using DBetter.Domain.FixedConsists.ValueObjects;

namespace DBetter.Domain.FixedConsists;

public static class KnownFixedConsists
{
    public static FixedConsist Ic2 => new FixedConsist(ConsistId.Create("15215d52-5cdc-41fb-bd9c-9b8957d61c07").Value, FixedConsistIdentifier.Default("IC2"));
    
    public static FixedConsist IceL => new FixedConsist(ConsistId.Create("f397a58d-6791-43e4-8e70-df4ff77ee8b5").Value, FixedConsistIdentifier.Default("ICEL"));
}