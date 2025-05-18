using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Messaging;
using DBetter.Contracts.Connections.Queries.GetSuggestions.Results;
using DBetter.Domain.Errors;

namespace DBetter.Application.Connections.Queries.GetSuggestions;

public class GetConnectionSuggestionsQueryHandler(IConnectionsQueryRepository repository) : ICommandHandler<GetConnectionSuggestionsQuery, ConnectionSuggestionsDto>
{
    public async Task<CanFail<ConnectionSuggestionsDto>> Handle(GetConnectionSuggestionsQuery request, CancellationToken cancellationToken)
    {
        var suggestions = await repository.GetConnectionSuggestionsAsync(request.Id, request.Page);
        if (suggestions is null) return DomainErrors.ConnectionRequest.NotFound;
        return suggestions;
    }
}