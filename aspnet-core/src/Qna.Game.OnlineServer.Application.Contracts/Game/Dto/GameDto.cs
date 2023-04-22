using Volo.Abp.Application.Dtos;

namespace Qna.Game.OnlineServer.Game.Dto;

public class GameDto : EntityDto<long>
{
    public string Name { get; set; }
}