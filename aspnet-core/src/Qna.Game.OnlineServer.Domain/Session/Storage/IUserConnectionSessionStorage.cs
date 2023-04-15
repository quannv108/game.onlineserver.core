using System;
using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Session.Storage;

public interface IUserConnectionSessionStorage : ISingletonDependency
{
    UserConnectionSession GetByUserId(Guid userId);

    UserConnectionSession Delete(Guid userId, string connectionId);
    void Update(UserConnectionSession session);
    void Create(UserConnectionSession session);
    int Count();
}