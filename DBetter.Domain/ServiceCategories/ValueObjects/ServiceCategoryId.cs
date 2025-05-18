using CleanDomainValidation.Domain;
using DBetter.Domain.Errors;

namespace DBetter.Domain.ServiceCategories.ValueObjects;

public record ServiceCategoryId(Guid Value)
{
    public static ServiceCategoryId CreateNew()
    {
        return new(Guid.NewGuid());
    }
    
    public static CanFail<ServiceCategoryId> Create(string value)
    {
        if (Guid.TryParse(value, out var guid))
        {
            return new ServiceCategoryId(guid);
        }

        return DomainErrors.ServiceCategory.Id.Invalid;
    }
}