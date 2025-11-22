using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public interface IConnectionRequestRepository
{
    Task<ConnectionRequest?> GetById(ConnectionRequestId id);
    
    void Store(ConnectionRequest connectionRequest);
}