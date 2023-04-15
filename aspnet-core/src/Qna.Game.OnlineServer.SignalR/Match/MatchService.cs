using Microsoft.AspNetCore.SignalR;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.InGame;
using Qna.Game.OnlineServer.Room.Events;
using Qna.Game.OnlineServer.Room.Helpers;
using Qna.Game.OnlineServer.Room.Managers;
using Qna.Game.OnlineServer.Session;
using Qna.Game.OnlineServer.Session.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Match.Events;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;
using Qna.Game.OnlineServer.SignalR.Hub.Main;
using Volo.Abp;
using Volo.Abp.EventBus;

namespace Qna.Game.OnlineServer.SignalR.Match;

[RemoteService(IsMetadataEnabled = false)]
public class MatchService : MainHubAppService, IMatchService,
    ILocalEventHandler<UserSessionRemovedEvent>,
    ILocalEventHandler<RoomPlayerAddedEvent>,
    ILocalEventHandler<RoomPlayerRemovedEvent>,
    ILocalEventHandler<RoomStateChangedEvent>
{
    private readonly IRoomManager _roomManager;

    public MatchService(
        IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }
    
    public async Task<Room.Room> AutoJoinAsync(UserConnectionSession session)
    {
        if (session.CurrentMatch != null)
        {
            throw new UserFriendlyException("existing in a match already");
        }
        var match = await _roomManager.AutoJoinOrCreateAsync(session);
        session.CurrentMatch = match;
        Logger.LogDebug($"user {session.UserId} will join room {match.Id}");

        var matchDto = ObjectMapper.Map<Room.Room, GameMatchDto>(match);
        await GetClient(session.ConnectionId).MatchFoundAsync(matchDto);
        
        return match;
    }

    public Task HandleEventAsync(UserSessionRemovedEvent eventData)
    {
        var userId = eventData.UserId;
        var matchs = _roomManager.GetAllAsync(userId);
        Logger.LogDebug($"MatchService: UserSessionRemovedEvent: {eventData.UserId}, " +
                        $"old connectionId = {eventData.ConnectionId}, appear in {matchs.Count} matchs");
        foreach (var m in matchs)
        {
            _roomManager.RemovePlayer(m, userId, eventData.ConnectionId);
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
                Players = ObjectMapper.Map<List<GamePlayer>, List<GamePlayerDto>>(room.Players)
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
            Players = ObjectMapper.Map<List<GamePlayer>, List<GamePlayerDto>>(room.Players)
        });
    }

    public async Task HandleEventAsync(RoomStateChangedEvent eventData)
    {
        await Clients.Groups(eventData.RoomName)
            .UpdateMatchStateAsync(new MatchStateEventUpdateDto()
            {
                State = eventData.NewState
            });

        if (eventData.NewState == RoomState.ReadyForPlay)
        {
            // start game
            // _gameService.Start(match);
        }
    }
}