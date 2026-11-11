# EquipIQ Progress

Paste this file into a new chat to continue.

## Project root
`C:\Users\pierr\source\repos\equipiq` (GitHub: pierregerges/equipiq, public, MIT)

## Working style
- Visual Studio 2026 for .NET, VS Code for Angular. Use UI where possible; terminal only when needed.
- Branching: `main` is protected. One short branch per phase, small commits, pull request into `main` at phase end.
- Commits through the Git Changes window (Commit All and Sync).
- Budget: max $25/month on Azure (personal Pay-As-You-Go, region East US).
- Free and open-source packages only (MIT/Apache). Check license before adding.

## Environment (verified 2026-11-11)
- Windows 11 Home 25H2, Visual Studio 2026 Community 18.11.2
- .NET SDK 10.0.403, Node 24.20.0, npm 11.19.0, Angular CLI 22.1.6, Git 2.55.0
- Docker Desktop installed, hello-world works
- Azure CLI installed; signed in to tenant e61bab44-58d4-49a9-a71c-dbf72025c4d7 (Default Directory), subscription "Azure subscription 1"
- Aspire: no separate workload needed; use the Visual Studio templates

## Decisions
- Visual Studio 2026.
- Trunk-based workflow (main + short branches), no long-lived development branch. Staging slot in Azure handles pre-production testing.
- Naming guide: docs/naming-guide.md
- Azure resource group: rg-equipiq-dev, region East US, tags project=equipiq, environment=dev

## Completed steps
- Phase 0 complete:
  - Tools installed and verified (VS 2026, .NET 10, Node, Angular CLI, Docker, Azure CLI, Git)
  - GitHub repo created, folder layout, ignore/config files, naming guide, Copilot instructions
  - Azure: resource group `rg-equipiq-dev` (East US), monthly budget `budget-equipiq-monthly` ($25, alerts at 50/80/100% actual and 100% forecast)

## Next step
Phase 1, Step 1: create the solution with EquipIQ.Core, Application, Infrastructure, Api, and the Aspire AppHost in Visual Studio 2026.ns.md