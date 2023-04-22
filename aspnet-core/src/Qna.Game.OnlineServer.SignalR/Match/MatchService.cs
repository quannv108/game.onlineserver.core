using Microsoft.AspNetCore.SignalR;
using Qna.Game.OnlineServer.GamePlay.Players;
using Qna.Game.OnlineServer.Room.Events;
using Qna.Game.OnlineServer.Room.Helpers;
using Qna.Game.OnlineServer.Room.Managers;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.Session.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Hub.Core;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;
using Volo.Abp;
using Volo.Abp.EventBus;

namespace Qna.Game.OnlineServer.SignalR.Match;

[RemoteService(false)]
public class MatchService<THub, TClientAction> : SignalRServiceBase<THub, TClientAction>,
    IMatchService,
    ILocalEventHandler<UserSessionRemovedEvent>,
    ILocalEventHandler<RoomPlayerAddedEvent>,
    ILocalEventHandler<RoomPlayerRemovedEvent>,
    ILocalEventHandler<RoomStateChangedEvent>
    where THub : Hub<TClientAction>
    where TClientAction : class, IHubClientActionBase
{
    private readonly IRoomManager _roomManager;

    public MatchService(
        IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public async Task<Room.Room> AutoJoinAsync(UserConnectionSession session, long gameId)
    {
        if (session.CurrentMatch != null)
        {
            throw new UserFriendlyException("existing in a match already");
        }

        var match = await _roomManager.AutoJoinOrCreateAsync(session, gameId);
        session.CurrentMatch = match;

        var matchDto = ObjectMapper.Map<Room.Room, GameMatchDto>(match);
        await GetClient(session.ConnectionId).MatchFoundAsync(matchDto);

        return match;
    }

    public async Task LeaveMatchAsync(UserConnectionSession session)
    {
        var match = session.CurrentMatch;
        await _roomManager.RemovePlayerAsync(match, session.UserId, session.ConnectionId);
        session.CurrentMatch = null;
    }

    public Task HandleEventAsync(UserSessionRemovedEvent eventData)
    {
        var userId = eventData.UserId;
        var matchs = _roomManager.GetAll(userId);
        Logger.LogDebug($"MatchService: UserSessionRemovedEvent: userId = {eventData.UserId}, " +
                        $"old connectionId = {eventData.ConnectionId}, existing in {matchs.Count} room");
        foreach (var m in matchs)
        {
            _roomManager.RemovePlayerAsync(m, userId, eventData.ConnectionId); // this line should be on SessionManager

            var roomName = m.GetRoomName();
            Groups.RemoveFromGroupAsync(eventData.ConnectionId, roomName);
        }

        return Task.CompletedTask;
    }

    public async Task HandleEventAsync(RoomPlayerAddedEvent eventData)
    {
        var room = _roomManager.Get(eventData.RoomId);
        var roomName = room.GetRoomName();

        await Clients.Groups(roomName)
            .UpdateMatchPlayersAsync(new MatchPlayersUpdateEventDto
            {
                Players = ObjectMapper.Map<List<GamePlayer>, List<GamePlayerDto>>(room.Players.ToList())
            });

        await Groups.AddToGroupAsync(eventData.NewPlayerConnectionId, roomName);
    }

    public async Task HandleEventAsync(RoomPlayerRemovedEvent eventData)
    {
        var room = _roomManager.Get(eventData.RoomId);
        var roomName = room.GetRoomName();

        await Groups.RemoveFromGroupAsync(eventData.RemovedPlayerConnectionId, roomName);
        await Clients.Groups(roomName).UpdateMatchPlayersAsync(new MatchPlayersUpdateEventDto
        {
            Players = ObjectMapper.Map<List<GamePlayer>, List<GamePlayerDto>>(room.Players.ToList())
        });
    }

    public Task HandleEventAsync(RoomStateChangedEvent eventData)
    {
        return Clients.Groups(eventData.RoomName)
            .UpdateMatchStateAsync(new MatchStateEventUpdateDto()
            {
                State = eventData.NewState
            });
    }
}