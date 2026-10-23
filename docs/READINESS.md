# CiAi.CogA readiness

**Implementation readiness: 35% — initial offline vertical slice.**

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

## Not yet verified / not implemented

- [ ] CI workflow has run green on the current commit.
- [ ] Shared vocabulary reconciled with CiAi and ProtocolAi.
- [ ] Version range negotiation beyond major-version compatibility.
- [ ] Import/export or persisted catalog format.
- [ ] Validation of authoritative GitHub repository and workflow evidence.
- [ ] Integration with CogB/CogC envelopes.
- [ ] Presentation-neutral projection consumed by CogE.
- [ ] GUI.SVG/GUI.Blazor projection.
- [ ] Package packing and API compatibility checks.

## Evidence and limits

The source, sample, and tests are committed to the `development` branch. Run the sample and checks with:

```bash
dotnet run --project samples/CiAi.CogA.Sample/CiAi.CogA.Sample.csproj --configuration Release
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj --configuration Release
```

The CI workflow builds the library, runs the offline checks, and runs the sample. I could not execute .NET locally in this session because the .NET SDK is not installed in the execution environment. The GitHub status query currently returns no checks for the latest workflow commit, so CI status is **unverified**, not green.

CogA intentionally performs no GitHub API calls, dispatch, credential handling, publishing, deployment, or GUI rendering. A structurally valid catalog does not prove that repositories exist or that a relationship is authorized or operational. The 35% estimate reflects the narrow offline slice and the substantial integration contract still outstanding.
