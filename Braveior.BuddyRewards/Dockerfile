#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Braveior.BuddyRewards.API/Braveior.BuddyRewards.API.csproj", "Braveior.BuddyRewards.API/"]
COPY ["Braveior.BuddyRewards.Service/Braveior.BuddyRewards.Service.csproj", "Braveior.BuddyRewards.Service/"]
COPY ["Braveior.BuddyRewards.DTO/Braveior.BuddyRewards.DTO.csproj", "Braveior.BuddyRewards.DTO/"]
RUN dotnet restore "Braveior.BuddyRewards.API/Braveior.BuddyRewards.API.csproj"
COPY . .
WORKDIR "/src/Braveior.BuddyRewards.API"
RUN dotnet build "Braveior.BuddyRewards.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Braveior.BuddyRewards.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Braveior.BuddyRewards.API.dll"]