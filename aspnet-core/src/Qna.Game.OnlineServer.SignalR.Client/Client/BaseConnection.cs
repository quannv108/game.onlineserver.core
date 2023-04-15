using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Main;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;

namespace Qna.Game.OnlineServer.SignalR.Client.Client;

public abstract class BaseConnection : IAsyncDisposable, IClientConnection
{
    public HubConnectionState State => _connection.State;
    
    private readonly string _authHostUrl;
    private readonly UserCredentials _userUserCredentials;
    private readonly IMessageCallbackHandler _messageCallbackHandler;
    private readonly HubUrlBuilder _hubUrlBuilder;

    protected readonly HubConnection _connection;

    public BaseConnection(string authHostUrl, string signalRHostUrl,
        UserCredentials userUserCredentials,
        IMessageCallbackHandler messageCallbackHandler)
    {
        _authHostUrl = authHostUrl;
        _userUserCredentials = userUserCredentials;
        _hubUrlBuilder = new HubUrlBuilder(signalRHostUrl);
        var mainHubUri = _hubUrlBuilder.GetMainHub();
        
        _messageCallbackHandler = messageCallbackHandler;
        _messageCallbackHandler.Client = this;

        _connection = new HubConnectionBuilder()
            .WithUrl(mainHubUri, options => { options.AccessTokenProvider = GetAccessTokenAsync; })
            .WithAutomaticReconnect()
            .Build();
        RegisterCallback();
    }

    public ValueTask DisposeAsync()
    {
        return _connection.DisposeAsync();
    }

    public Task StartAsync()
    {
        return _connection.StartAsync();
    }

    public Task HelloAsync()
    {
        return _connection.SendAsync(nameof(IMainHubClientToServer.HelloAsync));
    }

    public Task AutoJoinMatchAsync(AutoJoinMatchInput input)
    {
        return _connection.SendAsync(nameof(IMainHubClientToServer.AutoJoinMatchAsync), input);
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        // return "eyJhbGciOiJSUzI1NiIsImtpZCI6IkY1NEFGMEE2NkRGQzVFOTY1OTQxQUMzQTI2Q0VDM0I3RDBCODE1RkIiLCJ4NXQiOiI5VXJ3cG0zOFhwWlpRYXc2SnM3RHQ5QzRGZnMiLCJ0eXAiOiJhdCtqd3QifQ.eyJzdWIiOiIzYTBhNzFhNy1jZDhhLTMxZmEtZGU2MS05ZmM5YzFkOGVmMjIiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZXN0dXNlciIsImVtYWlsIjoidGFydGFyb3MubnZxQGdtYWlsLmNvbSIsInBob25lX251bWJlcl92ZXJpZmllZCI6IkZhbHNlIiwiZW1haWxfdmVyaWZpZWQiOiJGYWxzZSIsInVuaXF1ZV9uYW1lIjoidGVzdHVzZXIiLCJvaV9wcnN0IjoiT25saW5lU2VydmVyX0FwcCIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzI1LyIsIm9pX2F1X2lkIjoiM2EwYTcxYjYtYTkwOC02NzE4LTM3YWUtNzFmYjZiYjcwYTgxIiwiY2xpZW50X2lkIjoiT25saW5lU2VydmVyX0FwcCIsIm9pX3Rrbl9pZCI6IjNhMGE3MWJiLWI5NWItZmQwOS1iY2Y0LTNkZjI4YTg4Mjk3ZSIsImF1ZCI6Ik9ubGluZVNlcnZlciIsInNjb3BlIjoib3BlbmlkIG9mZmxpbmVfYWNjZXNzIE9ubGluZVNlcnZlciIsImp0aSI6ImRmODYyMDNlLWZmNWEtNDRjZi1iNmVlLWE0YmIzYjM0NjA0OSIsImV4cCI6MTY4MDkzOTAxMiwiaWF0IjoxNjgwOTM1NDEyfQ.RGzbNGOSsudwaHqdx5N4iYlQ59mdBPD1kljBoIPfWFnI6I9T2231y5ftd9JApYlSlbYoEVOUQjVL3bCDLXHBZDvoVhvBdYMwtKuXAvpqjsQmcORoV7_jQU-qstThQfF0w01h5437CBEpDLoXtVTxQTLUkQa7Yjg4wMKy1gBumMfE2Ip1XFU0hU09D_mB2CEOX-Dlj_T5kjyqN3Zyl_8h6O8zrXwO-m7Eep8jo1rtTz-wDowZipOziMy1kF-E5RPHWIjac_dIm7GFp3yUCAj5rK-zAK0_4i3OeAaPpSRGRodEpH1TQnyln6NrQ7jps0K69jR_NDmYeTd_cE4ZHdpgnA";
        var authHostUri = new Uri(_authHostUrl);
        var authEndpoint = new Uri(authHostUri + "connect/token");
        using var httpClient = new HttpClient();
        var formContent = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
            { "grant_type", "password" },
            { "scope", "offline_access OnlineServer" },
            { "username", _userUserCredentials.Username },
            { "password", _userUserCredentials.Password },
            { "client_id", "OnlineServer_App" },
            { "client_secret", "" }
        });
        var response = await httpClient.PostAsync(authEndpoint, formContent);
        if (response is not { IsSuccessStatusCode: true })
            return null;
        var responseContentStream = await response.Content.ReadAsStreamAsync();
        var getTokenOutput = JsonSerializer.Deserialize<GetTokenOutput>(responseContentStream);

        return getTokenOutput?.AccessToken;
    }

    private void RegisterCallback()
    {
        // NOTE: if don't want to use any callback, can comment it out to speed up
        _connection.On(nameof(IMainHubServerToClient.HiAsync), () =>
        {
            _messageCallbackHandler.HiAsync();
        });
        _connection.On<string>(nameof(IMainHubServerToClient.ShowErrorAsync), (errorMessage) =>
        {
            _messageCallbackHandler.ShowErrorAsync(errorMessage);
        });
        _connection.On<GameMatchDto>(nameof(IMainHubServerToClient.MatchFoundAsync), (findMatchOutput) =>
        {
            _messageCallbackHandler.MatchFoundAsync(findMatchOutput);
        });
        _connection.On(nameof(IMainHubServerToClient.MultiLoginDetectedAsync), () =>
        {
            _messageCallbackHandler.MultiLoginDetectedAsync();
            _connection.StopAsync();
        });
        _connection.On<MatchPlayersUpdateEventDto>(nameof(IMainHubServerToClient.UpdateMatchPlayersAsync), (eventDto) =>
        {
            _messageCallbackHandler.UpdateMatchPlayersAsync(eventDto);
        });
        _connection.On<MatchStateEventUpdateDto>(nameof(IMainHubServerToClient.UpdateMatchStateAsync), (eventDto) =>
        {
            _messageCallbackHandler.UpdateMatchStateAsync(eventDto);
        });
        //TODO: add new when have new event from server
    }
}