using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users;
using DBetter.Domain.Errors;
using DBetter.Domain.Users;

namespace DBetter.Application.Users.Commands.Register;

public class RegisterUserCommandHandler(IUserRepository repository) : ICommandHandler<RegisterCommand, UserResult>
{
    public async Task<CanFail<UserResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await repository.GetByEmailAsync(request.Email);
        if (existingUser is not null) return DomainErrors.User.Exists;

        var user = User.Register(
            request.Firstname,
            request.Lastname,
            request.Email,
            request.Birthday,
            request.Password);

        if (user.HasFailed) return user.Errors;
        
        await repository.AddAsync(user.Value);

        return new UserResult
        {
            Id = user.Value.Id.Value.ToString(),
            Firstname = user.Value.Firstname.Value,
            Lastname = user.Value.Lastname.Value,
            Email = user.Value.Email.Value
        };
    }
}