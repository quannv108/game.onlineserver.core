using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Repository;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Finance.Managers;

public class UserFinanceManager : DomainService, IUserFinanceManager
{
    private readonly IRepository<UserFinance, Guid> _userFinanceRepository;

    public UserFinanceManager(IRepository<UserFinance, Guid> userFinanceRepository)
    {
        _userFinanceRepository = userFinanceRepository;
    }
    
    public Task<UserFinance> CreateAsync(UserFinance finance)
    {
        return _userFinanceRepository.InsertAsync(finance);
    }

    public Task<UserFinance> GetAsync(Guid userId)
    {
        return _userFinanceRepository.GetAll().AsNoTracking().SingleAsync(x => x.UserId == userId);
    }

    public Task<UserFinance> UpdateAsync(UserFinance finance)
    {
        return _userFinanceRepository.UpdateAsync(finance);
    }
}