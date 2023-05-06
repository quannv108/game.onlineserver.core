using Qna.Game.OnlineServer.SignalR.Client.Client.Core.Models;
using Qna.Game.OnlineServer.SignalR.Client.Client.TicTacToe;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;

namespace Qna.Game.OnlineServer.SignalR.Tests.TicTacToe;

public class TicTacToeClient : TicTacToeConnection
{
    public TicTacToeClient(ITicTacToeCallbackHandler messageCallbackHandler)
        : base(
            "https://dev.quannv.click:44325",
            "https://dev.quannv.click:44335",
            // "https://host.docker.internal:44325",
            // "https://host.docker.internal:44335",
            // "https://localhost:44325",
            // "https://localhost:44335",
            new UserCredentials
            {
                Username = "testuser",
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