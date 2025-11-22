namespace DBetter.Domain.ConnectionRequests.ValueObjects;

public record PaginationReference
{
    public string Token { get; private init; }

    private PaginationReference(string token)
    {
        Token = token;
    }

    public static PaginationReference Create(string token)
    {
        return new PaginationReference(token);
    }
}