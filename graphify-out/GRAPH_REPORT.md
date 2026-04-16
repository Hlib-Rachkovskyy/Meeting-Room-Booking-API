# Graph Report - .  (2026-04-13)

## Corpus Check
- Corpus is ~10,829 words - fits in a single context window. You may not need a graph.

## Summary
- 260 nodes · 252 edges · 49 communities detected
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]
- [[_COMMUNITY_Community 6|Community 6]]
- [[_COMMUNITY_Community 7|Community 7]]
- [[_COMMUNITY_Community 8|Community 8]]
- [[_COMMUNITY_Community 9|Community 9]]
- [[_COMMUNITY_Community 10|Community 10]]
- [[_COMMUNITY_Community 11|Community 11]]
- [[_COMMUNITY_Community 12|Community 12]]
- [[_COMMUNITY_Community 13|Community 13]]
- [[_COMMUNITY_Community 14|Community 14]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]
- [[_COMMUNITY_Community 20|Community 20]]
- [[_COMMUNITY_Community 21|Community 21]]
- [[_COMMUNITY_Community 22|Community 22]]
- [[_COMMUNITY_Community 23|Community 23]]
- [[_COMMUNITY_Community 24|Community 24]]
- [[_COMMUNITY_Community 25|Community 25]]
- [[_COMMUNITY_Community 26|Community 26]]
- [[_COMMUNITY_Community 27|Community 27]]
- [[_COMMUNITY_Community 28|Community 28]]
- [[_COMMUNITY_Community 29|Community 29]]
- [[_COMMUNITY_Community 30|Community 30]]
- [[_COMMUNITY_Community 31|Community 31]]
- [[_COMMUNITY_Community 32|Community 32]]
- [[_COMMUNITY_Community 33|Community 33]]
- [[_COMMUNITY_Community 34|Community 34]]
- [[_COMMUNITY_Community 35|Community 35]]
- [[_COMMUNITY_Community 36|Community 36]]
- [[_COMMUNITY_Community 37|Community 37]]
- [[_COMMUNITY_Community 38|Community 38]]
- [[_COMMUNITY_Community 39|Community 39]]
- [[_COMMUNITY_Community 40|Community 40]]
- [[_COMMUNITY_Community 41|Community 41]]
- [[_COMMUNITY_Community 42|Community 42]]
- [[_COMMUNITY_Community 43|Community 43]]
- [[_COMMUNITY_Community 44|Community 44]]
- [[_COMMUNITY_Community 45|Community 45]]
- [[_COMMUNITY_Community 46|Community 46]]
- [[_COMMUNITY_Community 47|Community 47]]
- [[_COMMUNITY_Community 48|Community 48]]

## God Nodes (most connected - your core abstractions)
1. `BookingOverlapTests` - 20 edges
2. `DtoValidationTests` - 15 edges
3. `AuthService` - 11 edges
4. `BookingRepository` - 8 edges
5. `AuthController` - 7 edges
6. `IBookingRepository` - 7 edges
7. `RoomRepository` - 7 edges
8. `UserRepository` - 7 edges
9. `RoomsController` - 6 edges
10. `User` - 6 edges

## Surprising Connections (you probably didn't know these)
- `Meeting-Room-Booking-API (Web API)` --references--> `Meeting-Room-Booking-API.Application`  [EXTRACTED]
  Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj → Meeting-Room-Booking-API.Application/Meeting-Room-Booking-API.Application.csproj
- `Meeting-Room-Booking-API (Web API)` --references--> `Meeting-Room-Booking-API.Infrastructure`  [EXTRACTED]
  Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj → Meeting-Room-Booking-API.Infrastructure/Meeting-Room-Booking-API.Infrastructure.csproj
- `Meeting-Room-Booking-API (Web API)` --references--> `Meeting-Room-Booking-API.Domain`  [EXTRACTED]
  Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj → Meeting-Room-Booking-API.Domain/Meeting-Room-Booking-API.Domain.csproj
- `Meeting-Room-Booking-API.Tests` --references--> `Meeting-Room-Booking-API (Web API)`  [EXTRACTED]
  Meeting-Room-Booking-API.Tests/Meeting-Room-Booking-API.Tests.csproj → Meeting-Room-Booking-API/Meeting-Room-Booking-API.csproj
