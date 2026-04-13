# Multi-stage Dockerfile for Meeting Room Booking API
# ------------------------------------------------------------------------------
# Stage 1: Base - Defines the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

# Security: Run as non-root user
RUN addgroup -g 1000 -S appgroup && \
    adduser -u 1000 -S appuser -G appgroup
USER appuser

# Stage 2: Build - Restores and builds the application
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy solution and project files for efficient restore caching
COPY ["Meeting-Room-Booking-API.sln", "./"]
COPY ["**/Meeting-Room-Booking-API*.csproj", "./"]
# Note: Above glob works if we flatten or handle subdirs. Let's stick to explicit to be safe in Alpine.
COPY ["Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj", "Meeting-Room-Booking-API/"]
COPY ["Meeting-Room-Booking-API.Application/Meeting-Room-Booking-API.Application.csproj", "Meeting-Room-Booking-API.Application/"]
COPY ["Meeting-Room-Booking-API.Domain/Meeting-Room-Booking-API.Domain.csproj", "Meeting-Room-Booking-API.Domain/"]
COPY ["Meeting-Room-Booking-API.Infrastructure/Meeting-Room-Booking-API.Infrastructure.csproj", "Meeting-Room-Booking-API.Infrastructure/"]
COPY ["Meeting-Room-Booking-API.Tests/Meeting-Room-Booking-API.Tests.csproj", "Meeting-Room-Booking-API.Tests/"]

RUN dotnet restore "Meeting-Room-Booking-API.sln"

# Copy full source and build
COPY . .
RUN dotnet build "Meeting-Room-Booking-API.sln" -c $BUILD_CONFIGURATION

# Stage 3: Test - Runs unit tests during the build process
FROM build AS test
LABEL test=true
RUN dotnet test "Meeting-Room-Booking-API.Tests/Meeting-Room-Booking-API.Tests.csproj" --no-build --verbosity normal

# Stage 4: Publish - Primes the binaries for production
FROM build AS publish
RUN dotnet publish "Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Stage 5: Final - Production-ready minimal image
FROM base AS final
WORKDIR /app
COPY --from=publish --chown=appuser:appgroup /app/publish .

# Environment configuration
ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

# Container Metadata
LABEL org.opencontainers.image.title="Meeting Room Booking API" \
      org.opencontainers.image.description="RESTful API for managing meeting rooms and bookings using Clean Architecture." \
      org.opencontainers.image.authors="Hlib Rachkovskyy" \
      org.opencontainers.image.version="1.0.0"

# Healthcheck configuration (Depends on /health endpoint in Program.cs)
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget --no-verbose --tries=1 --spider http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Meeting-Room-Booking-API.dll"]
