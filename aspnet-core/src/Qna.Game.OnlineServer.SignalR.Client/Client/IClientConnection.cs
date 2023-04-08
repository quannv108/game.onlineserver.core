using Microsoft.AspNetCore.SignalR.Client;

namespace Qna.Game.OnlineServer.SignalR.Client.Client;

public interface IClientConnection
{
    Task StartAsync();
    Task HelloAsync();

    HubConnectionState State { get; }
}