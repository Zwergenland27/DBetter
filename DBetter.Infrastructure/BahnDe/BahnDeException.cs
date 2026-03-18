using DBetter.Application.Abstractions;

namespace DBetter.Infrastructure.BahnDe;

public class BahnDeException(string serviceName, string message) : ServiceException($"BahnDe.{serviceName}", message);