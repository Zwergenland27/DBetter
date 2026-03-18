using DBetter.Domain.PassengerInformationManagement;
using DBetter.Domain.PassengerInformationManagement.ValueObjects;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.Repositories;

public class PassengerInformationRepository(DBetterContext db) : IPassengerInformationRepository
{
    public Task<List<PassengerInformation>> GetManyAsync(IEnumerable<PassengerInformationId> ids)
    {
        return db.PassengerInformation.Where(pi => ids.Contains(pi.Id)).ToListAsync();
    }

    public Task<List<PassengerInformation>> FindManyAsync(IEnumerable<PassengerInformationText> originalTexts)
    {
        return db.PassengerInformation.Where(pi => originalTexts.Contains(pi.Text)).ToListAsync();
    }

    public void AddRange(IEnumerable<PassengerInformation> passengerInformation)
    {
        db.PassengerInformation.AddRange(passengerInformation);
    }
}