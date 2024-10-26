using DBetter.Domain.Abstractions;
using MediatR;

namespace DBetter.Application.Abstractions.Messaging;

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent;