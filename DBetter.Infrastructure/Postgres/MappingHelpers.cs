namespace DBetter.Infrastructure.Postgres;

public static class MappingHelpers
{
    public static void Synchronize<TPersistence, TId, TDomain>(
        this List<TPersistence> persistenceList,
        IEnumerable<TDomain> domainList,
        Func<TPersistence, TId> existingIdSelector,
        Func<TDomain, TId> domainIdSelector,
        Func<TDomain, TPersistence> createMethod)
        where TPersistence: IPersistenceDto<TDomain>
        where TId: notnull
    {
        // Entfernen
        var domainObjects = domainList.ToList();
        foreach (var persistenceObject in persistenceList.ToList())
        {
            if (!domainObjects.Any(incoming =>
                {
                    var incomingId = domainIdSelector(incoming);
                    var existingId = existingIdSelector(persistenceObject);
                    return incomingId.Equals(existingId);
                }))
            {
                persistenceList.Remove(persistenceObject);
            }
        }

        // Hinzufügen & Aktualisieren
        foreach (var domainObject in domainObjects)
        {
            var existingObject = persistenceList.FirstOrDefault(existing =>
            {
                var incomingId = domainIdSelector(domainObject);
                var existingId = existingIdSelector(existing);
                return incomingId.Equals(existingId);
            });
            if (existingObject is not null)
            {
                existingObject.Apply(domainObject);
            }
            else
            {
                persistenceList.Add(createMethod(domainObject));
            }
        }
    }
}
