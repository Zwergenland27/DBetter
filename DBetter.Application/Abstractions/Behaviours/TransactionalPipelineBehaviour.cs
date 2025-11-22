using System.Diagnostics;
using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Persistence;
using MediatR;

namespace DBetter.Application.Abstractions.Behaviours;

internal class TransactionalPipelineBehaviour<TRequest, TResponse>(
    IUnitOfWork unitOfWork) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITransactionRequired
    where TResponse : ICanFail
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransaction(cancellationToken);
        try
        {
            var result = await next();
            if (result.HasFailed)
            {
                await unitOfWork.AbortAsync(cancellationToken);
            }
            else
            {
                var sw = Stopwatch.StartNew();
                await unitOfWork.CommitAsync(cancellationToken);
                sw.Stop();
                Console.WriteLine($"Transaction: {sw.ElapsedMilliseconds}ms");
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