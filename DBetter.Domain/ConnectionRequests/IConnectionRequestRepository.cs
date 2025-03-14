using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public interface IConnectionRequestRepository
{
    Task<ConnectionRequest?> GetAsync(ConnectionRequestId id);
    
    Task AddAsync(ConnectionRequest connectionRequest);
}