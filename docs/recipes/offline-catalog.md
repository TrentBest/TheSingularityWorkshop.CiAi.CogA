# Recipe: validate an offline repository catalog

## Run the sample

Requires the .NET 8 SDK.

```bash
dotnet run --project samples/CiAi.CogA.Sample/CiAi.CogA.Sample.csproj
```

The sample constructs an in-memory catalog for CiAi and HeadlessAi. It defines a capability, associates a declaration with CiAi, and connects the two repository identities with a directed request relationship. It validates the catalog locally and prints a message that explicitly distinguishes structural validity from authorization or execution.

## Run the adversarial checks

```bash
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj
```

The executable checks cover a valid catalog, malformed and duplicate identities, unknown capabilities, incompatible contract major versions, invalid edge endpoints, duplicate relationship IDs, self-edges, and declarations for unknown repositories.

## Model rules

- Repository IDs use `owner/name`; parsing validates shape only and does not contact GitHub.
- Capability IDs belong to the application vocabulary, not repository display names.
- Contract versions use `major.minor.patch`; this first slice enforces version shape and matching major version.
- A relationship has a source and target. Both endpoints and its capability must be declared.
- A named authorization requirement is descriptive policy metadata, not a secret and not proof of authorization.

A zero-issue result means the in-memory model satisfies these local structural rules. It does **not** mean the repositories exist, are permitted to communicate, have compatible deployed code, or have passing CI.

See [integration proposal](../INTEGRATION.md), [README](../../README.md), and [readiness](../READINESS.md).
