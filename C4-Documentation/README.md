# C4 Architecture Documentation

Architecture documentation for the **Meeting Room Booking API** following the [C4 Model](https://c4model.com).

## Reading Order (outer → inner)

| Level | File | Audience |
|-------|------|----------|
| 🌍 **Context** | [c4-context.md](c4-context.md) | Everyone — stakeholders, product, business |
| 📦 **Container** | [c4-container.md](c4-container.md) | Architects, DevOps, senior developers |
| 🧩 **Component** | [c4-component.md](c4-component.md) | Software developers |
| 💻 **Code** | See below | Individual contributors |

---

## Code-Level Documents

| File | Directory Covered |
|------|-------------------|
| [c4-code-domain-entities.md](c4-code-domain-entities.md) | Domain/Entities |
| [c4-code-domain-interfaces.md](c4-code-domain-interfaces.md) | Domain/Interfaces + DTOs |
| [c4-code-application-services.md](c4-code-application-services.md) | Application/Services |
| [c4-code-infrastructure.md](c4-code-infrastructure.md) | Infrastructure (Auth, Repos, DB) |
| [c4-code-webapi.md](c4-code-webapi.md) | Web API (Controllers, Middleware) |

---

## Component Documents

| File | Component |
|------|-----------|
| [c4-component-domain.md](c4-component-domain.md) | Domain |
| [c4-component-application.md](c4-component-application.md) | Application |
| [c4-component-infrastructure.md](c4-component-infrastructure.md) | Infrastructure |
| [c4-component-webapi.md](c4-component-webapi.md) | Web API |

---

## API Specification

| File | Description |
|------|-------------|
| [apis/meeting-room-booking-api.yaml](apis/meeting-room-booking-api.yaml) | OpenAPI 3.1 — all endpoints, schemas, auth |
