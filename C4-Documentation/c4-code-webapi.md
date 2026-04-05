# C4 Code — Web API Layer

## Overview

| Field | Value |
|-------|-------|
| **Name** | Web API (Controllers, Middleware, Program) |
| **Location** | [Meeting-Room-Booking-API/](../Meeting-Room-Booking-API/) |
| **Language** | C# 12 / ASP.NET Core 8.0 |
| **Purpose** | HTTP entry point. Handles request routing, authentication middleware, JWT Bearer configuration, exception handling, Swagger docs, and DI composition root. |

---

## Code Elements

### `AuthController`
**File:** [Controllers/AuthController.cs](../Meeting-Room-Booking-API/Controllers/AuthController.cs)  
**Route:** `api/auth`  
**Auth:** anonymous (all endpoints except `logout`)

| Endpoint | Method | Route | Description |
|----------|--------|-------|-------------|
| `Register` | POST | `/api/auth/register` | Calls `IAuthService.RegisterAsync`, sets `refreshToken` HttpOnly cookie, returns `AuthResponse` (201). |
| `Login` | POST | `/api/auth/login` | Calls `IAuthService.LoginAsync`, sets `refreshToken` HttpOnly cookie, returns `AuthResponse` (200). |
| `Refresh` | POST | `/api/auth/refresh` | Reads `refreshToken` cookie, calls `IAuthService.RefreshAsync`, rotates cookie, returns `RefreshResponse` (200). |
| `Logout` | POST | `/api/auth/logout` | Requires `[Authorize]`. Clears `refreshToken` cookie. Returns 204. |

Cookie options applied on all Set-Cookie calls: `HttpOnly=true`, `Secure=true`, `SameSite=Strict`, `Expires=+7days`.

---

### `RoomsController`
**File:** [Controllers/RoomsController.cs](../Meeting-Room-Booking-API/Controllers/RoomsController.cs)  
**Route:** `api/rooms`  
**Auth:** `[Authorize]` (all endpoints)

| Endpoint | Method | Route | Description |
|----------|--------|-------|-------------|
| `GetAll` | GET | `/api/rooms` | Returns all rooms (200). |
| `GetById` | GET | `/api/rooms/{id:guid}` | Returns a single room with bookings (200/404). |
| `Create` | POST | `/api/rooms` | Creates room from `CreateRoomRequest`; returns 201 with Location header. |
| `Delete` | DELETE | `/api/rooms/{id:guid}` | Deletes room (204/404). |

---

### `BookingsController`
**File:** [Controllers/BookingsController.cs](../Meeting-Room-Booking-API/Controllers/BookingsController.cs)  
**Route:** `api/rooms/{roomId:guid}/bookings`  
**Auth:** `[Authorize]` (all endpoints)

| Endpoint | Method | Route | Description |
|----------|--------|-------|-------------|
| `GetByRoom` | GET | `…/bookings` | Returns all bookings for a room (200). |
| `Create` | POST | `…/bookings` | Creates booking from `CreateBookingRequest`; enforces overlap detection via `BookingService` (201/400/404). |
| `Cancel` | DELETE | `…/bookings/{bookingId:guid}` | Cancels a booking (204/404). |

---

### `ExceptionHandlingMiddleware`
**File:** [Middleware/ExceptionHandlingMiddleware.cs](../Meeting-Room-Booking-API/Middleware/ExceptionHandlingMiddleware.cs)

| Exception Type | HTTP Status | Notes |
|---------------|-------------|-------|
| `ArgumentException` | 400 Bad Request | |
| `InvalidOperationException` | 400 Bad Request | Includes booking conflicts |
| `KeyNotFoundException` | 404 Not Found | |
| `UnauthorizedAccessException` | 401 Unauthorized | |
| All others | 500 Internal Server Error | |

Returns RFC 7807 `ProblemDetails` JSON for all errors.

---

### Web DTOs
**File:** [DTOs/Requests.cs](../Meeting-Room-Booking-API/DTOs/Requests.cs)

| Record | Fields | Used By |
|--------|--------|---------|
| `CreateRoomRequest` | `string Name, string Location, int Capacity` | `RoomsController.Create` |
| `CreateBookingRequest` | `string BookedBy, DateTime StartTime, DateTime EndTime` | `BookingsController.Create` |

---

### `Program.cs`
**File:** [Program.cs](../Meeting-Room-Booking-API/Program.cs)

Middleware pipeline order:
1. `ExceptionHandlingMiddleware` (custom)
2. `UseHttpsRedirection`
3. `UseAuthentication` (JWT Bearer)
4. `UseAuthorization`
5. `MapControllers`

Registers: Swagger with JWT security scheme, JWT Bearer authentication (`Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience`), `AddApplication()`, `AddInfrastructure()`.

---

## Dependencies

### Internal
- `Domain.DTOs` — `AuthResponse`, `RefreshResponse`, `RegisterRequest`, `LoginRequest`
- `Domain.Interfaces` — `IAuthService`, `IBookingService`, `IRoomRepository`
- `Application` (via DI)
- `Infrastructure` (via DI)

### External

| Package | Purpose |
|---------|---------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | JWT Bearer authentication middleware |
| `Microsoft.IdentityModel.Tokens` | Token validation parameters |
| `Swashbuckle.AspNetCore` | Swagger/OpenAPI UI |
