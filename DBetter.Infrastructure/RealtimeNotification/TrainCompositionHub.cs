using DBetter.Contracts.TrainCompositions.Get;
using Microsoft.AspNetCore.SignalR;

namespace DBetter.Infrastructure.RealtimeNotification;

public class TrainCompositionHub : Hub
{
    public Task Subscribe(string trainRunId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, trainRunId);
    }

    public Task Unsubscribe(string trainRunId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, trainRunId);
    }
}