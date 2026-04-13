# Graph Report - .  (2026-04-12)

## Corpus Check
- Corpus is ~10,802 words - fits in a single context window. You may not need a graph.

## Summary
- 257 nodes · 250 edges · 48 communities detected
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Community Hubs (Navigation)
- [[_COMMUNITY_Booking Overlap Tests|Booking Overlap Tests]]
- [[_COMMUNITY_API Controllers|API Controllers]]
- [[_COMMUNITY_DTO Validation Tests|DTO Validation Tests]]
- [[_COMMUNITY_Integration & Rate Limit Tests|Integration & Rate Limit Tests]]
- [[_COMMUNITY_Auth Service Implementation|Auth Service Implementation]]
- [[_COMMUNITY_Entity Configurations|Entity Configurations]]
- [[_COMMUNITY_Booking Repository Implementation|Booking Repository Implementation]]
- [[_COMMUNITY_IBookingRepository Interface|IBookingRepository Interface]]
- [[_COMMUNITY_Room Repository|Room Repository]]
- [[_COMMUNITY_User Repository|User Repository]]
- [[_COMMUNITY_User Entity Logic|User Entity Logic]]
- [[_COMMUNITY_IRoomRepository Interface|IRoomRepository Interface]]
- [[_COMMUNITY_IUserRepository Interface|IUserRepository Interface]]
- [[_COMMUNITY_Exception Middleware Tests|Exception Middleware Tests]]
- [[_COMMUNITY_Booking Service Implementation|Booking Service Implementation]]
- [[_COMMUNITY_IAuthService Interface|IAuthService Interface]]
- [[_COMMUNITY_Database Seeding|Database Seeding]]
- [[_COMMUNITY_Booking Controller Tests|Booking Controller Tests]]
- [[_COMMUNITY_Auth Service Tests|Auth Service Tests]]
- [[_COMMUNITY_Dependency Injection|Dependency Injection]]
- [[_COMMUNITY_IBookingService Interface|IBookingService Interface]]
- [[_COMMUNITY_Project Documentation|Project Documentation]]
- [[_COMMUNITY_Project Architecture|Project Architecture]]
- [[_COMMUNITY_Exception Middleware|Exception Middleware]]
- [[_COMMUNITY_Room Entity Logic|Room Entity Logic]]
- [[_COMMUNITY_Database Context|Database Context]]
- [[_COMMUNITY_Room Controller Tests|Room Controller Tests]]
- [[_COMMUNITY_Gemini & Graphify Documentation|Gemini & Graphify Documentation]]
- [[_COMMUNITY_Architecture Documentation|Architecture Documentation]]
- [[_COMMUNITY_Auth DTOs|Auth DTOs]]
- [[_COMMUNITY_Basic Unit Tests|Basic Unit Tests]]
- [[_COMMUNITY_Auth Documentation|Auth Documentation]]
- [[_COMMUNITY_Application Startup|Application Startup]]
- [[_COMMUNITY_Request Validation|Request Validation]]
- [[_COMMUNITY_Application Logic|Application Logic]]
- [[_COMMUNITY_Booking Entity|Booking Entity]]
- [[_COMMUNITY_Dependency Rules|Dependency Rules]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Community 43|Community 43]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]
- [[_COMMUNITY_Build Metadata|Build Metadata]]

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

### Community 0 - "Booking Overlap Tests"
Cohesion: 0.1
Nodes (1): BookingOverlapTests

### Community 1 - "API Controllers"
Cohesion: 0.12
Nodes (4): AuthController, BookingsController, ControllerBase, RoomsController

### Community 2 - "DTO Validation Tests"
Cohesion: 0.23
Nodes (1): DtoValidationTests

### Community 3 - "Integration & Rate Limit Tests"
Cohesion: 0.14
Nodes (5): AuthRateLimitTests, GlobalRateLimitTests, IClassFixture, SecurityHeaderTests, StartupTests

### Community 4 - "Auth Service Implementation"
Cohesion: 0.32
Nodes (2): AuthService, IAuthService

### Community 5 - "Entity Configurations"
Cohesion: 0.2
Nodes (4): BookingConfiguration, IEntityTypeConfiguration, RoomConfiguration, UserConfiguration

### Community 6 - "Booking Repository Implementation"
Cohesion: 0.22
Nodes (2): BookingRepository, IBookingRepository

### Community 7 - "IBookingRepository Interface"
Cohesion: 0.25
Nodes (1): IBookingRepository

### Community 8 - "Room Repository"
Cohesion: 0.25
Nodes (2): IRoomRepository, RoomRepository

### Community 9 - "User Repository"
Cohesion: 0.25
Nodes (2): IUserRepository, UserRepository

### Community 10 - "User Entity Logic"
Cohesion: 0.29
Nodes (1): User

### Community 11 - "IRoomRepository Interface"
Cohesion: 0.29
Nodes (1): IRoomRepository

### Community 12 - "IUserRepository Interface"
Cohesion: 0.29
Nodes (1): IUserRepository

