using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanMediator.Commands;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using DBetter.Domain.Shared;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.EditPersonalData;

public class EditPersonalDataRequestBuilder : IRequestBuilder<EditPersonalDataParameters, EditPersonalDataCommand>
{
    public ValidatedRequiredProperty<EditPersonalDataCommand> Configure(RequiredPropertyBuilder<EditPersonalDataParameters, EditPersonalDataCommand> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, UserId.Create);
        
        var firstname = builder.ClassProperty(r => r.Firstname)
            .Optional()
            .Map(p => p.Firstname, Firstname.Create);
        
        var lastname = builder.ClassProperty(r => r.Lastname)
            .Optional()
            .Map(p => p.Lastname, Lastname.Create);
        
        var email = builder.ClassProperty(r => r.Email)
            .Optional()
            .Map(p => p.Email, Email.Create);
        
        var birthday = builder.ClassProperty(r => r.Birthday)
            .Optional()
            .Map(p => p.Birthday, value => Birthday.Create(DateTimeFactory.CreateFromIso8601(value)));

        return builder.Build(() => new EditPersonalDataCommand(id, firstname, lastname, email, birthday));
    }
}

public record EditPersonalDataCommand(
    UserId Id,
    Firstname? Firstname,
    Lastname? Lastname,
    Email? Email,
    Birthday? Birthday) : ICommand<EditPersonalDataResult>;