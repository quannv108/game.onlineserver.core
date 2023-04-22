using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Events;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models.UserActions;
using Qna.Game.OnlineServer.Room.Helpers;
using Volo.Abp;
using Volo.Abp.EventBus.Local;

namespace Qna.Game.OnlineServer.GamePlay.TicTacToe;

public class GameLoop : GameLoopBase<GamePlayData, TicTacToeGameState>
{
    public override Room.Room Room => _room;
    public override bool IsFinished => GamePlayData.Winner != Mark.None;
    private readonly ILocalEventBus _localEventBus;
    private readonly IGameEndDetector _gameEndDetector;
    private Room.Room _room;

    public GameLoop(ILocalEventBus localEventBus, IGameEndDetector gameEndDetector)
    {
        _localEventBus = localEventBus;
        _gameEndDetector = gameEndDetector;
    }

    public override void Setup(Room.Room room)
    {
        var playerIds = room.PlayerIds;
        if (playerIds.Count != 2)
        {
            throw new NotSupportedException("only support 2 player");
        }

        _room = room;
        _room.GameLoop = this;

        // create new game play
        GamePlayData = new GamePlayData()
        {
            Id = Guid.NewGuid(),
            Board = new GameBoard(3),
            MarkPlayerIds = new Dictionary<Mark, Guid>
            {
                { Mark.X, playerIds[0] },
                { Mark.O, playerIds[1] }
            }.ToImmutableDictionary(),
            CurrentTurnMark = RandomHelper.GetRandomOf(Mark.O, Mark.X),
            Winner = Mark.None
        };
        State = TicTacToeGameState.Playing;
        
        _localEventBus.PublishAsync(new TicTacToeGameStartEvent
        {
            GamePlayId = GamePlayData.Id,
            RoomName = _room.GetRoomName(),
            MarkPlayerIds = GamePlayData.MarkPlayerIds,
            CurrentTurnMark = GamePlayData.CurrentTurnMark
        }, false);
    }

    public Task UserPlacedMark(UserPlacedMarkAction action)
    {
        // change board data
        var board = GamePlayData.Board;
        var currentTurnMark = GamePlayData.CurrentTurnMark;
        if (currentTurnMark != action.Mark)
        {
            throw new NotSupportedException("invalid player action (not on turn)");
        }
        var currentMark = board.Marks[action.RowIndex, action.ColumnIndex]; 
        if (currentMark != Mark.None)
        {
            throw new NotSupportedException(
                $"already put mark {currentMark.ToString()} at [{action.RowIndex},{action.ColumnIndex}]");
        }
        board.Marks[action.RowIndex, action.ColumnIndex] = action.Mark;
        GamePlayData.CurrentTurnMark = action.Mark == Mark.X ? Mark.O : Mark.X;
        
        // send board update
        _localEventBus.PublishAsync(new TicTacToeGameBoardChangedEvent
        {
            Board = board,
            CurrentTurnMark = GamePlayData.CurrentTurnMark,
            RoomName = _room.GetRoomName()
        }, false);
        
        // check for game state change
        var (shouldEndGame, winnerMark) = _gameEndDetector.ShouldEndGame(GamePlayData.Board);
        if (shouldEndGame)
        {
            GamePlayData.Winner = winnerMark;
            ChangeToState(TicTacToeGameState.Ended);
        }

        return Task.CompletedTask;
    }

    protected override void OnGameChangeState(TicTacToeGameState oldState, TicTacToeGameState newState)
    {
        switch (newState)
        {
            case TicTacToeGameState.Ended:
                Logger.LogDebug($"game ended with winner = {GamePlayData.Winner.ToString()}");
                _localEventBus.PublishAsync(new TicTacToeGameEndEvent
                {
                    RoomId = _room.Id,
                    Winner = GamePlayData.Winner,
                    RoomName = _room.GetRoomName()
                }, false);
                break;
            case TicTacToeGameState.Playing:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    protected override void OnGameUpdate(int milliseconds)
    {
        
    }
}