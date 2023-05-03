using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Finance.Managers;

public interface IUserFinanceManager : IDomainService
{
    Task<UserFinance> CreateAsync(UserFinance finance);
    Task<UserFinance> GetAsync(Guid userId);
    Task<UserFinance> UpdateAsync(UserFinance finance);
}