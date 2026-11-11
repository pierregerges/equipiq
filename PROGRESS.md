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

## Completed steps
- Phase 0: Steps 1-7 done (tools, Docker, Azure CLI login, repo, folder layout, ignore/config files)
- Phase 0: Step 8 (this file and naming guide)

## Next step
Phase 0, Step 9: write .github/copilot-instructions.md