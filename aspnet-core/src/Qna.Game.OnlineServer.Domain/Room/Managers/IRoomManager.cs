using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Session;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Room.Managers;

public interface IRoomManager : IDomainService
{
    Task<Room> CreateAsync(UserConnectionSession hostUser, long gameId);
    Task<Room> AutoJoinOrCreateAsync(UserConnectionSession session, long gameId);
    Task DeleteAsync(Room room);
    Task AddPlayerAsync(Room room, UserConnectionSession userConnectionSession);
    Task RemovePlayerAsync(Room room, Guid userId, string connectionId);
    List<Room> GetAll(Guid userId);
    Room Get(Guid roomId);
    Task UpdateRoomStateAsync(Room room);
}