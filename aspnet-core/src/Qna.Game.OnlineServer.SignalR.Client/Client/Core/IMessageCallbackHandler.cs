using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

namespace Qna.Game.OnlineServer.SignalR.Client.Client.Core;

public interface IMessageCallbackHandler : IHubClientActionBase
{
    IHubServerActionBase Client { get; set; }
}