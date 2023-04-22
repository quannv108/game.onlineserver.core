using Qna.Game.OnlineServer.SignalR.Client.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core.Models;
using Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Tests;

namespace Qna.Game.OnlineServer.SignalR.Tests2;

public class TicTacToeClient2 : TicTacToeConnection
{
    public TicTacToeClient2(ITicTacToeCallbackHandler messageCallbackHandler) : base("https://localhost:44325",
        "https://localhost:44335",
        new UserCredentials
        {
            Username = "testuser2",
            Password = "Test@1234"
        },
        messageCallbackHandler)
    {
    }
    
    
    public async Task SayHelloAsync()
    {
        // not using await, so doesn't block in client
        HelloAsync();

        await AutoJoinMatchAsync(new AutoJoinMatchInput
        {
            GameId = 1,
            TestParam = 123
        });
    }
}