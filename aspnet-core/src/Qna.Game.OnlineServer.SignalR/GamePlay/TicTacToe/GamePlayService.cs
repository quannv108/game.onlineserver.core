using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Game.Managers;
using Qna.Game.OnlineServer.GamePlay.TicTacToe;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Events;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models;
using Qna.Game.OnlineServer.GamePlay.TicTacToe.Models.UserActions;
using Qna.Game.OnlineServer.Room.Managers;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Hub.TicTacToe;
using Volo.Abp;
using Volo.Abp.EventBus;

namespace Qna.Game.OnlineServer.SignalR.GamePlay.TicTacToe;

[RemoteService(false)]
public class GamePlayService : SignalRServiceBase<TicTacToeHub, ITicTacToeClientAction>,
    IGamePlayService,
    ILocalEventHandler<TicTacToeGameBoardChangedEvent>,
    ILocalEventHandler<TicTacToeGameEndEvent>,
    ILocalEventHandler<TicTacToeGameStartEvent>
{
    private readonly IRoomManager _roomManager;
    private readonly Game.Game _game;

    public GamePlayService(IRoomManager roomManager, IGameManager gameManager)
    {
        _roomManager = roomManager;
        _game = gameManager.GetByTypeAsync(GameType.TicTacToe).Result;
    }

    public GameLoop GetGameLoop(Guid roomId)
    {
        var room = _roomManager.Get(roomId);
        if (room == null)
        {
            return null;
        }

        var gameLoop = (GameLoop) room.GameLoop;
        return gameLoop;
    }
    
    public async Task PutMarkAsync(UserConnectionSession session, PutMarkToBoardInput toBoardInput)
    {
        var gameLoop = GetGameLoop(session.CurrentMatch.Id);
        var gameData = gameLoop.GamePlayData;
        var playerMark = gameData.GetPlayerMark(session.CurrentPlayer.Id);
        await gameLoop.UserPlacedMark(new UserPlacedMarkAction
        {
            RowIndex = toBoardInput.RowIndex,
            ColumnIndex = toBoardInput.ColumnIndex,
            Mark = playerMark
        });
    }

    public Task HandleEventAsync(TicTacToeGameBoardChangedEvent eventData)
    {
        // send update to all user
        return Clients.Group(eventData.RoomName).UpdateBoardAsync(new TicTacToeBoardDto
        {
            Marks = eventData.Board.GetMarksAsList(),
            CurrentTurnMark = eventData.CurrentTurnMark.ToChar()
        });
    }

    public async Task HandleEventAsync(TicTacToeGameEndEvent eventData)
    {
        // send update to all user
        await Clients.Group(eventData.RoomName).EndGameAsync(new TicTacToeGameResultDto
        {
            WinnerMark = eventData.Winner.ToChar()
        });
    }

    public Task HandleEventAsync(TicTacToeGameStartEvent eventData)
    {
        //send update to all user
        return Clients.Group(eventData.RoomName).StartGameAsync(new TicTacToeGameSetupDto
        {
            PlayerMarks = eventData.MarkPlayerIds.ToList().Select(x => new PlayerMarkDto
            {
                Mark = x.Key.ToChar(),
                PlayerId = x.Value
            }).ToList(),
            StartTurnMark = eventData.CurrentTurnMark.ToChar()
        });
    }
}