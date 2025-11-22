using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.ValueObjects;
using DBetter.Domain.Users.ValueObjects;

namespace DBetter.Application.Requests.GetSuggestions;

public class GetConnectionSuggestionsQueryBuilder : IRequestBuilder<GetSuggestionsDto, GetConnectionSuggestionsQuery>{
    public ValidatedRequiredProperty<GetConnectionSuggestionsQuery> Configure(RequiredPropertyBuilder<GetSuggestionsDto, GetConnectionSuggestionsQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, ConnectionRequestId.Create);

        var userId = builder.ClassProperty(r => r.UserId)
            .Optional()
            .Map(p => p.Id, UserId.Create);

        var suggestionMode = builder.EnumProperty(r => r.SuggestionMode)
            .Required()
            .Map(p => p.SuggestionMode, Error.Validation("Request.GetSuggestions.SuggestionModeInvalid", "The suggestion mode must be valid"));
        
        return builder.Build(() => new GetConnectionSuggestionsQuery(id, userId, suggestionMode));
    }
}

public record GetConnectionSuggestionsQuery(
    ConnectionRequestId Id,
    UserId?  UserId,
    SuggestionMode SuggestionMode) : IQuery<List<ConnectionResponse>>;