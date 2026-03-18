namespace DBetter.Application.Abstractions;

public class ServiceException(string serviceName, string message) : Exception($"{serviceName}: {message}");