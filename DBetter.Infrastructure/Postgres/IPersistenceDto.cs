namespace DBetter.Infrastructure.Postgres;

public interface IPersistenceDto<TDomain>
{
    TDomain ToDomain();
    
    void Apply(TDomain domain);
}