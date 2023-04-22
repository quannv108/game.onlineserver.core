using System.Text.Json;
using Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR.Tests;

public class MessageCallbackHandler : ITicTacToeCallbackHandler
{
    public IHubServerActionBase Client { get; set; }
    public ITicTacToeServerAction GameClient => Client as ITicTacToeServerAction;

    private Guid _myPlayerId;
    private char _myMark;
    private int _turnCount;

    public Task MultiLoginDetectedAsync()
    {
        Console.WriteLine("MultiLoginDetected");
        Thread.Sleep(1000);
        Client.AutoJoinMatchAsync(new AutoJoinMatchInput
        {
            GameId = 1,
            TestParam = 111
        });
        
        return Task.CompletedTask;
    }

    public Task ShowErrorAsync(string errorMessage)
    {
        Console.WriteLine($"general error: {errorMessage}");
        return Task.CompletedTask;
    }

    public Task HiAsync(GamePlayerDto gamePlayerDto)
    {
        Console.WriteLine("hi from server");
        Console.WriteLine($"my playerId is {gamePlayerDto.Id}");
        _myPlayerId = gamePlayerDto.Id;
        return Task.CompletedTask;
    }

    public Task MatchFoundAsync(GameMatchDto gameMatchDto)
    {
        Console.WriteLine($"match found {JsonSerializer.Serialize(gameMatchDto)}");
        return Task.CompletedTask;
    }

    public Task UpdateMatchPlayersAsync(MatchPlayersUpdateEventDto eventDto)
    {
        Console.WriteLine($"match players updated {JsonSerializer.Serialize(eventDto)}");
        return Task.CompletedTask;
    }

    public Task UpdateMatchStateAsync(MatchStateEventUpdateDto eventDto)
    {
        Console.WriteLine($"match state updated {JsonSerializer.Serialize(eventDto)}");
        return Task.CompletedTask;
    }

    private static List<List<int>> PlayerX = new List<List<int>>
    {
        new List<int> { 0, 0 },
        new List<int> { 1, 1 },
        new List<int> { 2, 2 }
    };
    private static List<List<int>> PlayerO = new List<List<int>>
    {
        new List<int> { 1, 0 },
        new List<int> { 2, 1 },
        new List<int> { 1, 2 }
    };

    public Task UpdateBoardAsync(TicTacToeBoardDto board)
    {
        Console.WriteLine($"Game board updated:\n" +
                          $"{board.Marks[0][0]} {board.Marks[0][1]} {board.Marks[0][2]}\n" +
                          $"{board.Marks[1][0]} {board.Marks[1][1]} {board.Marks[1][2]}\n" +
                          $"{board.Marks[2][0]} {board.Marks[2][1]} {board.Marks[2][2]}");
        Console.WriteLine($"turn of {board.CurrentTurnMark}");
        TryPlayOneTurn(board.CurrentTurnMark);
        return Task.CompletedTask;
    }

    public Task StartGameAsync(TicTacToeGameSetupDto setup)
    {
        _myMark = setup.PlayerMarks.Single(x => x.PlayerId == _myPlayerId).Mark;
        Console.WriteLine($"Game Ready, my mark is {_myMark}");
        Console.WriteLine($"turn of {setup.StartTurnMark}");
        TryPlayOneTurn(setup.StartTurnMark);
        return Task.CompletedTask;
    }

    public Task EndGameAsync(TicTacToeGameResultDto result)
    {
        Console.WriteLine($"Game ended, winner is {result.WinnerMark}");
        Client.LeaveMatchAsync();
        return Task.CompletedTask;
    }

    private void TryPlayOneTurn(char currentTurnMark)
    {
        if (currentTurnMark == _myMark)
        {
            Console.WriteLine("it's my turn");
            var play = (_myMark == 'X') ? PlayerX : PlayerO;
            var playThisTurn = play[_turnCount];
            _turnCount++;
            GameClient.PutMarkAsync(new PutMarkToBoardInput
            {
                RowIndex = playThisTurn[0],
                ColumnIndex = playThisTurn[1]
            });
        }
        else
        {
            Console.WriteLine("it's opponent turn");
        }
    }
}