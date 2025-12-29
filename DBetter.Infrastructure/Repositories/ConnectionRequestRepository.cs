using DBetter.Domain.ConnectionRequests;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class ConnectionRequestRepository(DBetterContext db) : IConnectionRequestRepository
{
    public Task<ConnectionRequest?> GetById(ConnectionRequestId id)
    {
        return db.ConnectionRequests.FirstOrDefaultAsync(request => request.Id == id);
    }

    public void Store(ConnectionRequest connectionRequest)
    {
        db.ConnectionRequests.Add(connectionRequest);
    }
}