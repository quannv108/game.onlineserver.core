using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Qna.Game.OnlineServer.Friendship.Helpers;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Friendship.Managers;

public class FriendshipManager : DomainService, IFriendshipManager
{
    private readonly IRepository<FriendshipInvitation, Guid> _friendRequestRepository;

    public FriendshipManager(IRepository<FriendshipInvitation, Guid> friendRequestRepository)
    {
        _friendRequestRepository = friendRequestRepository;
    }

    public async Task CreateInvitationAsync(Guid fromUserId, Guid toUserId, string message)
    {
        var existing = await _friendRequestRepository.GetAll().AsNoTracking()
            .Where(x => x.FromUserId == fromUserId
                                 && x.ToUserId == toUserId)
            .ToListAsync();
        if (existing.Any(x => x.Status == FriendRequestInvitationStatus.Sent))
        {
            throw new UserFriendlyException("already sent invitation to this user");
        }
        
        if (existing.Any(x => x.Status == FriendRequestInvitationStatus.Accepted))
        {
            throw new UserFriendlyException("already accept buddy for this user");
        }

        if (existing.Any(x => x.Status == FriendRequestInvitationStatus.Blocked))
        {
            throw new UserFriendlyException("can't sent to blocked user");
        }

        var invitation = new FriendshipInvitation
        {
            FromUserId = fromUserId,
            ToUserId = toUserId,
            Message = message,
            Status = FriendRequestInvitationStatus.Sent
        };
        await _friendRequestRepository.InsertAsync(invitation);
    }

    public async Task AnswerInvitationAsync(Guid requestId, Guid actionUserId, FriendshipRequestActionType actionType)
    {
        var invitation = await GetAsync(requestId);
        if (invitation.ToUserId != actionUserId)
        {
            throw new UserFriendlyException("not allowed");
        }
        if (invitation.Status != FriendRequestInvitationStatus.Sent)
        {
            throw new UserFriendlyException("already change status before");
        }

        var newStatus = actionType.ToInvitationStatus();
        invitation.Status = newStatus;

        await _friendRequestRepository.UpdateAsync(invitation);
    }

    public Task<List<FriendshipInvitation>> GetAllPendingInvitationAsync(Guid userId)
    {
        return _friendRequestRepository.GetAll().AsNoTracking()
            .Include(x => x.FromUser)
            .Where(x => x.ToUserId == userId && x.Status == FriendRequestInvitationStatus.Sent)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();
    }

    public Task<List<FriendshipInvitation>> GetAllSentInvitationAsync(Guid userId)
    {
        return _friendRequestRepository.GetAll().AsNoTracking()
            .Include(x => x.ToUser)
            .Where(x => x.FromUserId == userId)
            .OrderByDescending(x => x.CreationTime)
            .ToListAsync();
    }

    public async Task<List<Friendship>> GetAllFriendsAsync(Guid userId)
    {
        var acceptedRequests = await _friendRequestRepository.GetAll().AsNoTracking()
            .Include(x => x.FromUser)
            .Include(x => x.ToUser)
            .Where(x => x.Status == FriendRequestInvitationStatus.Accepted &&
                        (x.FromUserId == userId || x.ToUserId == userId))
            .OrderByDescending(x => x.LastModificationTime)
            .ToListAsync();
        return acceptedRequests.Select(x => x.ToFriendship(userId)).ToList();
    }

    private Task<FriendshipInvitation> GetAsync(Guid requestId)
    {
        return _friendRequestRepository.GetAll().AsNoTracking()
            .Include(x => x.ToUser)
            .Include(x => x.FromUser)
            .FirstOrDefaultAsync(x => x.Id == requestId);
    }
}