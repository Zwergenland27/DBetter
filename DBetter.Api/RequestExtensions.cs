using CleanDomainValidation.Domain;
using CleanMediator;
using CleanMediator.Commands;
using CleanMediator.Queries;

namespace DBetter.Api;

public static class RequestExtensions
{
    public static async Task<IResult> HandleCommandAsync<TRequest>(
        this IMediator mediator,
        CanFail<TRequest> requestResult,
        Func<IResult> onSuccess)
    where TRequest : ICommand
    {
        if(requestResult.HasFailed) return requestResult.HandleFailure("Request is invalid");
        
        var executionResult = await mediator.ExecuteAsync(requestResult.Value);
        if (executionResult.HasFailed) return executionResult.HandleFailure();
        return onSuccess();
    }
    
    public static async Task<IResult> HandleCommandAsync<TRequest, TResult>(
        this IMediator mediator,
        CanFail<TRequest> requestResult,
        Func<TResult, IResult> onSuccess)
        where TRequest : ICommand<TResult>
    {
        if(requestResult.HasFailed) return requestResult.HandleFailure("Request is invalid");
        
        var executionResult = await mediator.ExecuteAsync(requestResult.Value);
        if (executionResult.HasFailed) return executionResult.HandleFailure();
        return onSuccess(executionResult.Value);
    }
    
    public static async Task<IResult> HandleQueryAsync<TRequest, TResult>(
        this IMediator mediator,
        CanFail<TRequest> requestResult,
        Func<TResult, IResult> onSuccess)
        where TRequest : IQuery<TResult>
    {
        if(requestResult.HasFailed) return requestResult.HandleFailure("Request is invalid");
        
        var executionResult = await mediator.RunAsync(requestResult.Value);
        if (executionResult.HasFailed) return executionResult.HandleFailure();
        return onSuccess(executionResult.Value);
    }

    public static IResult HandleFailure(this ICanFail result, string? customTitle = null)
    {
        var errors = result.Errors.ToList();
        if (result.Type == FailureType.ManyDifferent)
        {
            var firstType = errors[0].Type;
            errors = result.Errors
                .Where(error => error.Type == firstType)
                .ToList();
        }
        
        var statusCode = result.Errors[0].Type switch
        {
            ErrorType.Conflict => 409,
            ErrorType.NotFound => 404,
            ErrorType.Validation => 400,
            ErrorType.Forbidden => 403,
            _ => 500
        };

        var title = "Multiple errors occured. See details for more information.";

        if (result.Type == FailureType.One)
        {
            title = result.Errors[0].Message;
        }

        if (customTitle is not null)
        {
            title = customTitle;
        }
        
        Dictionary<string, List<ErrorDto>> errorsDictionary = [];
        foreach (var error in errors)
        {
            string errorField = error.Code.Remove(error.Code.LastIndexOf('.'));
            if (!errorsDictionary.ContainsKey(errorField))
            {
                errorsDictionary.Add(errorField, []);
            }
            
            errorsDictionary[errorField].Add(new ErrorDto
            {
                Title = error.Code.Substring(error.Code.LastIndexOf('.') + 1),
                Description = error.Message,
            });
        }
        
        return Results.Problem(
            statusCode: statusCode,
            title: title,
            extensions: new Dictionary<string, object?>
            {
                { "errors", errorsDictionary }
            });
    }
}

public class ErrorDto
{
    public required string Title { get; set; }
    public required string Description { get; set; }
}