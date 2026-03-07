using DBetter.Application.Abstractions.RealtimeNotification;
using Microsoft.AspNetCore.SignalR;

namespace DBetter.Infrastructure.RealtimeNotification;

public class SignalRNotifier(IHubContext<TrainCompositionHub> trainCompositionHub) : IRealtimeNotifier
{
    public Task Notify<T>(string subscription, string method, T value)
    {
        return trainCompositionHub.Clients.Group(subscription).SendAsync(method, value);
    }
}