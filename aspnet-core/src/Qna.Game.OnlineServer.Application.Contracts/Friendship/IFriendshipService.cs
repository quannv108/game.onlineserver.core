using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Friendship.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.Friendship;

public interface IFriendshipService : IApplicationService
{
    Task<List<FriendshipDto>> GetAllAsync();
    
    Task RequestFriendshipAsync(RequestFriendshipInput input);
    Task AnswerFriendshipRequestAsync(AnswerFriendshipRequestInput input);

    Task<List<FriendshipInvitationDto>> GetMyPendingInvitationsAsync();
}