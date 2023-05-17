# Use Red Hat UBI 8 for runtime
FROM registry.access.redhat.com/ubi8/dotnet-60-runtime:6.0-19.20221020081244 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#NOTE - Having build output folder permission issues with UBI, so build on ms
WORKDIR /src
COPY ["WeatherApp/WeatherApp.csproj", "WeatherApp/"]
RUN dotnet restore "WeatherApp/WeatherApp.csproj"
COPY . .
WORKDIR "/src/WeatherApp"
RUN dotnet build "WeatherApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherApp.dll"]