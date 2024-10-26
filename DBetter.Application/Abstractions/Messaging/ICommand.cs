using CleanDomainValidation.Domain;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface ICommand : IRequest<CanFail>, CleanDomainValidation.Application.IRequest;

public interface ICommand<TResult> : IRequest<CanFail<TResult>>, CleanDomainValidation.Application.IRequest;