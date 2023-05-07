using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Qna.Game.OnlineServer.Notifications.Dto;

public class NotificationMessageDto : EntityDto<Guid>
{
    public bool IsRead { get; set; }
    [MaxLength(NotificationMessageConsts.TitleMaxLength)]
    public string Title { get; set; }
    [MaxLength(NotificationMessageConsts.ContentMaxLength)]
    public string Content { get; set; }
}