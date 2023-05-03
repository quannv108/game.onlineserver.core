using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Friendship.Managers;

public interface IFriendshipManager : IDomainService
{
    Task CreateInvitationAsync(Guid fromUserId, Guid toUserId, string message);
    Task AnswerInvitationAsync(Guid requestId, Guid actionUserId, FriendshipRequestActionType actionType);
    
    Task<List<FriendshipInvitation>> GetAllPendingInvitationAsync(Guid userId);
    Task<List<FriendshipInvitation>> GetAllSentInvitationAsync(Guid userId);
    
    Task<List<Friendship>> GetAllFriendsAsync(Guid userId);
}