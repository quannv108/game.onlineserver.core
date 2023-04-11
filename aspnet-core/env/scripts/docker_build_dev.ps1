$ErrorActionPreference = "Stop"

$scriptpath = $MyInvocation.MyCommand.Path
$dir = Split-Path $scriptpath
Write-host "Current Directory at $dir"

Write-host "Generate env appsettings.json"
python "$dir\..\update.py" dev

Write-host "Generate Docker Images"
docker build -f Migrator.Dockerfile -t public.ecr.aws/a9z0m0c0/gameonline-migrator "$dir\..\.."
docker build -f SignalR.Dockerfile -t public.ecr.aws/a9z0m0c0/gameonline-signalr "$dir\..\.."
docker build -f Api.Dockerfile -t public.ecr.aws/a9z0m0c0/gameonline-api "$dir\..\.."

Write-host "Done"