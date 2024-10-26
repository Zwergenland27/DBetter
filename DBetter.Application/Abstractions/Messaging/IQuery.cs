using CleanDomainValidation.Domain;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface IQuery<TResult> : IRequest<CanFail<TResult>>, CleanDomainValidation.Application.IRequest;