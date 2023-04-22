using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

/// <summary>
/// interface to show which action server can call to client
/// </summary>
public interface IHubClientActionBase : ISessionClientAction, IMatchClientAction
{
    Task ShowErrorAsync(string errorMessage);
    Task HiAsync(GamePlayerDto gamePlayer);
}