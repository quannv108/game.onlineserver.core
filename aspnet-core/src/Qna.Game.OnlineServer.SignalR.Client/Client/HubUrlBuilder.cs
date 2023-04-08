namespace Qna.Game.OnlineServer.SignalR.Client.Client;

public class HubUrlBuilder
{
    public Uri HostUrl { get; private set; }

    private Uri _signalUri;
    public HubUrlBuilder(string hostUrl)
    {
        HostUrl = new Uri(hostUrl);
        _signalUri = new Uri(hostUrl + "/signalr-hubs/");
    }

    public Uri GetMainHub()
    {
        return new Uri(_signalUri + "Main");
    }
}