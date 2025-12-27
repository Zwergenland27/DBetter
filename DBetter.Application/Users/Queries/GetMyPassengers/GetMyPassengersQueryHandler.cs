using CleanDomainValidation.Domain;
using CleanMediator.Queries;
using DBetter.Contracts.Users.Queries.GetMyPassengers;
using DBetter.Domain.Errors;

namespace DBetter.Application.Users.Queries.GetMyPassengers;

public class GetMyPassengersQueryHandler(
    IUserQueryRepository repository) : QueryHandlerBase<GetMyPassengersQuery, MyPassengersResult>
{
    public override async Task<CanFail<MyPassengersResult>> Handle(GetMyPassengersQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetUserPassengers(request.UserId);
        if (result is null) return DomainErrors.User.InvalidCredentials;
        return result;
    }
}