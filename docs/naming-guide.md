# EquipIQ Naming Guide

One convention per kind of name. If unsure, copy an existing example.

## C# (.NET)
| What | Convention | Example |
|---|---|---|
| Solution file | PascalCase | `EquipIQ.slnx` |
| Projects and project folders | `EquipIQ.` + layer name | `EquipIQ.Core`, `EquipIQ.Api` |
| Classes, records, enums, methods, properties | PascalCase | `WorkOrder`, `CreateWorkOrderCommand` |
| Interfaces | `I` + PascalCase | `IWorkOrderRepository` |
| Local variables, parameters | camelCase | `workOrder`, `cancellationToken` |
| Private fields | `_` + camelCase | `_repository` |
| Async methods | end with `Async` | `GetByIdAsync` |
| Commands (change data) | Verb + Noun + `Command` | `CreateWorkOrderCommand` |
| Queries (only read) | `Get` + Noun + `Query` | `GetWorkOrderByIdQuery` |
| Handlers | Command or query name + `Handler` | `CreateWorkOrderCommandHandler` |
| Validators | Command or query name + `Validator` | `CreateWorkOrderCommandValidator` |
| DTOs (data sent to or from the API) | Noun + `Dto` | `WorkOrderDto` |
| Domain events | Noun + past-tense verb | `WorkOrderCreated` |
| Domain event handlers | Event name + `Handler` | `WorkOrderCreatedHandler` |
| Test projects | project name + `.UnitTests` etc. | `EquipIQ.Core.UnitTests` |
| Test classes | class under test + `Tests` | `WorkOrderTests` |
| Test methods | Method_Situation_ExpectedResult | `Close_WhenAlreadyClosed_Throws` |

## Database (SQL Server)
- Tables: plural PascalCase (`WorkOrders`, `Documents`, `ChatMessages`)
- Columns: PascalCase (`CreatedAt`, `EquipmentName`)
- Migrations: describe the change (`AddWorkOrdersTable`)

## Angular (TypeScript)
| What | Convention | Example |
|---|---|---|
| Folders and files | kebab-case (lowercase, dashes) | `work-orders`, `work-order-list.component.ts` |
| Classes | PascalCase | `WorkOrderListComponent` |
| Variables, methods, signals | camelCase | `workOrders`, `loadWorkOrders()` |
| Services | `.service.ts` | `work-order.service.ts` |
| Models | `.model.ts` | `work-order.model.ts` |
| Routes | `<feature>.routes.ts` | `work-orders.routes.ts` |
| Component selectors | `app-` + kebab-case | `app-work-order-list` |

## Git
- Branches: `feature/phase-N-short-name` (one per phase, e.g. `feature/phase-1-api`). Bug fixes: `fix/short-name`.
- Commit messages: `type: short description` where type is `feat`, `fix`, `chore`, `docs`, `test`, or `refactor`.
  Example: `feat: add WorkOrder entity with status rules`

## Azure
Pattern: `<type>-equipiq-<environment>`, all lowercase. Environment is `dev` for now.
| Resource | Name |
|---|---|
| Resource group | `rg-equipiq-dev` |
| Key Vault | `kv-equipiq-dev` |
| Storage account (no dashes allowed) | `stequipiqdev` |
| Service Bus namespace | `sb-equipiq-dev` |
| Azure SQL server and database | `sql-equipiq-dev`, `sqldb-equipiq-dev` |
| AI Search | `srch-equipiq-dev` |
| Azure OpenAI | `oai-equipiq-dev` |
| App Service (API, web) | `app-equipiq-api-dev`, `app-equipiq-web-dev` |
| Function App | `func-equipiq-dev` |
| API Management | `apim-equipiq-dev` |

Some names must be globally unique. If one is taken, add a short suffix like `-pg`.

## Docs and repo folders
Top-level repo folders are lowercase (`src`, `docs`, `infra`, `tests`) because that is the standard in almost every repo, and `.github` must be lowercase.