- `Meeting-Room-Booking-API.Infrastructure` --references--> `Meeting-Room-Booking-API.Application`  [EXTRACTED]
  Meeting-Room-Booking-API.Infrastructure/Meeting-Room-Booking-API.Infrastructure.csproj → Meeting-Room-Booking-API.Application/Meeting-Room-Booking-API.Application.csproj

## Hyperedges (group relationships)
- **Clean Architecture Layers** — readme_domain_layer, readme_application_layer, readme_infrastructure_layer, readme_webapi_layer [EXTRACTED 1.00]
- **Clean Architecture Layers** — project_domain, project_application, project_infrastructure, project_webapi [EXTRACTED 1.00]

## Communities

### Community 0 - "Community 0"
Cohesion: 0.1
Nodes (1): BookingOverlapTests

### Community 1 - "Community 1"
Cohesion: 0.12
Nodes (4): AuthController, BookingsController, ControllerBase, RoomsController

### Community 2 - "Community 2"
Cohesion: 0.23
Nodes (1): DtoValidationTests

### Community 3 - "Community 3"
Cohesion: 0.14
Nodes (5): AuthRateLimitTests, GlobalRateLimitTests, IClassFixture, SecurityHeaderTests, StartupTests

### Community 4 - "Community 4"
Cohesion: 0.32
Nodes (2): AuthService, IAuthService

### Community 5 - "Community 5"
Cohesion: 0.2
Nodes (4): BookingConfiguration, IEntityTypeConfiguration, RoomConfiguration, UserConfiguration

### Community 6 - "Community 6"
Cohesion: 0.22
Nodes (2): BookingRepository, IBookingRepository

### Community 7 - "Community 7"
Cohesion: 0.25
Nodes (1): IBookingRepository

### Community 8 - "Community 8"
Cohesion: 0.25
Nodes (2): IRoomRepository, RoomRepository

### Community 9 - "Community 9"
Cohesion: 0.25
Nodes (2): IUserRepository, UserRepository

### Community 10 - "Community 10"
Cohesion: 0.29
Nodes (1): User

### Community 11 - "Community 11"
Cohesion: 0.29
Nodes (1): IRoomRepository

### Community 12 - "Community 12"
Cohesion: 0.29
Nodes (1): IUserRepository

### Community 13 - "Community 13"
Cohesion: 0.48
Nodes (1): ExceptionHandlingMiddlewareTests

### Community 14 - "Community 14"
Cohesion: 0.33
Nodes (2): BookingService, IBookingService

### Community 15 - "Community 15"
Cohesion: 0.33
Nodes (1): IAuthService

### Community 16 - "Community 16"
Cohesion: 0.53
Nodes (1): DatabaseSeeder

### Community 17 - "Community 17"
Cohesion: 0.33
Nodes (1): BookingsControllerTests

### Community 18 - "Community 18"
Cohesion: 0.47
Nodes (1): AuthServiceTests

### Community 19 - "Community 19"
Cohesion: 0.4
Nodes (1): DependencyInjection

### Community 20 - "Community 20"
Cohesion: 0.4
Nodes (1): IBookingService

### Community 21 - "Community 21"
Cohesion: 0.4
Nodes (5): Meeting Room Booking API, ASP.NET Core 8.0, Clean Architecture, InMemory database, InMemory database rationale

### Community 22 - "Community 22"
Cohesion: 0.7
Nodes (5): Meeting-Room-Booking-API.Application, Meeting-Room-Booking-API.Domain, Meeting-Room-Booking-API.Infrastructure, Meeting-Room-Booking-API.Tests, Meeting-Room-Booking-API (Web API)

### Community 23 - "Community 23"
Cohesion: 0.67
Nodes (1): ExceptionHandlingMiddleware

### Community 24 - "Community 24"
Cohesion: 0.67
Nodes (1): Room

### Community 25 - "Community 25"
Cohesion: 0.5
Nodes (2): ApplicationDbContext, DbContext

