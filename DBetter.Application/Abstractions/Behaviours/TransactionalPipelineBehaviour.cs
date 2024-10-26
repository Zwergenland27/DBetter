using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Persistence;
using MediatR;

namespace DBetter.Application.Abstractions.Behaviours;

internal class TransactionalPipelineBehaviour<TRequest, Tresponse>(
    IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, Tresponse>
    where TRequest : CleanDomainValidation.Application.IRequest
    where Tresponse : ICanFail
{
    private IPipelineBehavior<TRequest, Tresponse> _pipelineBehaviorImplementation;
    public async Task<Tresponse> Handle(TRequest request, RequestHandlerDelegate<Tresponse> next, CancellationToken cancellationToken)
    {
        unitOfWork.BeginTransaction();
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