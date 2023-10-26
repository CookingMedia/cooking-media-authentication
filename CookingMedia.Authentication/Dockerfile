FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CookingMedia.Authentication/CookingMedia.Authentication.csproj", "CookingMedia.Authentication/"]
RUN dotnet restore "CookingMedia.Authentication/CookingMedia.Authentication.csproj"
COPY . .
WORKDIR "/src/CookingMedia.Authentication"
RUN dotnet build "CookingMedia.Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CookingMedia.Authentication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CookingMedia.Authentication.dll"]
