using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;

public interface ITicTacToeServerAction : IHubServerActionBase
{
    Task PutMarkAsync(PutMarkToBoardInput toBoardInput);    
}