using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Commands.AddDiscount;
using DBetter.Domain.Errors;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.AddDiscount;

public class AddDiscountRequestBuilder : IRequestBuilder<AddDiscountParameters, AddDiscountCommand>
{
    public ValidatedRequiredProperty<AddDiscountCommand> Configure(RequiredPropertyBuilder<AddDiscountParameters, AddDiscountCommand> builder)
    {
        var userId = builder.ClassProperty(r => r.UserId)
            .Required()
            .Map(p => p.UserId, UserId.Create);

        var type = builder.EnumProperty(r => r.Type)
            .Required()
            .Map(p => p.Type, DomainErrors.Shared.DiscountType.Invalid);
        
        var @class = builder.EnumProperty(r => r.Class)
            .Required()
            .Map(p => p.Class, DomainErrors.Shared.Class.Invalid);

        var boughtAtUtc = builder.StructProperty(r => r.BoughtAtUtc)
            .Required()
            .Map(p => p.BoughtAt);

        var validUntilUtc = builder.StructProperty(r => r.ValidUntilUtc)
            .Required()
            .Map(p => p.ValidUntil);
        
        return builder.Build(() => new AddDiscountCommand(
            userId,
            type,
            @class,
            boughtAtUtc,
            validUntilUtc));
    }
}

public record AddDiscountCommand(
    UserId UserId,
    DiscountType Type,
    Class Class,
    DateTime BoughtAtUtc,
    DateTime? ValidUntilUtc) : ICommand;