### Community 26 - "Community 26"
Cohesion: 0.5
Nodes (1): RoomsControllerTests

### Community 27 - "Community 27"
Cohesion: 0.5
Nodes (4): GRAPH_REPORT.md, graphify knowledge graph, Rebuild code graph rationale, wiki/index.md

### Community 28 - "Community 28"
Cohesion: 0.67
Nodes (4): Meeting-Room-Booking-API.Application, Meeting-Room-Booking-API.Domain, Meeting-Room-Booking-API.Infrastructure, WebAPI

### Community 29 - "Community 29"
Cohesion: 0.67
Nodes (2): LoginRequest, RegisterRequest

### Community 30 - "Community 30"
Cohesion: 0.67
Nodes (1): UnitTest1

### Community 31 - "Community 31"
Cohesion: 0.67
Nodes (3): Authentication, JWT Bearer token, Refresh token rotation

### Community 32 - "Community 32"
Cohesion: 0.67
Nodes (1): BookingIntegrationTests

### Community 33 - "Community 33"
Cohesion: 1.0
Nodes (1): Program

### Community 34 - "Community 34"
Cohesion: 1.0
Nodes (0): 

### Community 35 - "Community 35"
Cohesion: 1.0
Nodes (1): Class1

### Community 36 - "Community 36"
Cohesion: 1.0
Nodes (1): Booking

### Community 37 - "Community 37"
Cohesion: 1.0
Nodes (2): Dependency Rule rationale, Dependency Rule

### Community 38 - "Community 38"
Cohesion: 1.0
Nodes (0): 

### Community 39 - "Community 39"
Cohesion: 1.0
Nodes (0): 

### Community 40 - "Community 40"
Cohesion: 1.0
Nodes (0): 

### Community 41 - "Community 41"
Cohesion: 1.0
Nodes (0): 

### Community 42 - "Community 42"
Cohesion: 1.0
Nodes (0): 

### Community 43 - "Community 43"
Cohesion: 1.0
Nodes (0): 

### Community 44 - "Community 44"
Cohesion: 1.0
Nodes (0): 

### Community 45 - "Community 45"
Cohesion: 1.0
Nodes (0): 

### Community 46 - "Community 46"
Cohesion: 1.0
Nodes (0): 

### Community 47 - "Community 47"
Cohesion: 1.0
Nodes (0): 

### Community 48 - "Community 48"
Cohesion: 1.0
Nodes (0): 

## Knowledge Gaps
- **17 isolated node(s):** `Program`, `Class1`, `RegisterRequest`, `LoginRequest`, `Booking` (+12 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **Thin community `Community 33`** (2 nodes): `Program.cs`, `Program`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 34`** (2 nodes): `Requests.cs`, `Validate()`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 35`** (2 nodes): `Class1`, `Class1.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 36`** (2 nodes): `Booking`, `Booking.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 37`** (2 nodes): `Dependency Rule rationale`, `Dependency Rule`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 38`** (1 nodes): `Meeting-Room-Booking-API.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 39`** (1 nodes): `Meeting-Room-Booking-API.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 40`** (1 nodes): `Meeting-Room-Booking-API.MvcApplicationPartsAssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 41`** (1 nodes): `Meeting-Room-Booking-API.Application.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 42`** (1 nodes): `Meeting-Room-Booking-API.Application.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 43`** (1 nodes): `Meeting-Room-Booking-API.Domain.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 44`** (1 nodes): `Meeting-Room-Booking-API.Domain.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 45`** (1 nodes): `Meeting-Room-Booking-API.Infrastructure.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 46`** (1 nodes): `Meeting-Room-Booking-API.Infrastructure.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 47`** (1 nodes): `Meeting-Room-Booking-API.Tests.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 48`** (1 nodes): `Meeting-Room-Booking-API.Tests.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What connects `Program`, `Class1`, `RegisterRequest` to the rest of the system?**
  _17 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.1 - nodes in this community are weakly interconnected._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.12 - nodes in this community are weakly interconnected._
- **Should `Community 3` be split into smaller, more focused modules?**
  _Cohesion score 0.14 - nodes in this community are weakly interconnected._