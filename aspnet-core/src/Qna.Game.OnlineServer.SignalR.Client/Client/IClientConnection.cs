using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;

namespace Qna.Game.OnlineServer.SignalR.Client.Client;

public interface IClientConnection : IMainHubClientToServer
{
    Task StartAsync();

    HubConnectionState State { get; }
}