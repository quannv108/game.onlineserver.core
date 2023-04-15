using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Session.Managers;

public interface IUserConnectionSessionManager : IDomainService
{
    Task<UserConnectionSession> CreateOrUpdateAsync(Guid userId, string connectionId);
    Task<UserConnectionSession> GetByUserIdAsync(Guid userId);
    Task DeleteAsync(Guid userId, string connectionId);
    Task<int> GetTotalSessionCountAsync();
}