using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;

namespace DBetter.Application.Users.Commands.EditPersonalData;

public class EditPersonalDataCommandHandler(IUserRepository repository) : ICommandHandler<EditPersonalDataCommand, UserResult>
{
    public async Task<CanFail<UserResult>> Handle(EditPersonalDataCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetAsync(request.Id);
        if (user is null) return DomainErrors.User.InvalidCredentials;

        var result = user.EditPersonalData(
            request.Firstname,
            request.Lastname,
            request.Email);

        if (result.HasFailed) return result.Errors;

        return new UserResult
        {
            Id = user.Id.Value.ToString(),
            Firstname = user.Firstname.Value,
            Lastname = user.Lastname.Value,
            Email = user.Email.Value
        };
    }
}