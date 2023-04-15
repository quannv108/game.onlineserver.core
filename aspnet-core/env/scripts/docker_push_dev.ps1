Write-host "Login to AWS CLI"
aws ecr-public get-login-password --region us-east-1 | docker login --username AWS --password-stdin public.ecr.aws/a9z0m0c0

Write-host "Push Docker Images to Registry"
docker push public.ecr.aws/a9z0m0c0/gameonline-migrator:latest
docker push public.ecr.aws/a9z0m0c0/gameonline-api:latest
docker push public.ecr.aws/a9z0m0c0/gameonline-signalr:latest

Write-host "Done"