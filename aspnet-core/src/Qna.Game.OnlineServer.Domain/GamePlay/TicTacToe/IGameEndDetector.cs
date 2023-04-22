using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe;

public interface IGameEndDetector : ITransientDependency
{
    public (bool, Mark) ShouldEndGame(GameBoard board);
}