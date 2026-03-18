namespace DBetter.Application.Abstractions.RealtimeNotification;

public interface IRealtimeNotifier
{
    Task Notify<T>(string subscription, string method, T value);
}