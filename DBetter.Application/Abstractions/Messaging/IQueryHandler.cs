using CleanDomainValidation.Domain;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, CanFail<TResult>> where TQuery : IQuery<TResult>;