using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Session;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Room.Managers;

public interface IRoomManager : IDomainService
{
    Task<Room> CreateAsync(UserConnectionSession hostUser);
    Task<Room> AutoJoinOrCreateAsync(UserConnectionSession session);
    Task DeleteAsync(Room room);
    Task AddPlayer(Room room, UserConnectionSession userConnectionSession);
    Task RemovePlayer(Room room, Guid userId, string connectionId);
    List<Room> GetAllAsync(Guid userId);
    Room Get(Guid roomId);
}