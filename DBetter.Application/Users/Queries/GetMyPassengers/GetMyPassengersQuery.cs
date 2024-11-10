using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Application.Errors;
using DBetter.Contracts.Users.Queries.GetMyPassengers;
using DBetter.Domain.Users.ValueObjects;
using MediatR;

namespace DBetter.Application.Users.Queries.GetMyPassengers;

public class GetMyPassengersRequestBuilder : IRequestBuilder<GetMyPassengersParameters, GetMyPassengersQuery>
{
    public ValidatedRequiredProperty<GetMyPassengersQuery> Configure(RequiredPropertyBuilder<GetMyPassengersParameters, GetMyPassengersQuery> builder)
    {
        var userId = builder.ClassProperty(r => r.UserId)
            .Required(ApplicationErrors.User.GetMyPassengers.UserId.Missing)
            .Map(p => p.UserId, UserId.Create);
        
        return builder.Build(() => new GetMyPassengersQuery(userId));
    }
}

public record GetMyPassengersQuery(UserId UserId) : IQuery<MyPassengersResult>;