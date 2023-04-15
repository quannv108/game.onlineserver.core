using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Room.Storage;

public interface IRoomStorage : ISingletonDependency
{
    void Add(string key, Room newRoom);
    public List<Room> GetAll(string conditionKey);
    List<Room> GetAll(Guid userId);
    Room Get(Guid roomId);
    void Delete(string conditionKey, Guid roomId);
}