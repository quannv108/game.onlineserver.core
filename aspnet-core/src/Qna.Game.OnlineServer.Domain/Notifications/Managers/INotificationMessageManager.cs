using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Notifications.Managers;

public interface INotificationMessageManager : IDomainService
{
    Task<List<NotificationMessage>> GetAllAsync(Guid userId, bool onlyUnRead = false);
    Task DeleteAsync(Guid messageId, Guid userId);
    Task MaskAsReadAsync(Guid messageId, Guid userId);
    Task CreateAsync(Guid userId, string title, string content);
    Task CreateAsync(Guid userId, long templateId, ITemplateParameters parameters);
    
    Task CreateTemplateAsync(string templateTitle, string templateContent);
    Task UpdateTemplateAsync(long templateId, string templateTitle, string templateContent);
}