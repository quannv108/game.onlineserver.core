# Online Game Server

## Requirement
* docker
* python

## Setup environment
* Go to `env` folder, update the config there.
* You can create new env by copy a `*.env` file and rename to `<env_name>.env`
* Run script `python update.py <env_name>`
* You will got new config in `out` folder
* Using these `appsettings.json` to mount to docker container

## Build docker images
Set current working directory to `asp-netcore` folder.

Then run this command to build docker images
> docker build --target publish -f Api.Dockerfile -t gameonline-server-api-publish .
>
> docker build --target publish -f SignalR.Dockerfile -t gameonline-server-signalr-publish .
>
> docker build --target publish -f Migrator.Dockerfile -t gameonline-server-migrator-publish .

Then you will have 3 docker images.

## Deployment
* Copy all `appsettings.json` file which is generated from above step to server
* 
To run docker, use these command

> docker run --rm --name gameonline-server-migrator gameonline-server-migrator-final:latest
> 
> docker run -p 44325:80 --name gameonline-server-api -d gameonline-server-api-final:latest
> 
> docker run -p 44335:80 --name gameonline-server-signalr -d gameonline-server-signalr-final:latest

`Migrator` should be run first when start new deployment, and it will run once then stop.

`SignalR` and `Api` should be run as daemon