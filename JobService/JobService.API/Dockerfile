FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JobService.Domain/JobService.Domain.csproj", "JobService.Domain/"]
COPY ["JobService.Application/JobService.Application.csproj", "JobService.Application/"]
COPY ["JobService.Infrastructure/JobService.Infrastructure.csproj", "JobService.Infrastructure/"]
COPY ["JobService.API/JobService.API.csproj", "JobService.API/"]
RUN dotnet restore "JobService.API/JobService.API.csproj"
COPY . .
WORKDIR "/src/JobService.API"
RUN dotnet build "JobService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JobService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobService.API.dll"]

