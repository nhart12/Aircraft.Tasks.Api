#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Aircraft.Tasks.Api/Aircraft.Tasks.Api.csproj", "Aircraft.Tasks.Api/"]
COPY ["Aircraft.Tasks.Internal/Aircraft.Tasks.Internal.csproj", "Aircraft.Tasks.Internal/"]
COPY ["Aircraft.Tasks.Core/Aircraft.Tasks.Core.csproj", "Aircraft.Tasks.Core/"]
RUN dotnet restore "Aircraft.Tasks.Api/Aircraft.Tasks.Api.csproj"
COPY . .
WORKDIR "/src/Aircraft.Tasks.Api"
RUN dotnet build "Aircraft.Tasks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Aircraft.Tasks.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Aircraft.Tasks.Api.dll"]