### Community 13 - "Exception Middleware Tests"
Cohesion: 0.48
Nodes (1): ExceptionHandlingMiddlewareTests

### Community 14 - "Booking Service Implementation"
Cohesion: 0.33
Nodes (2): BookingService, IBookingService

### Community 15 - "IAuthService Interface"
Cohesion: 0.33
Nodes (1): IAuthService

### Community 16 - "Database Seeding"
Cohesion: 0.53
Nodes (1): DatabaseSeeder

### Community 17 - "Booking Controller Tests"
Cohesion: 0.33
Nodes (1): BookingsControllerTests

### Community 18 - "Auth Service Tests"
Cohesion: 0.47
Nodes (1): AuthServiceTests

### Community 19 - "Dependency Injection"
Cohesion: 0.4
Nodes (1): DependencyInjection

### Community 20 - "IBookingService Interface"
Cohesion: 0.4
Nodes (1): IBookingService

### Community 21 - "Project Documentation"
Cohesion: 0.4
Nodes (5): Meeting Room Booking API, ASP.NET Core 8.0, Clean Architecture, InMemory database, InMemory database rationale

### Community 22 - "Project Architecture"
Cohesion: 0.7
Nodes (5): Meeting-Room-Booking-API.Application, Meeting-Room-Booking-API.Domain, Meeting-Room-Booking-API.Infrastructure, Meeting-Room-Booking-API.Tests, Meeting-Room-Booking-API (Web API)

### Community 23 - "Exception Middleware"
Cohesion: 0.67
Nodes (1): ExceptionHandlingMiddleware

### Community 24 - "Room Entity Logic"
Cohesion: 0.67
Nodes (1): Room

### Community 25 - "Database Context"
Cohesion: 0.5
Nodes (2): ApplicationDbContext, DbContext

### Community 26 - "Room Controller Tests"
Cohesion: 0.5
Nodes (1): RoomsControllerTests

### Community 27 - "Gemini & Graphify Documentation"
Cohesion: 0.5
Nodes (4): GRAPH_REPORT.md, graphify knowledge graph, Rebuild code graph rationale, wiki/index.md

### Community 28 - "Architecture Documentation"
Cohesion: 0.67
Nodes (4): Meeting-Room-Booking-API.Application, Meeting-Room-Booking-API.Domain, Meeting-Room-Booking-API.Infrastructure, WebAPI

### Community 29 - "Auth DTOs"
Cohesion: 0.67
Nodes (2): LoginRequest, RegisterRequest

### Community 30 - "Basic Unit Tests"
Cohesion: 0.67
Nodes (1): UnitTest1

### Community 31 - "Auth Documentation"
Cohesion: 0.67
Nodes (3): Authentication, JWT Bearer token, Refresh token rotation

### Community 32 - "Application Startup"
Cohesion: 1.0
Nodes (1): Program

### Community 33 - "Request Validation"
Cohesion: 1.0
Nodes (0): 

### Community 34 - "Application Logic"
Cohesion: 1.0
Nodes (1): Class1

### Community 35 - "Booking Entity"
Cohesion: 1.0
Nodes (1): Booking

### Community 36 - "Dependency Rules"
Cohesion: 1.0
Nodes (2): Dependency Rule rationale, Dependency Rule

### Community 37 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 38 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 39 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 40 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 41 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 42 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 43 - "Community 43"
Cohesion: 1.0
Nodes (0): 

### Community 44 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 45 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 46 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

### Community 47 - "Build Metadata"
Cohesion: 1.0
Nodes (0): 

## Knowledge Gaps
- **17 isolated node(s):** `Program`, `Class1`, `RegisterRequest`, `LoginRequest`, `Booking` (+12 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **Thin community `Application Startup`** (2 nodes): `Program.cs`, `Program`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Request Validation`** (2 nodes): `Requests.cs`, `Validate()`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Application Logic`** (2 nodes): `Class1`, `Class1.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Booking Entity`** (2 nodes): `Booking`, `Booking.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Dependency Rules`** (2 nodes): `Dependency Rule rationale`, `Dependency Rule`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.MvcApplicationPartsAssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Application.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Application.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Domain.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Community 43`** (1 nodes): `Meeting-Room-Booking-API.Domain.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Infrastructure.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Infrastructure.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Tests.AssemblyInfo.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.
- **Thin community `Build Metadata`** (1 nodes): `Meeting-Room-Booking-API.Tests.GlobalUsings.g.cs`
  Too small to be a meaningful cluster - may be noise or needs more connections extracted.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What connects `Program`, `Class1`, `RegisterRequest` to the rest of the system?**
  _17 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Booking Overlap Tests` be split into smaller, more focused modules?**
  _Cohesion score 0.1 - nodes in this community are weakly interconnected._
- **Should `API Controllers` be split into smaller, more focused modules?**
  _Cohesion score 0.12 - nodes in this community are weakly interconnected._
- **Should `Integration & Rate Limit Tests` be split into smaller, more focused modules?**
  _Cohesion score 0.14 - nodes in this community are weakly interconnected._