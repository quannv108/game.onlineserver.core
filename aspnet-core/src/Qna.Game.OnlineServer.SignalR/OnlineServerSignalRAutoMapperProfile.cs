using AutoMapper;
using Qna.Game.OnlineServer.GamePlay;
using Qna.Game.OnlineServer.GamePlay.Players;
using Qna.Game.OnlineServer.SignalR.Contracts.Match;
using Qna.Game.OnlineServer.SignalR.Contracts.Users;

namespace Qna.Game.OnlineServer.SignalR;

public class OnlineServerSignalRAutoMapperProfile : Profile
{
    public OnlineServerSignalRAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Room.Room, GameMatchDto>()
            .ForMember(x => x.Players, y => y.MapFrom(z => z.Players.ToList()));
        CreateMap<GamePlayer, GamePlayerDto>();
    }
}