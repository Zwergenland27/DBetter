using CleanDomainValidation.Application;
using CleanDomainValidation.Application.Extensions;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.ConnectionRequests.ValueObjects;

namespace DBetter.Application.Connections.Queries.GetSuggestions;

public class GetConnectionSuggestionsQueryBuilder : IRequestBuilder<GetSuggestionsParameters, GetConnectionSuggestionsQuery>{
    public ValidatedRequiredProperty<GetConnectionSuggestionsQuery> Configure(RequiredPropertyBuilder<GetSuggestionsParameters, GetConnectionSuggestionsQuery> builder)
    {
        var id = builder.ClassProperty(r => r.Id)
            .Required()
            .Map(p => p.Id, ConnectionRequestId.Create);

        var page = builder.ClassProperty(r => r.Page)
            .Optional()
            .Map(p => p.Page);
        
        return builder.Build(() => new GetConnectionSuggestionsQuery(id,page));
    }
}

public record GetConnectionSuggestionsQuery(
    ConnectionRequestId Id,
    string? Page) : ICommand<ConnectionSuggestionsDto>;