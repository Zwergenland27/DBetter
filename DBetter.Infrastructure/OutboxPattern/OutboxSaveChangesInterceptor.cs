using DBetter.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DBetter.Infrastructure.OutboxPattern;

public class OutboxSaveChangesInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        
        PublishDomainEvents(dbContext);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void PublishDomainEvents(DbContext? dbContext)
    {
        if (dbContext is null) return;
        
        var outboxMessages = dbContext.ChangeTracker
            .Entries<IHasDomainEvent>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.DomainEvents;
                aggregateRoot.ClearDomainEvents();
                return domainEvents;
            })
            .Select(OutboxMessage.FromEvent)
            .ToList();
        
        dbContext.AddRange(outboxMessages);
    }

}