using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Qna.Game.OnlineServer.Core.Repository;
using Qna.Game.OnlineServer.Notifications.Handlers;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Qna.Game.OnlineServer.Notifications.Managers;

public class NotificationMessageManager : DomainService, INotificationMessageManager
{
    private readonly IRepository<NotificationMessage, Guid> _notificationMessageRepository;
    private readonly IRepository<NotificationMessageTemplate, long> _templateRepository;
    private readonly INotificationMessageNormalizer _notificationMessageNormalizer;

    public NotificationMessageManager(
        IRepository<NotificationMessage, Guid> notificationMessageRepository,
        IRepository<NotificationMessageTemplate, long> templateRepository,
        INotificationMessageNormalizer notificationMessageNormalizer)
    {
        _notificationMessageRepository = notificationMessageRepository;
        _templateRepository = templateRepository;
        _notificationMessageNormalizer = notificationMessageNormalizer;
    }

    public async Task<List<NotificationMessage>> GetAllAsync(Guid userId, bool onlyUnRead)
    {
        var messagesQuery = _notificationMessageRepository.GetAll().AsNoTracking()
            .Include(m => m.Template)
            .Where(x => x.UserId == userId);
        if (onlyUnRead)
        {
            messagesQuery = messagesQuery.Where(m => m.IsRead == false);
        }

        var messages = await messagesQuery
            .OrderByDescending(m => m.CreationTime)
            .ToListAsync();
        messages.ForEach(m => _notificationMessageNormalizer.Normalize(m));

        return messages;
    }

    public async Task DeleteAsync(Guid messageId, Guid userId)
    {
        var message = await _notificationMessageRepository.GetAsync(messageId);
        if (message.UserId != userId)
        {
            throw new UserFriendlyException("not allowed");
        }

        await _notificationMessageRepository.DeleteAsync(messageId);
    }

    public async Task MaskAsReadAsync(Guid messageId, Guid userId)
    {
        var message = await _notificationMessageRepository.GetAsync(messageId);
        if (message.UserId != userId)
        {
            throw new UserFriendlyException("not allowed");
        }

        message.IsRead = true;
        // entity is tracked, so UnitOfWork will take care of update the entity
    }

    public Task CreateAsync(Guid userId, string title, string content)
    {
        var message = new NotificationMessage
        {
            Title = title,
            Content = content,
            UserId = userId,
            Type = NotificationMessageType.DirectContent
        };
        return _notificationMessageRepository.InsertAsync(message);
    }

    public Task CreateAsync(Guid userId, long templateId, ITemplateParameters parameters)
    {
        var message = new NotificationMessage
        {
            TemplateId = templateId,
            UserId = userId,
            TemplateParameters = JsonSerializer.Serialize(parameters),
            Type = NotificationMessageType.Template
        };
        return _notificationMessageRepository.InsertAsync(message);
    }

    public async Task CreateTemplateAsync(string templateTitle, string templateContent)
    {
        var template = new NotificationMessageTemplate
        {
            Title = templateTitle,
            Content = templateContent
        };
        await _templateRepository.InsertAsync(template);
    }

    public async Task UpdateTemplateAsync(long templateId, string templateTitle, string templateContent)
    {
        var template = await _templateRepository.GetAsync(templateId);
        template.Title = templateTitle;
        template.Content = templateContent;
        
        // entity is tracked, so UnitOfWork will take care of update the entity
    }
}