using Qna.Game.OnlineServer.SignalR.Client.Client;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;

namespace Qna.Game.OnlineServer.SignalR.Tests2;

public class DummyClient2 : BaseConnection
{
    public DummyClient2(IMessageCallbackHandler messageCallbackHandler) : base("https://localhost:44325",
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

        await AutoJoinMatchAsync(new AutoJoinMatchInput()
        {
            TestParam = 123
        });
    }
}