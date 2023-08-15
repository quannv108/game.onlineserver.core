using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Qna.Game.OnlineServer.Version.Dto;

namespace Qna.Game.OnlineServer.Version;

[AllowAnonymous]
public class VersionAppService : OnlineServerAppService, IVersionAppService
{
    public Task<VersionDto> GetAsync()
    {
        var versionFromEnv = Environment.GetEnvironmentVariable("VERSION");
        var tagFromEnv = Environment.GetEnvironmentVariable("TAG");
        return Task.FromResult(new VersionDto
        {
            Version = versionFromEnv.IsNullOrEmpty() ? "0.0" : versionFromEnv,
            Tag = tagFromEnv.IsNullOrEmpty() ? "" : tagFromEnv
        });
    }
}