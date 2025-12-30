using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Domain.ConnectionRequests;

public interface IConnectionRequestRepository
{
    Task<ConnectionRequest?> GetById(ConnectionRequestId id);
    
    Task StoreAsync(ConnectionRequest connectionRequest);
}