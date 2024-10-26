using CleanDomainValidation.Domain;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, CanFail> where TCommand : ICommand;

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, CanFail<TResult>> where TCommand : ICommand<TResult>;