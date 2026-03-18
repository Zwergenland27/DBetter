using DBetter.Domain.PassengerInformationManagement.ValueObjects;

namespace DBetter.Domain.PassengerInformationManagement;

public interface IPassengerInformationRepository
{
    Task<List<PassengerInformation>> GetManyAsync(IEnumerable<PassengerInformationId> ids);
    Task<List<PassengerInformation>> FindManyAsync(IEnumerable<PassengerInformationText> originalTexts);
    
    void AddRange(IEnumerable<PassengerInformation> passengerInformation);
}