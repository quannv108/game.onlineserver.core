using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;
using Qna.Game.OnlineServer.SignalR.Client.Client.Core.Models;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR.Client.Client.Core;

public abstract class ConnectionBase : IAsyncDisposable, IClientConnection, IHubServerActionBase
{
    public HubConnectionState State => Connection.State;
    protected readonly HubConnection Connection;
    
    private readonly string _authHostUrl;
    private readonly UserCredentials _userUserCredentials;

    public ConnectionBase(string authHostUrl, string signalRHostUrl,
        UserCredentials userUserCredentials,
        IMessageCallbackHandler messageCallbackHandler)
    {
        _authHostUrl = authHostUrl;
        _userUserCredentials = userUserCredentials;
        var hubUrlBuilder = new HubUrlBuilder(signalRHostUrl);
        var mainHubUri = hubUrlBuilder.GetTicTacToeHub();
        
        messageCallbackHandler.Client = this;

        Connection = new HubConnectionBuilder()
            .WithUrl(mainHubUri, options =>
            {
                options.AccessTokenProvider = GetAccessTokenAsync;
                options.HttpMessageHandlerFactory = (message) =>
                {
                    if (message is HttpClientHandler clientHandler)
                        clientHandler.ServerCertificateCustomValidationCallback +=
                            (sender, certificate, chain, sslPolicyErrors) => { return true; }; //// ignore non-trusted self-signed certificate
                    return message;
                };
                // options.WebSocketConfiguration = conf =>
                // {
                //     conf.RemoteCertificateValidationCallback = (message, cert, chain, errors) => { return true; };
                // };
            })
            .WithAutomaticReconnect()
            .Build();
        RegisterCallback(messageCallbackHandler);
    }

    public ValueTask DisposeAsync()
    {
        return Connection.DisposeAsync();
    }

    public Task StartAsync()
    {
        return Connection.StartAsync();
    }

    public Task HelloAsync()
    {
        return Connection.SendAsync(nameof(IHubServerActionBase.HelloAsync));
    }

    public Task AutoJoinMatchAsync(AutoJoinMatchInput input)
    {
        return Connection.SendAsync(nameof(IMatchServerAction.AutoJoinMatchAsync), input);
    }

    public Task LeaveMatchAsync()
    {
        return Connection.SendAsync(nameof(IMatchServerAction.LeaveMatchAsync));
    }

    private async Task<string?> GetAccessTokenAsync()
    {
        // return "eyJhbGciOiJSUzI1NiIsImtpZCI6IkY1NEFGMEE2NkRGQzVFOTY1OTQxQUMzQTI2Q0VDM0I3RDBCODE1RkIiLCJ4NXQiOiI5VXJ3cG0zOFhwWlpRYXc2SnM3RHQ5QzRGZnMiLCJ0eXAiOiJhdCtqd3QifQ.eyJzdWIiOiIzYTBhNzFhNy1jZDhhLTMxZmEtZGU2MS05ZmM5YzFkOGVmMjIiLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ0ZXN0dXNlciIsImVtYWlsIjoidGFydGFyb3MubnZxQGdtYWlsLmNvbSIsInBob25lX251bWJlcl92ZXJpZmllZCI6IkZhbHNlIiwiZW1haWxfdmVyaWZpZWQiOiJGYWxzZSIsInVuaXF1ZV9uYW1lIjoidGVzdHVzZXIiLCJvaV9wcnN0IjoiT25saW5lU2VydmVyX0FwcCIsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjQ0MzI1LyIsIm9pX2F1X2lkIjoiM2EwYTcxYjYtYTkwOC02NzE4LTM3YWUtNzFmYjZiYjcwYTgxIiwiY2xpZW50X2lkIjoiT25saW5lU2VydmVyX0FwcCIsIm9pX3Rrbl9pZCI6IjNhMGE3MWJiLWI5NWItZmQwOS1iY2Y0LTNkZjI4YTg4Mjk3ZSIsImF1ZCI6Ik9ubGluZVNlcnZlciIsInNjb3BlIjoib3BlbmlkIG9mZmxpbmVfYWNjZXNzIE9ubGluZVNlcnZlciIsImp0aSI6ImRmODYyMDNlLWZmNWEtNDRjZi1iNmVlLWE0YmIzYjM0NjA0OSIsImV4cCI6MTY4MDkzOTAxMiwiaWF0IjoxNjgwOTM1NDEyfQ.RGzbNGOSsudwaHqdx5N4iYlQ59mdBPD1kljBoIPfWFnI6I9T2231y5ftd9JApYlSlbYoEVOUQjVL3bCDLXHBZDvoVhvBdYMwtKuXAvpqjsQmcORoV7_jQU-qstThQfF0w01h5437CBEpDLoXtVTxQTLUkQa7Yjg4wMKy1gBumMfE2Ip1XFU0hU09D_mB2CEOX-Dlj_T5kjyqN3Zyl_8h6O8zrXwO-m7Eep8jo1rtTz-wDowZipOziMy1kF-E5RPHWIjac_dIm7GFp3yUCAj5rK-zAK0_4i3OeAaPpSRGRodEpH1TQnyln6NrQ7jps0K69jR_NDmYeTd_cE4ZHdpgnA";
        var authHostUri = new Uri(_authHostUrl);
        var authEndpoint = new Uri(authHostUri + "connect/token");
        using var httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, certChain, policyErrors) => true // ignore non-trusted self-signed certificate
        });
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
        {
            throw new Exception("unauthorized: " + response.Content.ReadAsStringAsync().Result);
        }
        var responseContentStream = await response.Content.ReadAsStreamAsync();
        var getTokenOutput = JsonSerializer.Deserialize<GetTokenOutput>(responseContentStream);
        return getTokenOutput?.AccessToken;
    }

    private void RegisterCallback(IMessageCallbackHandler messageCallbackHandler)
    {
        // NOTE: if don't want to use any callback, can comment it out to speed up
        Connection.On<GamePlayerDto>(nameof(IHubClientActionBase.HiAsync), (gamePlayer) =>
        {
            messageCallbackHandler.HiAsync(gamePlayer);
        });
        Connection.On<string>(nameof(IHubClientActionBase.ShowErrorAsync), (errorMessage) =>
        {
            messageCallbackHandler.ShowErrorAsync(errorMessage);
        });
        Connection.On<GameMatchDto>(nameof(IHubClientActionBase.MatchFoundAsync), (findMatchOutput) =>
        {
            messageCallbackHandler.MatchFoundAsync(findMatchOutput);
        });
        Connection.On(nameof(IHubClientActionBase.MultiLoginDetectedAsync), () =>
        {
            messageCallbackHandler.MultiLoginDetectedAsync();
            Connection.StopAsync();
        });
        Connection.On<MatchPlayersUpdateEventDto>(nameof(IHubClientActionBase.UpdateMatchPlayersAsync), (eventDto) =>
        {
            messageCallbackHandler.UpdateMatchPlayersAsync(eventDto);
        });
        Connection.On<MatchStateEventUpdateDto>(nameof(IHubClientActionBase.UpdateMatchStateAsync), (eventDto) =>
        {
            messageCallbackHandler.UpdateMatchStateAsync(eventDto);
        });
        //TODO: add new when have new event from server
    }
}