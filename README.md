# Meeting Room Booking API

A RESTful API for managing meeting rooms and time-slot bookings, built with ASP.NET Core 8.0 using Clean Architecture.

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

That's it — the project uses an **InMemory database**, so no database server is needed.

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

All endpoints require a **JWT Bearer token**. In Swagger UI, click the **Authorize 🔒** button and enter your token.

The JWT settings are configured in `appsettings.json` under the `Jwt` section:

```json
{
  "Jwt": {
    "Issuer": "https://localhost:5001",
    "Audience": "https://localhost:5001",
    "Key": "YourSecretKeyHere_AtLeast32Characters!"
  }
}
```

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

Dependencies only point **inwards** — Domain has zero external references.

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
