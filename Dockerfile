# Use the official multi-stage build pattern
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
# Security hardening: Run as non-root user
RUN addgroup -g 1000 -S appgroup && \
    adduser -u 1000 -S appuser -G appgroup
WORKDIR /app
USER 1000
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Ensure efficient layer caching by restoring dependencies first
COPY ["Meeting-Room-Booking-API.sln", "./"]
COPY ["Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj", "Meeting-Room-Booking-API/"]
COPY ["Meeting-Room-Booking-API.Application/Meeting-Room-Booking-API.Application.csproj", "Meeting-Room-Booking-API.Application/"]
COPY ["Meeting-Room-Booking-API.Domain/Meeting-Room-Booking-API.Domain.csproj", "Meeting-Room-Booking-API.Domain/"]
COPY ["Meeting-Room-Booking-API.Infrastructure/Meeting-Room-Booking-API.Infrastructure.csproj", "Meeting-Room-Booking-API.Infrastructure/"]
COPY ["Meeting-Room-Booking-API.Tests/Meeting-Room-Booking-API.Tests.csproj", "Meeting-Room-Booking-API.Tests/"]

RUN dotnet restore "Meeting-Room-Booking-API.sln"

# Copy remaining source code and build
COPY . .
WORKDIR "/src/Meeting-Room-Booking-API"
RUN dotnet build "Meeting-Room-Booking-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "Meeting-Room-Booking-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
# Copy binaries and set proper ownership
COPY --from=publish --chown=appuser:appgroup /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "Meeting-Room-Booking-API.dll"]
