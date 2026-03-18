using CleanDomainValidation.Domain;
using CleanMediator.Commands;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.AddDiscount;

public class AddDiscountCommandHandler(IUserRepository repository) : CommandHandlerBase<AddDiscountCommand>
{
    public override async Task<CanFail> Handle(AddDiscountCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(request.UserId);
        if (user is null) return DomainErrors.User.InvalidCredentials;
        
        var discount = Discount.Create(
            request.Type,
            request.ComfortClass,
            request.BoughtAtUtc,
            request.ValidUntilUtc);
        
        if (discount.HasFailed) return discount.Errors;

        var result = user.AddDiscount(discount.Value);
        if (result.HasFailed) return result.Errors;
        
        return CanFail.Success;
    }
}