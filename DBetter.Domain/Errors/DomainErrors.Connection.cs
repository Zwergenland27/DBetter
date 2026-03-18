using CleanDomainValidation.Domain;
using DBetter.Domain.Connections.ValueObjects;

namespace DBetter.Domain.Errors;

public static partial class DomainErrors
{
    public static class Connection
    {
        public static Error NotFound(ConnectionId id) => Error.NotFound("Connection.NotFound",
            $"A connection with the id {id.Value} was not found.");
        public static class Id
        {
            public static Error Invalid(string value) => Error.Validation("Connection.Id.Invalid", $"{value} is no valid guid.");
        }

        public static class Transfer
        {
            public static class Index
            {
                public static Error NotFound => Error.Validation("Connection.Transfer.Index.NotFound", "A transfer with this id does not exist.");
            }
        }
    }
}