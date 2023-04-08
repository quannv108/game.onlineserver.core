using Qna.Game.OnlineServer.SignalR.Client.Client;

namespace Qna.Game.OnlineServer.SignalR.Tests;

public class DummyClient : BaseConnection
{
    public DummyClient(IMessageCallbackHandler messageCallbackHandler)
        : base("https://localhost:44325",
            "https://localhost:44335",
            new UserCredentials
            {
                Username = "testuser",
                Password = "Test@1234"
            },
            messageCallbackHandler)
    {
    }
}