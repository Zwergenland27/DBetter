using DBetter.Domain.Abstractions;
using DBetter.Infrastructure.Postgres;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace DBetter.Infrastructure.OutboxPattern;

[DisallowConcurrentExecution]
public class OutboxProcessor(
    IMediator mediator,
    DBetterContext db) : IJob
{
    public static JobKey JobKey => new(nameof(OutboxProcessor));
    
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await db.OutboxMessages
            .Where(message => message.ProcessedAt == null)
            .Take(20)
            .ToListAsync();

        foreach (var message in messages)
        {
            IDomainEvent @event = message.ExtractEvent();
            await mediator.Publish(@event);
            message.Processed();
        }
        
        await db.SaveChangesAsync();
    }
}