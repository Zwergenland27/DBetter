using CleanDomainValidation.Domain;

namespace DBetter.Application.Errors;

public static partial class ApplicationErrors
{
    public static class Station
    {
        public static class Find
        {
            public static class SearchTerm
            {
                public static Error Missing => Error.Validation("Station.Find.SearchTerm.Missing", "Search term is required.");
            }
        }
    }
}