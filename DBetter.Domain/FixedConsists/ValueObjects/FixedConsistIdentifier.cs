namespace DBetter.Domain.FixedConsists.ValueObjects;

public record FixedConsistIdentifier(string FamilyIdentifier, string VariantIdentifier)
{
    public static FixedConsistIdentifier Default(string familyIdentifier) => new FixedConsistIdentifier(familyIdentifier, string.Empty);
}