# Online Game Server

A Simple Turn-Base game server using .NET Core 7, Abp.Io, SignalR, PostgreSql.
Ability to scale out when spitted into 2 server: Game Server (SignalR) and API Services (Http API)

![!docs/images/system_design.png](docs/images/system_design.png)

## Requirement
* docker
* python
* postgresql

## Setup database
Run `postgresql` via docker
> docker run --env=POSTGRES_PASSWORD=postgrespw --env=LANG=en_US.utf8 --env=PGDATA=/var/lib/postgresql/data --volume=/var/lib/postgresql/data:/var/lib/postgresql/data -p 5432:5432 --name postgres -d postgres:latest

Go inside docker container by command
> docker exec -it postgres bash

Inside the docker container, open `psql` using default user
> psql -U postgres

create new user,set password and create and database
> CREATE USER onlinegameserver PASSWORD '12345678';
>
> CREATE DATABASE onlinegameserver;
> 
> ALTER DATABASE onlinegameserver OWNER TO OnlineGameServer;
> 
> ALTER USER onlinegameserver CREATEDB LOGIN;

## Setup environment
* Go to `env` folder, update the config there.
* You can create new env by copy a `*.env` file and rename to `<env_name>.env`
* Run script `python update.py <env_name>`
* You will got new config in `out` folder
* Using these `appsettings.json` to mount to docker container

## Build docker images
Set current working directory to `asp-netcore` folder.

Then run this command to build docker images
```shell
docker build -f Migrator.Dockerfile -t gameonline-server-migrator-publish .
```
```shell
docker build -f Api.Dockerfile -t gameonline-server-api-publish .
```
```shell
docker build -f SignalR.Dockerfile -t gameonline-server-signalr-publish .
```

Then you will have 3 docker images.

## Deployment
* Copy all `appsettings.json` file which is generated from above step to server

To run docker, use these command

```shell
docker run --rm --name gameonline-server-migrator gameonline-server-migrator-publish:latest
```
```shell
docker run -p 44325:443 --name gameonline-server-api -d gameonline-server-api-publish:latest
```
```shell
docker run -p 44335:443 --name gameonline-server-signalr -d gameonline-server-signalr-publish:latest
```

`Migrator` should be run first when start new deployment, and it will run once then stop.

`SignalR` and `Api` should be run as daemon

## Deploy in AWS EC2 (Example with dev env)
Need to login into `Elastic Registry Container`.
```shell
aws ecr-public get-login-password --region us-east-1 | docker login --username AWS --password-stdin public.ecr.aws/a9z0m0c0
```

* Tag & push current docker images to `Elastic Registry Container`
```shell
docker tag gameonline-server-migrator-publish public.ecr.aws/a9z0m0c0/gameonline-migrator:latest
```
```shell
docker push public.ecr.aws/a9z0m0c0/gameonline-migrator:latest
```
```shell
docker tag gameonline-server-api-publish public.ecr.aws/a9z0m0c0/gameonline-api:latest
```
```shell
docker push public.ecr.aws/a9z0m0c0/gameonline-api:latest
```
```shell
docker tag gameonline-server-signalr-publish public.ecr.aws/a9z0m0c0/gameonline-signalr:latest
```
```shell
docker push public.ecr.aws/a9z0m0c0/gameonline-signalr:latest
```
* Connect to EC2 via ssh connection

* then start docker in order

```shell 
docker start postgres 
```
```shell
docker pull public.ecr.aws/a9z0m0c0/gameonline-migrator:latest
```
```shell
docker run --rm --name migrator public.ecr.aws/a9z0m0c0/gameonline-migrator:latest
```
```shell
docker pull public.ecr.aws/a9z0m0c0/gameonline-api:latest
```
```shell
docker run -p 44325:443 --name api -d public.ecr.aws/a9z0m0c0/gameonline-api:latest
```
```shell
docker pull public.ecr.aws/a9z0m0c0/gameonline-signalr:latest
```
```shell
docker run -p 44335:443 --name signalr -d public.ecr.aws/a9z0m0c0/gameonline-signalr:latest
```

