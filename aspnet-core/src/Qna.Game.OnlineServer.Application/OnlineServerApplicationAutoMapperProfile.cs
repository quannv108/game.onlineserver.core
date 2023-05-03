using AutoMapper;
using Qna.Game.OnlineServer.Friendship;
using Qna.Game.OnlineServer.Friendship.Dto;
using Qna.Game.OnlineServer.Game.Dto;
using Volo.Abp.Identity;

namespace Qna.Game.OnlineServer;

public class OnlineServerApplicationAutoMapperProfile : Profile
{
    public OnlineServerApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Game.Game, GameDto>();

        CreateMap<Friendship.Friendship, FriendshipDto>();
        CreateMap<IdentityUser, FriendUserDto>();
        CreateMap<FriendshipInvitation, FriendshipInvitationDto>()
            .ForMember(x => x.RequestTime, y => y.MapFrom(z => z.CreationTime));
    }
}
