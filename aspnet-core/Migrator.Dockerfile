ARG OS_VERSION=alpine3.17-amd64
ARG DOTNET_VERSION=7.0
ARG ROOT_NAMESPACE="Qna.Game.OnlineServer"

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION-$OS_VERSION AS base
ARG ROOT_NAMESPACE

WORKDIR /app
EXPOSE 80

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS=http://+:80

RUN apk add --no-cache icu-libs

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION-$OS_VERSION AS build
ARG ROOT_NAMESPACE

RUN dotnet tool install -g Volo.Abp.Cli
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /
ADD ["src/${ROOT_NAMESPACE}.DbMigrator", "/src/src/${ROOT_NAMESPACE}.DbMigrator"]
COPY ["src/${ROOT_NAMESPACE}.Domain", "/src/src/${ROOT_NAMESPACE}.Domain"]
COPY ["src/${ROOT_NAMESPACE}.Domain.Shared", "/src/src/${ROOT_NAMESPACE}.Domain.Shared"]
COPY ["src/${ROOT_NAMESPACE}.EntityFrameworkCore", "/src/src/${ROOT_NAMESPACE}.EntityFrameworkCore"]
COPY ["common.props", "/src/"]

WORKDIR "/src/src/${ROOT_NAMESPACE}.DbMigrator"
RUN abp install-libs


FROM build AS publish

RUN dotnet restore
RUN dotnet publish "${ROOT_NAMESPACE}.DbMigrator.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore -p:RunAnalyzers=false

FROM base AS final
ENV APP_NAME=${ROOT_NAMESPACE}.DbMigrator.dll
RUN echo $APP_NAME

WORKDIR /app
COPY --from=publish /app/publish .
COPY ["env/out/Migrator/appsettings.json", "appsettings.json"]
ENTRYPOINT dotnet $APP_NAME
