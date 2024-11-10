using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Users.Queries.GetMyPassengers;
using DBetter.Domain.Errors;

namespace DBetter.Application.Users.Queries.GetMyPassengers;

public class GetMyPassengersQueryHandler(
    IUserQueryRepository repository) : IQueryHandler<GetMyPassengersQuery, MyPassengersResult>
{
    public async Task<CanFail<MyPassengersResult>> Handle(GetMyPassengersQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetUserPassengers(request.UserId);
        if (result is null) return DomainErrors.User.InvalidCredentials;
        return result;
    }
}