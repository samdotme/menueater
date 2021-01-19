FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

COPY ./EaterCore/. /app/EaterCore
COPY ./menueater.cli/. /app/menueater.cli
WORKDIR /app

RUN dotnet publish ./menueater.cli/menueater.cli.csproj -c Release -o out
RUN dotnet restore ./menueater.cli/menueater.cli.csproj

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "menueater.cli.dll"]
