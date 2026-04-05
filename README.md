# Meeting Room Booking API

A RESTful API for managing meeting rooms and time-slot bookings, built with ASP.NET Core 8.0 using Clean Architecture.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

That's it- the project uses an **InMemory database**, so no database server is needed.

## Getting Started

### 1. Clone & Restore

```bash
git clone <your-repo-url>
cd Meeting-Room-Booking-API
dotnet restore
```

### 2. Run the API

```bash
dotnet run --project Meeting-Room-Booking-API
```

The API will start and you should see output like:

```
info: Now listening on: https://localhost:5001
info: Now listening on: http://localhost:5000
```

> **Note:** On first startup, the database is automatically seeded with **10 meeting rooms** and **50 realistic bookings**.

### 3. Open Swagger UI

Navigate to:

```
https://localhost:5001/swagger
```

You'll see full interactive API documentation with all endpoints.

### 4. Run Tests

```bash
dotnet test
```

## API Endpoints

### Authentication

| Method | Endpoint | Auth required | Description |
|--------|----------|---------------|-------------|
| `POST` | `/api/auth/register` | ❌ | Register a new user. Returns access token + sets refresh cookie. |
| `POST` | `/api/auth/login` | ❌ | Login. Returns access token + sets refresh cookie. |
| `POST` | `/api/auth/refresh` | ❌ (uses cookie) | Rotate refresh token. Returns new access token + new refresh cookie. |
| `POST` | `/api/auth/logout` | ✅ Bearer | Clears the refresh token cookie. |

### Rooms

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/rooms` | Get all meeting rooms |
| `GET` | `/api/rooms/{id}` | Get a room by ID (includes bookings) |
| `POST` | `/api/rooms` | Create a new room |
| `DELETE` | `/api/rooms/{id}` | Delete a room |

### Bookings

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/rooms/{roomId}/bookings` | Get all bookings for a room |
| `POST` | `/api/rooms/{roomId}/bookings` | Create a booking (rejects overlaps) |
| `DELETE` | `/api/rooms/{roomId}/bookings/{bookingId}` | Cancel a booking |

## Authentication

The API uses **JWT Bearer token** authentication with **refresh token rotation**.

### Token Lifecycle

| Token | Lifetime | Transport |
|-------|----------|-----------|
| Access Token | **15 minutes** | JSON response body — send as `Authorization: Bearer <token>` |
| Refresh Token | **7 days** | `HttpOnly`, `Secure` cookie — browser sends it automatically |

### Flow

1. **Login / Register** → receive an `accessToken` in the body + a `refreshToken` cookie (set automatically).
2. Include the access token in all protected requests: `Authorization: Bearer <accessToken>`.
3. When the access token expires (15 min), call `POST /api/auth/refresh` — no body needed, the browser sends the cookie automatically. You'll receive a new access token and a rotated refresh token cookie.
4. Call `POST /api/auth/logout` (with a valid access token) to clear the refresh token cookie.

### Configuration

JWT settings live in `appsettings.Development.json` for local development:

```json
{
  "Jwt": {
    "Key":        "<access-token-signing-key>",
    "RefreshKey": "<refresh-token-signing-key — must be different from Key>",
    "Issuer":     "https://localhost:5001",
    "Audience":   "https://localhost:5001"
  }
}
```

> **Important:** In production, set these values via environment variables or a secrets manager — never commit real keys to source control.

### Testing the Refresh Endpoint

> **⚠️ Swagger UI limitation:** The `/api/auth/refresh` endpoint relies on an `HttpOnly` cookie, which Swagger UI cannot send (JavaScript cannot access `HttpOnly` cookies). Use **Postman** instead:
>
> 1. Enable **Cookies** in Postman (enabled by default).
> 2. Call `POST /api/auth/login` — Postman automatically stores the `refreshToken` cookie.
> 3. Call `POST /api/auth/refresh` — Postman automatically includes the cookie.
> 4. You'll receive a new `accessToken` and the cookie will be rotated.

In Swagger UI, click the **Authorize** button and enter your access token to test all other protected endpoints.

## Project Structure

```
Meeting-Room-Booking-API/
├── Meeting-Room-Booking-API.Domain/        # Entities, Interfaces (zero dependencies)
├── Meeting-Room-Booking-API.Application/   # Use cases & business logic contracts
├── Meeting-Room-Booking-API.Infrastructure/ # EF Core, Repositories, Data Seeding
├── Meeting-Room-Booking-API/               # WebAPI (Controllers, Middleware, Program.cs)
└── Meeting-Room-Booking-API.Tests/         # xUnit + Moq unit tests
```

### Dependency Rule

```
WebAPI → Application → Domain
WebAPI → Infrastructure → Application → Domain
```

Dependencies only point **inwards**- Domain has zero external references.

## Example Request Bodies

### Create a Room

```json
{
  "name": "Conference Room A",
  "location": "Floor 2 - East Wing",
  "capacity": 12
}
```

### Create a Booking

```json
{
  "bookedBy": "John Doe",
  "startTime": "2026-03-27T10:00:00",
  "endTime": "2026-03-27T11:30:00"
}
```
