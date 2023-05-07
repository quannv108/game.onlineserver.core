using Volo.Abp.DependencyInjection;

namespace Qna.Game.OnlineServer.Notifications.Handlers;

public interface INotificationMessageNormalizer : ITransientDependency
{
    void Normalize(NotificationMessage message);
}