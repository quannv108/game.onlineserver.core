using System.Text.Json;
using Qna.Game.OnlineServer.SignalR.Client.Client;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

namespace Qna.Game.OnlineServer.SignalR.Tests;

public class MessageCallbackHandler : IMessageCallbackHandler
{
    public IMainHubClientToServer Client { get; set; }
    
    public Task MultiLoginDetectedAsync()
    {
        Console.WriteLine("MultiLoginDetected");
        Thread.Sleep(1000);
        Client.AutoJoinMatchAsync(new AutoJoinMatchInput
        {
            TestParam = 111
        });
        
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string errorMessage)
    {
        Console.WriteLine($"general error: {errorMessage}");
        return Task.CompletedTask;
    }

    public Task HiAsync()
    {
        Console.WriteLine("hi from server");
        return Task.CompletedTask;
    }

    public Task MatchFoundAsync(GameMatchDto gameMatchDto)
    {
        Console.Write($"match found {JsonSerializer.Serialize(gameMatchDto)}");
        return Task.CompletedTask;
    }

    public Task UpdateMatchPlayersAsync(MatchPlayersUpdateEventDto eventDto)
    {
        Console.Write($"match players updated {JsonSerializer.Serialize(eventDto)}");
        return Task.CompletedTask;
    }

    public Task UpdateMatchStateAsync(MatchStateEventUpdateDto eventDto)
    {
        Console.Write($"match state updated {JsonSerializer.Serialize(eventDto)}");
        return Task.CompletedTask;
    }
}