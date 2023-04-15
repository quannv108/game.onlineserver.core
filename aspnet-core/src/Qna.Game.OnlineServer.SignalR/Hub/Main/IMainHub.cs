using SignalRSwaggerGen.Attributes;

namespace Qna.Game.OnlineServer.SignalR.Hub.Main;

// workaround for issue: https://github.com/abpframework/abp/issues/16217#event-8969683870
// TODO: issue: https://github.com/essencebit/SignalRSwaggerGen/issues/39
[SignalRHub]
public interface IMainClient : Contracts.Hub.Main.IMainHubServerToClient
{
}