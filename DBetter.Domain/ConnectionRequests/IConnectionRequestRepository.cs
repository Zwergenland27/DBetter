using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public interface IConnectionRequestRepository
{
    Task<ConnectionRequest?> GetAsync(ConnectionRequestId id);
    
    void Add(ConnectionRequest connectionRequest);
}