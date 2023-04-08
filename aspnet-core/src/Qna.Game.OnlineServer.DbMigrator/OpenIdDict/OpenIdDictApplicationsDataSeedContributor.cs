using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.OpenIddict.Applications;

namespace Qna.Game.OnlineServer.OpenIdDict;

public class OpenIdDictApplicationsDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<OpenIddictApplication, Guid> _openIdDictApplicationRepository;

    public OpenIdDictApplicationsDataSeedContributor(
        IRepository<OpenIddictApplication, Guid> openIdDictApplicationRepository)
    {
        _openIdDictApplicationRepository = openIdDictApplicationRepository;
    }
    
    public async Task SeedAsync(DataSeedContext context)
    {
        await CreateForSignalRSwaggerAsync();
    }

    private async Task CreateForSignalRSwaggerAsync()
    {
        const string clientId = "Swagger_SignalR";
        if (await _openIdDictApplicationRepository.AnyAsync(x => x.ClientId == clientId))
        {
            return;
        }

        var referenceConfig = await _openIdDictApplicationRepository
            .SingleAsync(x => x.ClientId == "OnlineServer_Swagger");
        var newConfig = new OpenIddictApplication
        {
            ClientId = clientId,
            DisplayName = "Swagger SignalR",
            ClientSecret = referenceConfig.ClientSecret,
            ConsentType = referenceConfig.ConsentType,
            Permissions = referenceConfig.Permissions,
            RedirectUris = "[\"https://localhost:44335/swagger/oauth2-redirect.html\"]",
            PostLogoutRedirectUris = referenceConfig.PostLogoutRedirectUris,
            Properties = referenceConfig.Properties,
            DisplayNames = referenceConfig.DisplayNames,
            Requirements = referenceConfig.Requirements,
            Type = referenceConfig.Type,
            ClientUri = "https://localhost:44335",
            LogoUri = referenceConfig.LogoUri
        };

        await _openIdDictApplicationRepository.InsertAsync(newConfig);
    }
}