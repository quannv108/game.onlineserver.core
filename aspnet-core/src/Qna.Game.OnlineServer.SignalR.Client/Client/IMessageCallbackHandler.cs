using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;

namespace Qna.Game.OnlineServer.SignalR.Client.Client;

public interface IMessageCallbackHandler : IMainHubServerToClient
{
    IMainHubClientToServer Client { get; set; }
}