#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5390

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CookingMedia.Authentication/CookingMedia.Authentication.csproj", "CookingMedia.Authentication/"]
RUN dotnet restore "./CookingMedia.Authentication/./CookingMedia.Authentication.csproj"
COPY . .
WORKDIR "/src/CookingMedia.Authentication"
RUN dotnet build "./CookingMedia.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CookingMedia.Authentication.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookingMedia.Authentication.dll"]