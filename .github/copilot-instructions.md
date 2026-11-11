# EquipIQ: Instructions for GitHub Copilot

EquipIQ is an AI knowledge and maintenance assistant for medical equipment.
Stack: .NET 10 (ASP.NET Core Web API, EF Core, SQL Server), Angular 22, Azure.
The owner is a learner: write clear code, use plain names, add short comments that explain WHY.

## General rules
- Follow docs/naming-guide.md for every name.
- Never put secrets, keys, or connection strings in code. Use user secrets locally and Key Vault with managed identity in Azure.
- Use only free, open-source packages (MIT or Apache 2.0). Do NOT suggest MediatR 13+, AutoMapper, MassTransit 9, or FluentAssertions 8+.
- Tests: MSTest only (no xUnit, no NUnit). Write tests with every feature.
- Security first: validate every input at the edge, never trust client data, use parameterized data access only, return ProblemDetails for errors without leaking internals.

## .NET: Clean Architecture
Projects: EquipIQ.Core, EquipIQ.Application, EquipIQ.Infrastructure, EquipIQ.Api (plus Worker, Functions, McpServer, AppHost, ServiceDefaults).
- Dependencies point inward only: Api and Infrastructure -> Application -> Core. Core depends on nothing (no EF Core, Azure, or ASP.NET).
- Core: rich domain entities (private setters, methods that protect their own rules), value objects, domain events, domain services, repository and service interfaces, domain exceptions, enums.
- Application: CQRS with the free MIT Mediator source-generator library (hand-written dispatcher if its license is in doubt). Commands change state, queries only read. Each has its own handler, FluentValidation validator, and DTO. Logging, validation, and exception handling live in pipeline behaviors, not in handlers. Domain event handlers live here. DI setup is in an AddApplication extension method.
- Infrastructure: EF Core DbContext and migrations, repositories, Blob Storage, Service Bus, Azure AI Search, Azure OpenAI, Redis, Key Vault. DI setup is in an AddInfrastructure extension method.
- Api: thin controllers that only send commands and queries and return results. Middleware, authentication, Scalar, and the composition root live here. Never return entities, only DTOs.
- Domain events are raised by entities and dispatched only after the database save succeeds.
- Use SOLID, constructor injection, interfaces for anything with external behavior, composition over inheritance.
- Nullable reference types on, warnings as errors, analyzers on, async all the way with CancellationToken, records for commands, queries, and DTOs, central package management (Directory.Packages.props).
- Global exception middleware maps domain exceptions to HTTP status codes using ProblemDetails.
- Logging with Serilog (structured). Traces and metrics with OpenTelemetry.
- Architecture tests (NetArchTest with MSTest) must fail if a dependency rule is broken.

## Angular: standalone, feature-based, no store
- Standalone components only. No NgModules, no CommonModule, no app-routing module.
- Use built-in control flow (@if, @for, @switch), inject() instead of constructor injection, and signals for state.
- Each component lists exactly what it needs in its own imports array.
- Routing: app.routes.ts plus a routes file per feature, lazy-loaded with loadChildren or loadComponent.
- Feature folders (documents, chat, work-orders, dashboard) are self-contained: container components, presentational components, services, models, guards, routes. Features never import from each other. Shared code lives in a small shared folder (layout, common UI, auth).
- No NgRx or any store. Services hold state: private writable signals, public read-only signals and computed values, update methods, and HttpClient calls inside the service. Components stay thin.
- Typed models for every API response. Angular Material for UI. MSAL Angular for login.
- Never store tokens or secrets in code. Sanitize anything rendered as HTML.

## Azure and AI
- Authentication: Entra ID with OAuth2 and OpenID Connect, authorization code flow with PKCE. Roles: Admin and Technician.
- Use managed identity and Key Vault for secrets. Check current Microsoft docs before using fast-changing APIs (Aspire, Foundry IQ, MCP C# SDK, Agent Framework, Azure AI Search vector retrieval).
- RAG answers must come only from retrieved manual text, say when the answer is unknown, and cite document name and page.
- Treat text from uploaded documents as untrusted input (prompt injection risk).