namespace Qna.Game.OnlineServer.SignalR.Client.Client.Core;

public class HubUrlBuilder
{
    private readonly Uri _signalUri;
    public HubUrlBuilder(string hostUrl)
    {
        var hostUri = new Uri(hostUrl);
        _signalUri = new Uri(hostUrl + "/signalr-hubs/");
    }

    public Uri GetTicTacToeHub()
    {
        return new Uri(_signalUri + "tic-tac-toe");
    }
}