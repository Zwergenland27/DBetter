using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Users.Commands.EditPersonalData;

public class EditPersonalDataRequestBuilder : IRequestBuilder<EditPersonalDataParameters, EditPersonalDataCommand>
{
    public ValidatedRequiredProperty<EditPersonalDataCommand> Configure(RequiredPropertyBuilder<EditPersonalDataParameters, EditPersonalDataCommand> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required(ApplicationErrors.User.EditPersonalData.Id.Missing)
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
            .Map(p => p.Birthday, Birthday.Create);

        return builder.Build(() => new EditPersonalDataCommand(id, firstname, lastname, email, birthday));
    }
}

public record EditPersonalDataCommand(
    UserId Id,
    Firstname? Firstname,
    Lastname? Lastname,
    Email? Email,
    Birthday? Birthday) : ICommand<UserResult>;