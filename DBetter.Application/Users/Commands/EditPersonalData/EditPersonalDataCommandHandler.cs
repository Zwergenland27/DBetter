using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Commands.EditPersonalData;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;

namespace DBetter.Application.Users.Commands.EditPersonalData;

public class EditPersonalDataCommandHandler(IUserRepository repository) : ICommandHandler<EditPersonalDataCommand, EditPersonalDataResult>
{
    public async Task<CanFail<EditPersonalDataResult>> Handle(EditPersonalDataCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(request.Id);
        if (user is null) return DomainErrors.User.InvalidCredentials;

        var result = user.EditPersonalData(
            request.Firstname,
            request.Lastname,
            request.Email,
            request.Birthday);

        if (result.HasFailed) return result.Errors;

        return new EditPersonalDataResult
        {
            Id = user.Id.Value.ToString(),
            Firstname = user.Firstname.Value,
            Lastname = user.Lastname.Value,
            Email = user.Email.Value,
            Birthday = user.Birthday.Utc
        };
    }
}