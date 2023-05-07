using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Qna.Game.OnlineServer.Core.Users;
using Qna.Game.OnlineServer.Notifications.Dto;
using Qna.Game.OnlineServer.Notifications.Managers;

namespace Qna.Game.OnlineServer.Notifications;

[Authorize]
public class NotificationMessageAppService : OnlineServerAppService, INotificationMessageAppService
{
    private readonly INotificationMessageManager _notificationMessageManager;

    public NotificationMessageAppService(INotificationMessageManager notificationMessageManager)
    {
        _notificationMessageManager = notificationMessageManager;
    }

    public async Task<List<NotificationMessageDto>> GetAllAsync(bool onlyUnRead)
    {
        var userId = CurrentUser.GetUserId();
        var messages = await _notificationMessageManager.GetAllAsync(userId, onlyUnRead);
        return ObjectMapper.Map<List<NotificationMessage>, List<NotificationMessageDto>>(messages);
    }

    public Task MaskAsReadAsync(MaskAsReadNotificationMessageInput input)
    {
        var userId = CurrentUser.GetUserId();
        return _notificationMessageManager.MaskAsReadAsync(input.Id, userId);
    }

    public Task DeleteAsync(DeleteNotificationMessageInput input)
    {
        var userId = CurrentUser.GetUserId();
        return _notificationMessageManager.DeleteAsync(input.Id, userId);
    }
}