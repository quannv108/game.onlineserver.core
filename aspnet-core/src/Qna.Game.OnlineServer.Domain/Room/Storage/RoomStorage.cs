using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Concurrency;

namespace Qna.Game.OnlineServer.Room.Storage;


using ConditionKey = String;

public class RoomStorage : IRoomStorage
{
    /// <summary>
    /// condition as key
    /// </summary>
    private ConcurrentDictionary<ConditionKey, ConcurrentBag<Room>> _conditionRooms = new();

    private SynchronizeDictionary<Guid, Room> _rooms = new();

    public void Add(string conditionKey, Room newRoom)
    {
        _conditionRooms[conditionKey].Add(newRoom);
        _rooms.SetOrUpdate(newRoom.Id, newRoom);
    }

    public List<Room> GetAll(string conditionKey)
    {
        if (!_conditionRooms.ContainsKey(conditionKey))
        {
            _conditionRooms.TryAdd(conditionKey, new ConcurrentBag<Room>());
        }

        return _conditionRooms[conditionKey].ToList();
    }

    public List<Room> GetAll(Guid userId)
    {
        return _rooms.Where(x => x.Players.Any(p => p.UserId == userId));
    }
    public Room Get(Guid roomId)
    {
        return _rooms.GetOrDefault(roomId);
    }

    public void Delete(string conditionKey, Guid roomId)
    {
        _rooms.TryRemove(roomId, out var room);
        Task.Run(() =>
        {
            var existingRoom = _conditionRooms[conditionKey].ToList();
            existingRoom.Remove(room);
            _conditionRooms[conditionKey] = new ConcurrentBag<Room>(existingRoom);
        });
    }

}