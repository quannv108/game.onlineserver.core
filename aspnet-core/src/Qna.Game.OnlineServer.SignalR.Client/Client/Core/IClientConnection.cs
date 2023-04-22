using Microsoft.AspNetCore.SignalR.Client;

namespace Qna.Game.OnlineServer.SignalR.Client.Client.Core;

public interface IClientConnection
{
    Task StartAsync();

    HubConnectionState State { get; }
}