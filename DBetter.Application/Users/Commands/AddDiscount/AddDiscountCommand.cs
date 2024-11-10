using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users.Commands.AddDiscount;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.AddDiscount;

public class AddDiscountRequestBuilder : IRequestBuilder<AddDiscountParameters, AddDiscountCommand>
{
    public ValidatedRequiredProperty<AddDiscountCommand> Configure(RequiredPropertyBuilder<AddDiscountParameters, AddDiscountCommand> builder)
    {
        var userId = builder.ClassProperty(r => r.UserId)
            .Required(ApplicationErrors.User.AddDiscount.UserId.Missing)
            .Map(p => p.UserId, UserId.Create);

        var type = builder.EnumProperty(r => r.Type)
            .Required(ApplicationErrors.User.AddDiscount.Type.Missing)
            .Map(p => p.Type, ApplicationErrors.User.AddDiscount.Type.Invalid);
        
        var @class = builder.EnumProperty(r => r.Class)
            .Required(ApplicationErrors.User.AddDiscount.Class.Missing)
            .Map(p => p.Class, ApplicationErrors.User.AddDiscount.Class.Invalid);

        var boughtAtUtc = builder.StructProperty(r => r.BoughtAtUtc)
            .Required(ApplicationErrors.User.AddDiscount.BoughtAtUtc.Missing)
            .Map(p => p.BoughtAt);

        var validUntilUtc = builder.StructProperty(r => r.ValidUntilUtc)
            .Required(ApplicationErrors.User.AddDiscount.ValidUntilUtc.Missing)
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