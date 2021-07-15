#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Aircraft.Tasks.Worker/Aircraft.Tasks.Worker.csproj", "Aircraft.Tasks.Worker/"]
COPY ["Aircraft.Tasks.Internal/Aircraft.Tasks.Internal.csproj", "Aircraft.Tasks.Internal/"]
COPY ["Aircraft.Tasks.Core/Aircraft.Tasks.Core.csproj", "Aircraft.Tasks.Core/"]
RUN dotnet restore "Aircraft.Tasks.Worker/Aircraft.Tasks.Worker.csproj"
COPY . .
WORKDIR "/src/Aircraft.Tasks.Worker"
RUN dotnet build "Aircraft.Tasks.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aircraft.Tasks.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aircraft.Tasks.Worker.dll"]
