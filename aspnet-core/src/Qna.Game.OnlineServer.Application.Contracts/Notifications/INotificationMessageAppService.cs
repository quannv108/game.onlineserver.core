using System.Collections.Generic;
using System.Threading.Tasks;
using Qna.Game.OnlineServer.Notifications.Dto;
using Volo.Abp.Application.Services;

namespace Qna.Game.OnlineServer.Notifications;

public interface INotificationMessageAppService : IApplicationService
{
    Task<List<NotificationMessageDto>> GetAllAsync(bool onlyUnRead = false);
    Task MaskAsReadAsync(MaskAsReadNotificationMessageInput input);
    Task DeleteAsync(DeleteNotificationMessageInput input);
}