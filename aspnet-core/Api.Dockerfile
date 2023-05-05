ARG OS_VERSION=alpine3.17-amd64
ARG DOTNET_VERSION=7.0

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION-$OS_VERSION AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

RUN apk add --no-cache icu-libs

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION-$OS_VERSION AS build
RUN dotnet tool install -g Volo.Abp.Cli
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["src", "/src/src/"]
COPY ["common.props", "/src/"]
WORKDIR "/src/src/Qna.Game.OnlineServer.HttpApi.Host"
RUN abp install-libs


FROM build AS publish
RUN dotnet restore
RUN dotnet publish "Qna.Game.OnlineServer.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore -p:RunAnalyzers=false

FROM base AS final

ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_HTTPS_PORT=44325
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=12345678
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/Qna.Game.OnlineServer.HttpApi.Host.pfx

COPY ["env/certs/default/Qna.Game.OnlineServer.HttpApi.Host.pfx", "/https/Qna.Game.OnlineServer.HttpApi.Host.pfx"]

WORKDIR /app
COPY --from=publish /app/publish .
COPY ["env/out/Api/appsettings.json", "appsettings.json"]
ENTRYPOINT ["dotnet", "Qna.Game.OnlineServer.HttpApi.Host.dll"]
