# CiAi.CogA readiness

**Implementation readiness: 40% — validated offline vertical slice.**

## Implemented in `development`

- [x] Dependency-free .NET 8 catalog library.
- [x] Canonical `owner/name` repository identity parser.
- [x] Typed capability definitions and repository-scoped declarations.
- [x] Typed directed relationships with contract version, optional trigger, and named authorization requirement.
- [x] Deterministic structural validator with stable issue codes.
- [x] Executable offline contract checks, including malformed identities, duplicates, unknown references, incompatible versions, invalid endpoints, and self-edges.
- [x] Runnable offline catalog sample.
- [x] README, Gallery, source companion guides, UML, recipe, and integration proposal.
- [x] CI workflow definition with immutable checkout action reference.
- [x] Successful GitHub Actions run for the proposal commit: build, offline contract checks, and sample all completed successfully.

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

- Workflow: [CogA catalog validation run #28](https://github.com/TrentBest/TheSingularityWorkshop.CiAi.CogA/actions/runs/40036766541)
- Validated commit at that run: `1ebc881cd86902e4c4b9f28b56ac3a6d57fcf3c8`
- Result: **success**. The build, offline contract checks, and sample steps all passed.
- The .NET SDK was not installed in the assistant execution environment, so validation was performed by the repository's GitHub Actions workflow rather than a local run.

The runnable checks can be repeated with:

```bash
dotnet run --project samples/CiAi.CogA.Sample/CiAi.CogA.Sample.csproj --configuration Release
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj --configuration Release
```

## Safety boundary

CogA intentionally performs no GitHub API calls, dispatch, credential handling, publishing, deployment, or GUI rendering. A structurally valid catalog does not prove that repositories exist or that a relationship is authorized or operational. The current 40% estimate reflects a working, tested offline slice while shared vocabulary, evidence ingestion, integration contracts, packaging, and presentation remain unfinished.
