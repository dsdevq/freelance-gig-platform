FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files for restore
COPY ["JobService/JobService.Domain/JobService.Domain.csproj", "JobService/JobService.Domain/"]
COPY ["JobService/JobService.Application/JobService.Application.csproj", "JobService/JobService.Application/"]
COPY ["JobService/JobService.Infrastructure/JobService.Infrastructure.csproj", "JobService/JobService.Infrastructure/"]
COPY ["JobService/JobService.API/JobService.API.csproj", "JobService/JobService.API/"]
COPY ["Shared/Shared.Application/Shared.Application.csproj", "Shared/Shared.Application/"]
COPY ["Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "Shared/Shared.Infrastructure/"]
COPY ["Shared/Shared.WebApi/Shared.WebApi.csproj", "Shared/Shared.WebApi/"]

# Restore dependencies
WORKDIR /src/JobService/JobService.API
RUN dotnet restore "JobService.API.csproj"

# Copy all source code
WORKDIR /src
COPY ["JobService/", "JobService/"]
COPY ["Shared/", "Shared/"]

# Build
WORKDIR /src/JobService/JobService.API
RUN dotnet build "JobService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
WORKDIR /src/JobService/JobService.API
RUN dotnet publish "JobService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobService.API.dll"]
