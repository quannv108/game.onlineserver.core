using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Qna.Game.OnlineServer.Core;
using Qna.Game.OnlineServer.Core.Users;
using Qna.Game.OnlineServer.Friendship.Dto;
using Qna.Game.OnlineServer.Friendship.Managers;

namespace Qna.Game.OnlineServer.Friendship;

[Authorize]
public class FriendshipService : OnlineServerAppService, IFriendshipService
{
    private readonly IFriendshipManager _friendshipManager;

    public FriendshipService(IFriendshipManager friendshipManager)
    {
        _friendshipManager = friendshipManager;
    }

    public async Task<List<FriendshipDto>> GetAllAsync()
    {
        var userId = CurrentUser.GetUserId();
        var friends = await _friendshipManager.GetAllFriendsAsync(userId);
        return ObjectMapper.Map<List<Friendship>, List<FriendshipDto>>(friends);
    }

    public Task RequestFriendshipAsync(RequestFriendshipInput input)
    {
        var userId = CurrentUser.GetUserId();
        return _friendshipManager.CreateInvitationAsync(userId, input.ToUserId, input.Message);
    }

    public Task AnswerFriendshipRequestAsync(AnswerFriendshipRequestInput input)
    {
        var userId = CurrentUser.GetUserId();
        return _friendshipManager.AnswerInvitationAsync(input.RequestId, userId, input.ActionType);
    }

    public async Task<List<FriendshipInvitationDto>> GetMyPendingInvitationsAsync()
    {
        var userId = CurrentUser.GetUserId();
        var invitations = await _friendshipManager.GetAllPendingInvitationAsync(userId);
        return ObjectMapper.Map<List<FriendshipInvitation>, List<FriendshipInvitationDto>>(invitations);
    }
}