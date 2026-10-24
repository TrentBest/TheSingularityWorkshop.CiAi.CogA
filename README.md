# The Singularity Workshop CiAi.CogA

**CogA is the CiAi capability catalog.** It records explicit, version-pinned capability declarations; it does not execute provider work or grant permission to invoke it.

## 01 — See it in a minute

Requires the .NET 8 SDK.

```bash
dotnet run --project tests/TheSingularityWorkshop.CiAi.CogA.ContractTests --configuration Release
```

The offline contract suite proves exact lookup, duplicate rejection, provider isolation, stable snapshots, input validation, and exact removal.

## 02 — Responsibility

CogA describes **what capability exists, which provider declares it, and which versions/contracts it supports**. It intentionally does not make authorization decisions (CogD), create requests (CogB), validate returned results (CogC), or present lifecycle evidence (CogE).

The first implementation is an in-memory catalog populated by trusted coordinator configuration or a future verified discovery adapter. Repository search, remote manifests, and provider execution are outside this package's current scope.

## 03 — Trust boundary

- Capability IDs and versions are bounded identifiers.
- Provider identity is an owner/repository pair, not an arbitrary URL.
- Implementation revisions are pinned full Git commit SHAs.
- Registration does not imply authorization or execution success.
- No network access, external dependencies, dynamic code loading, or provider execution.

## Gallery

See [Gallery.md](Gallery.md) for the quick tour and verification recipe.

## 04 — UML and use case

See [CapabilityCatalog UML and use case](docs/UML/CapabilityCatalogUML.md).

See [CapabilityCatalog class companion](src/TheSingularityWorkshop.CiAi.CogA/CapabilityCatalog.md).

## 05 — Release status

**Implementation readiness: 25%. Package preparation: 25%.** The catalog's first typed slice and offline checks are committed; cross-Cog integration, durable catalog storage, signed provider declarations, release hardening, and owner review remain outstanding. This is not a publication approval.
