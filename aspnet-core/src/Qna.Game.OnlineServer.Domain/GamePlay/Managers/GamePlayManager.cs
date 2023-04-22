using System;
using Microsoft.Extensions.Logging;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.GamePlay.TicTacToe;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.GamePlay.Managers;

public class GamePlayManager : DomainService, IGamePlayManager
{
    public void CreateAndStartGameLoop(Game.Game game, Room.Room room)
    {
        switch (game.Type)
        {
            case GameType.None:
                throw new NotSupportedException();
            case GameType.TicTacToe:
                var gameLoop = CreateGameLoop<GamePlayData, TicTacToeGameState>(room);
                gameLoop.Start();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void StopGameLoop(IGameLoop gameLoop)
    {
        Logger.LogDebug($"aborting game {gameLoop.Id}");
        gameLoop.AbortGame();
        gameLoop.Room.GameLoop = null;
    }

    private IGameLoop<T, TS> CreateGameLoop<T, TS>(Room.Room room)
        where T : class, IGamePlayData
        where TS : struct, Enum
    {
        var gameLoop = LazyServiceProvider.LazyGetRequiredService<IGameLoop<T, TS>>();
        gameLoop.Setup(room);
        Logger.LogDebug($"Created GameLoop for {gameLoop.GetType().Name} with id = {gameLoop.Id}");
        return gameLoop;
    }
}