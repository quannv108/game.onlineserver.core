namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

public interface ISessionClientAction
{
    Task MultiLoginDetectedAsync();
}