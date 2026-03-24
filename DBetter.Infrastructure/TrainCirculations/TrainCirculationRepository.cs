using CleanMessageBus.Abstractions;
using DBetter.Domain.TrainCirculations;
using DBetter.Domain.TrainCirculations.ValueObjects;
using DBetter.Infrastructure.OutboxPattern;
using DBetter.Infrastructure.Postgres;
using Microsoft.EntityFrameworkCore;

namespace DBetter.Infrastructure.TrainCirculations;

public class TrainCirculationRepository(DBetterContext db) : ITrainCirculationRepository
{
    public async Task<TrainCirculation?> GetAsync(TrainCirculationId id)
    {
        var persistenceDto = await db.TrainCirculations.FirstOrDefaultAsync(x => x.Id == id.Value);
        return persistenceDto?.ToDomain();
    }

    public async Task<TrainCirculation?> GetAsync(TrainCirculationIdentifier identifier)
    {
        var persistenceDto = await db.TrainCirculations.FirstOrDefaultAsync(tc => tc.Identifier == identifier.DatabaseFriendly());
        return persistenceDto?.ToDomain();
    }

    public async Task<List<TrainCirculation>> GetManyAsync(IEnumerable<TrainCirculationIdentifier> identifiers)
    {
        var databaseFriendlyIdentifiers = identifiers
            .Select(id => id.DatabaseFriendly())
            .ToList();
        
        var persistenceDtos = await db.TrainCirculations
            .Where(tc => databaseFriendlyIdentifiers.Contains(tc.Identifier))
            .ToListAsync();
        
        return persistenceDtos.Select(x => x.ToDomain()).ToList();
    }

    public void Save(TrainCirculation trainCirculation)
    {
        var existing = db.TrainCirculations.Local.FirstOrDefault(e => e.Id == trainCirculation.Id.Value);
        if (existing is null)
        {
            db.TrainCirculations.Add(TrainCirculationPersistenceDto.Create(trainCirculation));
        }
        else
        {
            existing.Apply(trainCirculation);
        }
        
        db.OutboxMessages.AddRange(trainCirculation.DomainEvents.Select(OutboxMessage.FromEvent));
    }

    public void Save(IEnumerable<TrainCirculation> trainCirculations)
    {
        var locals = db.TrainCirculations.Local.ToList();
        var newTrainCirculations = new List<TrainCirculationPersistenceDto>();
        var domainEvents = new List<IDomainEvent>();
        
        foreach (var trainCirculation in trainCirculations)
        {
            var existing = locals.FirstOrDefault(e => e.Id == trainCirculation.Id.Value);
            if (existing is null)
            {
                newTrainCirculations.Add(TrainCirculationPersistenceDto.Create(trainCirculation));
            }
            else
            {
                existing.Apply(trainCirculation);
            }
            
            domainEvents.AddRange(trainCirculation.DomainEvents);
            trainCirculation.ClearDomainEvents();
        }
        
        db.OutboxMessages.AddRange(domainEvents.Select(OutboxMessage.FromEvent));
        db.TrainCirculations.AddRange(newTrainCirculations);
    }
}