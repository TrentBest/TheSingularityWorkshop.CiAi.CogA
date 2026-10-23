![The Singularity Workshop moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.GUI/development/docs/assets/fox-sheepdog-10x10.svg)

# CiAi.CogA — Repository Catalog

**CogA owns the offline catalog model:** canonical repository identities, declared capabilities, and directed, versioned relationships. It is an experimental design slice within [CiAi](https://github.com/TrentBest/TheSingularityWorkshop.CiAi), not a GitHub client or workflow dispatcher.

The model follows the parent [inter-repository contract](https://github.com/TrentBest/TheSingularityWorkshop.CiAi/blob/development/docs/INTER_REPOSITORY_MODEL.md) and [Cog assignments](https://github.com/TrentBest/TheSingularityWorkshop.CiAi/blob/development/docs/COG_ASSIGNMENTS.md). Repository identity and capability vocabulary are explicit; capability roles are composable; edges point from source to target. Catalog validation is deterministic and runs offline.

## 01 — See it in a minute

Requires the .NET 8 SDK.

```bash
dotnet run --project samples/CiAi.CogA.Sample/CiAi.CogA.Sample.csproj
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj
```

The sample demonstrates a structurally valid two-repository catalog. The executable contract checks use no external test framework, credentials, network access, or GitHub calls. They cover valid and malformed catalog structures, duplicate identities, unknown capabilities, incompatible contract major versions, invalid edge endpoints, self-edges, and declarations for unknown repositories.

## 02 — What CogA models

- **Repository identity** — canonical `owner/name` value.
- **Capability definition** — application-owned vocabulary ID and `major.minor.patch` contract version.
- **Capability declaration** — repository, capability, version, and role such as provider, consumer, observer, or coordinator.
- **Relationship** — stable ID, source, target, relationship type, capability, contract version, optional trigger, and a named authorization requirement. The authorization field is a policy label, never a credential.

A contract version with a different major number is rejected. This initial slice checks version shape and major compatibility; it does not yet implement a general semantic-version range solver or prove that a GitHub repository exists.

## 03 — Safety boundary

CogA performs no GitHub API calls, dispatch, credential handling, publishing, deployment, or GUI rendering. A catalog declaration is data, not permission to execute an operation. GitHub remains authoritative for repository metadata, commits, workflow state, permissions, and run evidence. The initial self-edge rule is intentionally strict and can be revised only through an explicit contract change.

## 04 — Gallery and recipes

- [Gallery.md](Gallery.md) — capability and relationship model overview.
- [Offline catalog recipe](docs/recipes/offline-catalog.md) — construct and validate a small catalog.
- [Integration proposal](docs/INTEGRATION.md) — questions to reconcile before cross-repository integration.
- [Source documentation](src/CiAi.CogA/README.md) — companion guide for the library source.
- [UML class diagram](docs/UML/RepositoryCatalogUML.md) — model relationships and use cases.
- [Readiness](docs/READINESS.md) — implemented scope, remaining work, and evidence.

## 05 — Documentation and theory

The catalog is a projection of declared architecture, not proof of runtime connectivity. A directed edge says what a repository claims to provide, consume, request, or notify; it does not assert that dispatch works or that a workflow succeeded. Later integration should reconcile CogA's proposed vocabulary with CiAi and ProtocolAi before any cross-repository operation is enabled.

## Status

**Implementation readiness: 40%.** The dependency-free .NET library, runnable sample, executable offline validation checks, Workshop documentation, and CI validation have all been exercised successfully on the current proposal commit. This remains an experimental slice, not a production integration: no provider ingestion, persisted manifest format, ProtocolAi binding, semantic-version range negotiation, or GUI projection is claimed complete. Shared vocabulary and integration contract review remain open.

---

The Singularity Workshop · CiAi experimental delegation set
