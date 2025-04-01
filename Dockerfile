FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OnlineElectronicsStore/OnlineElectronicsStore.csproj", "OnlineElectronicsStore/"]
RUN dotnet restore "OnlineElectronicsStore/OnlineElectronicsStore.csproj"
COPY . .
WORKDIR "/src/OnlineElectronicsStore"
RUN dotnet build "OnlineElectronicsStore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OnlineElectronicsStore.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OnlineElectronicsStore.dll"]
