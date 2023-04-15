using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Room.Events;
using Qna.Game.OnlineServer.Room.Helpers;
using Qna.Game.OnlineServer.Room.MatchMaking;
using Qna.Game.OnlineServer.Room.StateMachine;
using Qna.Game.OnlineServer.Room.Storage;
using Qna.Game.OnlineServer.Session;
using Volo.Abp.Domain.Services;
using Volo.Abp.EventBus.Local;

namespace Qna.Game.OnlineServer.Room.Managers;

public class RoomManager : DomainService, IRoomManager
{
    private readonly IRoomStateMachine _roomStateMachine;
    private readonly IMatchMaking _matchMaking;
    private readonly IRoomStorage _roomStorage;
    private readonly ILocalEventBus _localEventBus;

    public RoomManager(IRoomStateMachine roomStateMachine,
        IMatchMaking matchMaking,
        IRoomStorage roomStorage,
        ILocalEventBus localEventBus)
    {
        _roomStateMachine = roomStateMachine;
        _matchMaking = matchMaking;
        _roomStorage = roomStorage;
        _localEventBus = localEventBus;
    }
    
    public Task<Room> CreateAsync(UserConnectionSession hostUser)
    {
        var room = new Room(hostUser)
        {
            State = RoomState.Matching,
            MaxPlayablePlayer = 2
        };
        
        _roomStorage.Add(_matchMaking.GetConditionKey(room), room);
        Logger.LogDebug($"user {hostUser.UserId} host new match {room.Id}");
        
        _localEventBus.PublishAsync(new RoomCreatedEvent
        {
            RoomId = room.Id,
            RoomName = room.GetRoomName()
        });
        return Task.FromResult(room);
    }

    public async Task<Room> AutoJoinOrCreateAsync(UserConnectionSession session)
    {
        var (conditionKey, room) = await _matchMaking.FindMatchAsync(session);
        if (room != null)
        {
            Logger.LogDebug($"user {session.UserId} found match {room.Id} still has slots to join");
            await AddPlayer(room, session);
            return room;
        }

        var newMatch = await CreateAsync(session);
        return newMatch;
    }

    public Task DeleteAsync(Room room)
    {
        var conditionKey = _matchMaking.GetConditionKey(room);
        _roomStorage.Delete(conditionKey, room.Id);
        _localEventBus.PublishAsync(new RoomDestroyedEvent
        {
            RoomId = room.Id
        });
        return Task.CompletedTask;
    }

    public async Task AddPlayer(Room room, UserConnectionSession userConnectionSession)
    {
        room.Players.Add(userConnectionSession.CurrentPlayer);
        
        await _localEventBus.PublishAsync(new RoomPlayerAddedEvent
        {
            RoomId = room.Id,
            NewPlayerConnectionId = userConnectionSession.ConnectionId
        });

        var roomStateChanged = _roomStateMachine.ProcessState(room);
        if (roomStateChanged)
        {
            await _localEventBus.PublishAsync(new RoomStateChangedEvent
            {
                RoomId = room.Id,
                RoomName = room.GetRoomName(),
                NewState = room.State
            });
        }
    }

    public async Task RemovePlayer(Room room, Guid userId, string connectionId)
    {
        var count = room.Players.RemoveAll(x => x.UserId == userId);
        if (count != 0)
        {
            await _localEventBus.PublishAsync(new RoomPlayerRemovedEvent
            {
                RoomId = room.Id,
                RemovedPlayerConnectionId = connectionId
                
            });
        }
    }

    public List<Room> GetAllAsync(Guid userId)
    {
        return _roomStorage.GetAll(userId);
    }

    public Room Get(Guid roomId)
    {
        return _roomStorage.Get(roomId);
    }
}