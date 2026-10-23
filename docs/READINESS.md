# CiAi.CogA readiness

**Implementation readiness: 40% — validated offline vertical slice.**

## Implemented in `development`

- [x] Dependency-free .NET 8 catalog library.
- [x] Canonical `owner/name` repository identity parser.
- [x] Typed capability definitions and repository-scoped declarations.
- [x] Typed directed relationships with contract version, optional trigger, and named authorization requirement.
- [x] Deterministic structural validator with stable issue codes.
- [x] Executable offline contract checks, including malformed identities, duplicates, unknown references, incompatible versions, invalid endpoints, self-edges, malformed capability IDs/versions, malformed relationship IDs, and invalid enum values.
- [x] Runnable offline catalog sample.
- [x] README, Gallery, source companion guides, UML, recipe, and integration proposal.
- [x] CI workflow definition with immutable checkout action reference.
- [x] Successful GitHub Actions runs for the latest test commit: push validation and pull-request validation both passed.

## Not yet implemented / reconciled

- [ ] Shared vocabulary reconciled with CiAi and ProtocolAi.
- [ ] Version range negotiation beyond major-version compatibility.
- [ ] Import/export or persisted catalog format.
- [ ] Validation of authoritative GitHub repository and workflow evidence.
- [ ] Integration with CogB/CogC envelopes.
- [ ] Presentation-neutral projection consumed by CogE.
- [ ] GUI.SVG/GUI.Blazor projection.
- [ ] Package packing and API compatibility checks.
- [ ] Review and reconciliation of the proposed contract with the parent CiAi contract.

## Validation evidence

Latest test commit: [`12e28ac737b561bb77bd3078549d5a18c0c628e2`](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/commit/12e28ac737b561bb77bd3078549d5a18c0c628e2)

- [Push validation run #34](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/actions/runs/40038737560) — **success**.
- [Pull-request validation run #35](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/actions/runs/40038742783) — **success**.
- The executable offline test suite now contains 14 checks. The latest commit adds five rejection checks for malformed capability IDs, malformed capability versions, malformed relationship IDs, invalid relationship types, and invalid declaration roles.
- The .NET SDK was not installed in the assistant execution environment, so validation was performed by GitHub Actions rather than a local run. The later documentation commits are now covered by [CogA catalog validation run #41](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/actions/runs/40039240816) — **success** — for head commit [`166d946ae54c4bd58e756a559eceeec204f03a2f`](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/commit/166d946ae54c4bd58e756a559eceeec204f03a2f). The combined commit-status endpoint returned no status entries; run #41 is the observed workflow evidence. This documentation run does not replace the two successful push/PR validation runs for the latest test commit.

The runnable checks can be repeated with:

```bash
dotnet run --project samples/CiAi.CogA.Sample/CiAi.CogA.Sample.csproj --configuration Release
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj --configuration Release
```

## Safety boundary

CogA intentionally performs no GitHub API calls, dispatch, credential handling, publishing, deployment, or GUI rendering. A structurally valid catalog does not prove that repositories exist or that a relationship is authorized or operational. The current 40% estimate reflects a working, tested offline slice while shared vocabulary, evidence ingestion, integration contracts, packaging, and presentation remain unfinished.
