# C4 Code — Domain/Interfaces & DTOs

## Overview

| Field | Value |
|-------|-------|
| **Name** | Domain Contracts (Interfaces & DTOs) |
| **Location** | [Meeting-Room-Booking-API.Domain/Interfaces/](../Meeting-Room-Booking-API.Domain/Interfaces/) · [Meeting-Room-Booking-API.Domain/DTOs/](../Meeting-Room-Booking-API.Domain/DTOs/) |
| **Language** | C# 12 / .NET 8.0 |
| **Purpose** | Define all repository and service contracts consumed by Application and Infrastructure. Define data transfer objects shared across layers. |

---

## Code Elements

### Interfaces

#### `IAuthService`
**File:** [IAuthService.cs](../Meeting-Room-Booking-API.Domain/Interfaces/IAuthService.cs)

| Method | Signature | Description |
|--------|-----------|-------------|
| `RegisterAsync` | `Task<(AuthResponse Auth, string RefreshToken)> RegisterAsync(RegisterRequest request)` | Registers a new user and returns an access token + refresh token. Throws `InvalidOperationException` if email is taken. |
| `LoginAsync` | `Task<(AuthResponse Auth, string RefreshToken)> LoginAsync(LoginRequest request)` | Validates credentials and returns both tokens. Throws `UnauthorizedAccessException` on failure. |
| `RefreshAsync` | `Task<(RefreshResponse Refresh, string NewRefreshToken)> RefreshAsync(string refreshToken)` | Validates a refresh token JWT and issues a rotated token pair. |

#### `IBookingService`
**File:** [IBookingService.cs](../Meeting-Room-Booking-API.Domain/Interfaces/IBookingService.cs)

| Method | Signature | Description |
|--------|-----------|-------------|
| `BookRoomAsync` | `Task<Booking> BookRoomAsync(Guid roomId, string bookedBy, DateTime startTime, DateTime endTime)` | Creates a booking; enforces conflict detection. |
| `GetBookingsByRoomAsync` | `Task<IEnumerable<Booking>> GetBookingsByRoomAsync(Guid roomId)` | Returns all bookings for a given room. |
| `CancelBookingAsync` | `Task<bool> CancelBookingAsync(Guid bookingId)` | Cancels a booking by ID; returns false if not found. |

#### `IRoomRepository`
**File:** [IRoomRepository.cs](../Meeting-Room-Booking-API.Domain/Interfaces/IRoomRepository.cs)

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetAllAsync` | `Task<IEnumerable<Room>> GetAllAsync()` | Returns all rooms. |
| `GetByIdAsync` | `Task<Room?> GetByIdAsync(Guid id, bool includeBookings = false)` | Returns a room by ID, optionally eager-loading bookings. |
| `AddAsync` | `Task AddAsync(Room room)` | Persists a new room. |
| `UpdateAsync` | `Task UpdateAsync(Room room)` | Saves changes to an existing room. |
| `DeleteAsync` | `Task DeleteAsync(Guid id)` | Removes a room by ID. |

#### `IBookingRepository`
**File:** [IBookingRepository.cs](../Meeting-Room-Booking-API.Domain/Interfaces/IBookingRepository.cs)

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetByRoomIdAsync` | `Task<IEnumerable<Booking>> GetByRoomIdAsync(Guid roomId)` | Returns all bookings for a room. |
| `GetByIdAsync` | `Task<Booking?> GetByIdAsync(Guid id)` | Returns a single booking by ID. |
| `DeleteAsync` | `Task DeleteAsync(Guid id)` | Deletes a booking by ID. |

#### `IUserRepository`
**File:** [IUserRepository.cs](../Meeting-Room-Booking-API.Domain/Interfaces/IUserRepository.cs)

| Method | Signature | Description |
|--------|-----------|-------------|
| `GetByEmailAsync` | `Task<User?> GetByEmailAsync(string email)` | Finds a user by normalised email. |
| `GetByIdAsync` | `Task<User?> GetByIdAsync(Guid id)` | Finds a user by primary key. |
| `ExistsByEmailAsync` | `Task<bool> ExistsByEmailAsync(string email)` | Returns true if the email is already registered. |
| `AddAsync` | `Task AddAsync(User user)` | Persists a new user. |

---

### DTOs

#### `AuthDTOs` — [AuthDTOs.cs](../Meeting-Room-Booking-API.Domain/DTOs/AuthDTOs.cs)

| Record | Fields | Description |
|--------|--------|-------------|
| `RegisterRequest` | `string FullName, string Email, string Password` | Payload for POST /api/auth/register. |
| `LoginRequest` | `string Email, string Password` | Payload for POST /api/auth/login. |
| `AuthResponse` | `string AccessToken, string Email, string FullName, DateTime ExpiresAt` | Returned on login/register; access token valid 15 min. |
| `RefreshResponse` | `string AccessToken, DateTime ExpiresAt` | Returned on token refresh. |

---

## Dependencies

### Internal
- `Domain.Entities` (Room, Booking, User) — used as return/parameter types.

### External
- None.
