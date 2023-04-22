using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;

namespace Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;

public interface ITicTacToeClientAction : IHubClientActionBase
{
    Task UpdateBoardAsync(TicTacToeBoardDto board);
    Task StartGameAsync(TicTacToeGameSetupDto setup);
    Task EndGameAsync(TicTacToeGameResultDto result);
}