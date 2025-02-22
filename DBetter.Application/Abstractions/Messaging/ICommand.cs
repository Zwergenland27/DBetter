using CleanDomainValidation.Domain;
using DBetter.Application.Abstractions.Behaviours;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface ICommand : IRequest<CanFail>, CleanDomainValidation.Application.IRequest, ITransactionRequired;

public interface ICommand<TResult> : IRequest<CanFail<TResult>>, CleanDomainValidation.Application.IRequest, ITransactionRequired;