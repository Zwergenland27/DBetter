namespace DBetter.Infrastructure.BahnDe;

public class BahnDeException(string serviceName, string message) : Exception($"BahnDe.{serviceName}: {message}");