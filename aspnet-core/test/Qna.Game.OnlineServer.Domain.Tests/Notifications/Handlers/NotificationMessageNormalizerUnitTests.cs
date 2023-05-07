using System;
using Shouldly;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Qna.Game.OnlineServer.Notifications.Handlers;

public class NotificationMessageNormalizerUnitTests
{
    private readonly NotificationMessageNormalizer _notificationMessageNormalizer;
    public NotificationMessageNormalizerUnitTests()
    {
        _notificationMessageNormalizer = new NotificationMessageNormalizer();
    }

    [Fact]
    public void Normalize_ShouldSucceed_WhenNormalizeFromTemplate()
    {
        var parameters = new
        {
            Title = new
            {
                Name = "quan nguyen"
            },
            Content = new
            {
                Time = new DateTime(2023, 08, 12, 12, 13, 14),
                Count = 123,
                Number = 3.433434349,
                Enabled = true
            }
        };
        var parametersStr = JsonSerializer.Serialize(parameters);
        var message = new NotificationMessage
        {
            Type = NotificationMessageType.Template,
            Template = new NotificationMessageTemplate
            {
                Title = "hello {{Name}}",
                Content = "this is us from future, this is time {{Time}}, count {{ Count }}, format {{ Number }}, boolean {{Enabled}}"
            },
            TemplateParameters = parametersStr
        };
        _notificationMessageNormalizer.Normalize(message);
        message.Title.ShouldNotBeNull();
        message.Title.ShouldBe("hello quan nguyen");
        message.Content.ShouldContain("this is us from future, this is time 2023-08-12T12:13:14");
        message.Content.ShouldContain("count 123");
        message.Content.ShouldContain("format 3.433434349");
        message.Content.ShouldContain("boolean true");
    }
}