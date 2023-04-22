using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core.Models;
using Qna.Game.OnlineServer.SignalR.Contracts.GamePlay.TicTacToe.Dto;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.TicTacToe;

namespace Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;

public class TicTacToeConnection : ConnectionBase, ITicTacToeServerAction
{
    protected TicTacToeConnection(string authHostUrl,
        string signalRHostUrl,
        UserCredentials userUserCredentials,
        ITicTacToeCallbackHandler messageCallbackHandler) :
        base(authHostUrl,
            signalRHostUrl,
            userUserCredentials,
            messageCallbackHandler)
    {
        RegisterCallback(messageCallbackHandler);
    }

    public Task PutMarkAsync(PutMarkToBoardInput toBoardInput)
    {
        return Connection.SendAsync(nameof(ITicTacToeServerAction.PutMarkAsync), toBoardInput);
    }

    private void RegisterCallback(ITicTacToeCallbackHandler messageCallbackHandler)
    {
        Connection.On<TicTacToeGameSetupDto>(nameof(ITicTacToeClientAction.StartGameAsync),
            messageCallbackHandler.StartGameAsync
        );
        Connection.On<TicTacToeGameResultDto>(nameof(ITicTacToeClientAction.EndGameAsync),
            messageCallbackHandler.EndGameAsync
        );
        Connection.On<TicTacToeBoardDto>(nameof(ITicTacToeClientAction.UpdateBoardAsync),
            messageCallbackHandler.UpdateBoardAsync);
    }
}