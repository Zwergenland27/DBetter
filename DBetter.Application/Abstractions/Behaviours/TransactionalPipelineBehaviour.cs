using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Persistence;
using MediatR;

namespace DBetter.Application.Abstractions.Behaviours;

internal class TransactionalPipelineBehaviour<TRequest, TResponse>(
    IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : CleanDomainValidation.Application.IRequest
    where TResponse : ICanFail
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction();
        try
        {
            var result = await next();
            if (result.HasFailed)
            {
                await unitOfWork.AbortAsync(cancellationToken);
            }
            else
            {
                await unitOfWork.CommitAsync(cancellationToken);
            }
            return result;
        }
        catch (Exception)
        {
            await unitOfWork.AbortAsync(cancellationToken);
            throw;
        }
    }
}