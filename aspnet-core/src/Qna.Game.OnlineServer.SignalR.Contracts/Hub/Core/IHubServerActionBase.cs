namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

/// <summary>
/// interface to describe which action client can request to server
/// </summary>
public interface IHubServerActionBase : IMatchServerAction
{
    Task HelloAsync();
}