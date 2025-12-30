using System.Security.Claims;
using CleanDomainValidation.Application;
using CleanMediator;
using DBetter.Application.Requests;
using DBetter.Application.Requests.GetSuggestions;
using DBetter.Application.Requests.UpsertRequest;
using DBetter.Contracts.Requests.CreateRequest;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Parameters;
using DBetter.Contracts.Requests.Queries.GetSuggestions.Results;

namespace DBetter.Api;

public static class RequestModule
{
    public static void AddRequestEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPut("requests/{id}", async (
            string id,
            ClaimsPrincipal user,
            IMediator mediator,
            ConnectionRequestDto dto) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            var ownerId = userIdClaim?.Value;
                
            var command = Builder<UpsertConnectionRequestCommand>
                .WithName("Requests.Create")
                .BindParameters(dto)
                .MapParameter(r => r.RequestId, id)
                .MapParameter(r => r.OwnerId, ownerId)
                .BuildUsing<CreateConnectionRequestCommandBuilder>();
            
            return await mediator.HandleCommandAsync(command, (List<ConnectionResponse> result) =>
            {
                return Results.Ok(result);
            });
        }).WithName("CreateRequest")
        .Produces<List<ConnectionResponse>>()
        .WithOpenApi();
        
        app.MapGet("requests/{id}/suggestions", async (
            string id,
            ClaimsPrincipal user,
            IMediator mediator) =>
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

            var userId = userIdClaim?.Value;
            
            var query = Builder<GetConnectionSuggestionsQuery>
                .WithName("Request.GetSuggestions")
                .BindParameters(new GetSuggestionsDto())
                .MapParameter(r => r.Id, id)
                .MapParameter(r => r.UserId, userId)
                .MapParameter(r => r.SuggestionMode, nameof(SuggestionMode.Normal))
                .BuildUsing<GetConnectionSuggestionsQueryBuilder>();

            return await mediator.HandleQueryAsync(query, (List<ConnectionResponse> result) =>
            {
                return Results.Ok(result);
            });

        }).WithName("GetEarlierSuggestions")
        .Produces<List<ConnectionResponse>>()
        .WithOpenApi();
        
        app.MapGet("requests/{id}/suggestions/earlier", async (
                string id,
                ClaimsPrincipal user,
                IMediator mediator) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                var userId = userIdClaim?.Value;
            
                var query = Builder<GetConnectionSuggestionsQuery>
                    .WithName("Request.GetSuggestions")
                    .BindParameters(new GetSuggestionsDto())
                    .MapParameter(r => r.Id, id)
                    .MapParameter(r => r.UserId, userId)
                    .MapParameter(r => r.SuggestionMode, nameof(SuggestionMode.Earlier))
                    .BuildUsing<GetConnectionSuggestionsQueryBuilder>();

                return await mediator.HandleQueryAsync(query, (List<ConnectionResponse> result) =>
                {
                    return Results.Ok(result);
                });

            }).WithName("GetLaterSuggestions")
            .Produces<List<ConnectionResponse>>()
            .WithOpenApi();
        app.MapGet("requests/{id}/suggestions/later", async (
                string id,
                ClaimsPrincipal user,
                IMediator mediator) =>
            {
                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                var userId = userIdClaim?.Value;
            
                var query = Builder<GetConnectionSuggestionsQuery>
                    .WithName("Request.GetSuggestions")
                    .BindParameters(new GetSuggestionsDto())
                    .MapParameter(r => r.Id, id)
                    .MapParameter(r => r.UserId, userId)
                    .MapParameter(r => r.SuggestionMode, nameof(SuggestionMode.Later))
                    .BuildUsing<GetConnectionSuggestionsQueryBuilder>();

                return await mediator.HandleQueryAsync(query, (List<ConnectionResponse> result) =>
                {
                    return Results.Ok(result);
                });

            }).WithName("GetSuggestions")
            .Produces<List<ConnectionResponse>>()
            .WithOpenApi();
    }
}