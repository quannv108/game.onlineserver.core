using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;
using Qna.Game.OnlineServer.SignalR.GamePlay;
using Qna.Game.OnlineServer.SignalR.GamePlay.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Hub.Core;
using SignalRSwaggerGen.Attributes;

namespace Qna.Game.OnlineServer.SignalR.Hub.TicTacToe;

[SignalRHub]
public class TicTacToeHub : HubBase<TicTacToeHub, ITicTacToeClientAction>, ITicTacToeServerAction
{
    private readonly IGamePlayService _gamePlayService;
    private ITicTacToeServerAction _ticTacToeServerActionImplementation;

    public TicTacToeHub(IGamePlayService gamePlayService)
    {
        _gamePlayService = gamePlayService;
    }
    
    public Task PutMarkAsync(PutMarkToBoardInput toBoardInput)
    {
        return _gamePlayService.PutMarkAsync(CurrentUserConnectionSession, toBoardInput);
    }
}