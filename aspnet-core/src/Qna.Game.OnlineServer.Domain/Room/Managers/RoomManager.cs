using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Qna.Game.OnlineServer.Game;
using Qna.Game.OnlineServer.Game.Managers;
using Qna.Game.OnlineServer.GamePlay.Managers;
using Qna.Game.OnlineServer.Match;
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

    private IGameManager GameManager => LazyServiceProvider.LazyGetRequiredService<IGameManager>();
    private IGamePlayManager GamePlayManager => LazyServiceProvider.LazyGetRequiredService<IGamePlayManager>();

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
    
    public async Task<Room> CreateAsync(UserConnectionSession hostUser, long gameId)
    {
        var room = new Room
        {
            State = RoomState.Matching,
            MaxPlayablePlayer = 2, // TODO: bring this to Game.Game Entity
            GameId = gameId,
            ConditionKey = _matchMaking.GetConditionKey(hostUser.CurrentPlayer, gameId)
        };
        
        _roomStorage.Add(room.ConditionKey, room);
        Logger.LogDebug($"user {hostUser.UserId} create new room {room.Id}");
        
        await _localEventBus.PublishAsync(new RoomCreatedEvent
        {
            RoomId = room.Id,
            RoomName = room.GetRoomName()
        }, false);
        
        await AddPlayerAsync(room, hostUser);
        return room;
    }

    public async Task<Room> AutoJoinOrCreateAsync(UserConnectionSession session, long gameId)
    {
        var (_, room) = await _matchMaking.FindMatchAsync(session, gameId);
        if (room != null)
        {
            Logger.LogDebug($"user {session.UserId} found room {room.Id} has slots to join");
            await AddPlayerAsync(room, session);
            return room;
        }

        return await CreateAsync(session, gameId);
    }

    public Task DeleteAsync(Room room)
    {
        var conditionKey = room.ConditionKey;
        _roomStorage.Delete(conditionKey, room.Id);
        Logger.LogDebug($"room {room.Id} destroyed");
        
        return _localEventBus.PublishAsync(new RoomDestroyedEvent
        {
            RoomId = room.Id
        }, false);
    }

    public Task AddPlayerAsync(Room room, UserConnectionSession userConnectionSession)
    {
        room.Players.Add(userConnectionSession.CurrentPlayer);
        Logger.LogDebug($"room {room.Id} add new user {userConnectionSession.UserId}");

        // no await
        _localEventBus.PublishAsync(new RoomPlayerAddedEvent
        {
            RoomId = room.Id,
            NewPlayerConnectionId = userConnectionSession.ConnectionId
        }, false);

        return UpdateRoomStateAsync(room);
    }

    public Task RemovePlayerAsync(Room room, Guid userId, string connectionId)
    {   
        var count = room.Players.RemoveAll(x => x.UserId == userId);
        Logger.LogDebug($"room {room.Id} remove user {userId}: {count}");
        
        if (count != 0)
        {
            // no await
            _localEventBus.PublishAsync(new RoomPlayerRemovedEvent
            {
                RoomId = room.Id,
                RemovedPlayerConnectionId = connectionId
            }, false);
            
            UpdateRoomStateAsync(room);
        }

        return Task.CompletedTask;
    }

    public List<Room> GetAll(Guid userId)
    {
        return _roomStorage.GetAll(userId);
    }

    public Room Get(Guid roomId)
    {
        return _roomStorage.Get(roomId);
    }

    public Task UpdateRoomStateAsync(Room room)
    {
        var roomStateChanged = _roomStateMachine.ProcessState(room);
        if (!roomStateChanged)
        {
            return Task.CompletedTask;
        }

        Logger.LogDebug($"Room {room.Id} change status to {room.State.ToString()}");
        _localEventBus.PublishAsync(new RoomStateChangedEvent
        {
            RoomId = room.Id,
            RoomName = room.GetRoomName(),
            NewState = room.State
        }, false);
        
        // data changed depend on new state
        // no await
        return UpdateGameBaseOnRoomStateChangedAsync(room);
    }

    private async Task UpdateGameBaseOnRoomStateChangedAsync(Room room)
    {
        switch (room.State)
        {
            case RoomState.ReadyForPlay:
            {
                // TODO: below logic should be run on GameManager
                // start game
                var gameInfo = await GameManager.GetAsync(room.GameId);
                GamePlayManager.CreateAndStartGameLoop(gameInfo, room);

                // update room state
                await UpdateRoomStateAsync(room);
                break;
            }
            case RoomState.Ended:
            {
                // TODO: scoring update
                // TODO: leaderboard update
                // TODO: user statistic update

                // clean up & change state
                room.GameLoop = null;
                RemoveRoomIfNoPlayerAndGameEnded(room);

                break;
            }
        }
    }

    private void RemoveRoomIfNoPlayerAndGameEnded(Room room)
    {
        if (room.State == RoomState.Ended && room.TotalCurrentPlayers == 0)
        {
            DeleteAsync(room);
        }
    }
}