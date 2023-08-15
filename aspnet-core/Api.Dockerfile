ARG OS_VERSION=alpine3.17-amd64
ARG DOTNET_VERSION=7.0
ARG ROOT_NAMESPACE="Qna.Game.OnlineServer"

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION-$OS_VERSION AS base
ARG ROOT_NAMESPACE

WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

RUN apk add --no-cache icu-libs

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION-$OS_VERSION AS build
ARG ROOT_NAMESPACE

RUN dotnet tool install -g Volo.Abp.Cli
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["src", "/src/src/"]
COPY ["common.props", "/src/"]
WORKDIR "/src/src/${ROOT_NAMESPACE}.HttpApi.Host"
RUN abp install-libs


FROM build AS publish

RUN dotnet restore
RUN dotnet publish "${ROOT_NAMESPACE}.HttpApi.Host.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore -p:RunAnalyzers=false

FROM base AS final

ENV APP_NAME=${ROOT_NAMESPACE}.HttpApi.Host.dll
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_HTTPS_PORT=44325
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=12345678
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/${ROOT_NAMESPACE}.HttpApi.Host.pfx

COPY ["env/certs/default/${ROOT_NAMESPACE}.HttpApi.Host.pfx", "/https/${ROOT_NAMESPACE}.HttpApi.Host.pfx"]

WORKDIR /app
COPY --from=publish /app/publish .
COPY ["env/out/Api/appsettings.json", "appsettings.json"]
ENTRYPOINT dotnet $APP_NAME
