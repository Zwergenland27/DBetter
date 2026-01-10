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

    public async Task StoreAsync(ConnectionRequest connectionRequest)
    {
        var existing = await db.ConnectionRequests.FirstOrDefaultAsync(request => request.Id == connectionRequest.Id);
        if (existing is not null)
        {
            db.ConnectionRequests.Remove(existing);
        }
        db.ConnectionRequests.Add(connectionRequest);
    }
}