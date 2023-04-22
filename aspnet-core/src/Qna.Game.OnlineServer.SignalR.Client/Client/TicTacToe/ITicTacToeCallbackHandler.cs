using Qna.Game.OnlineServer.SignalR.Client.Client.Core;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;

namespace Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;

public interface ITicTacToeCallbackHandler : IMessageCallbackHandler, ITicTacToeClientAction
{
    ITicTacToeServerAction GameClient { get; }
}