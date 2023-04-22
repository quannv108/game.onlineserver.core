using AutoMapper;
using Qna.Game.OnlineServer.Game.Dto;

namespace Qna.Game.OnlineServer;

public class OnlineServerApplicationAutoMapperProfile : Profile
{
    public OnlineServerApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Game.Game, GameDto>();
    }
}
