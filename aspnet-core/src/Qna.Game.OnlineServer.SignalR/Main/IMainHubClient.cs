using SignalRSwaggerGen.Attributes;

namespace Qna.Game.OnlineServer.SignalR.Main;

[SignalRHub]
public interface IMainHubClient
{
    Task HelloAsync();
}