# Recipe: validate an offline repository catalog

This recipe uses the executable checks in `tests/CiAi.CogA.Tests` as the first proof path.

## Run the proof

```bash
dotnet run --project tests/CiAi.CogA.Tests/CiAi.CogA.Tests.csproj
```

The program constructs a small catalog in memory. It defines a capability, associates a declaration with a repository, and connects two catalogued repository identities with a directed relationship. It then checks both the valid case and deliberately malformed cases.

## Model rules

- Repository IDs use `owner/name`; this parser validates shape only and does not contact GitHub.
- Capability IDs belong to the application vocabulary, not repository display names.
- Contract versions use `major.minor.patch`; this first slice enforces version shape and matching major version.
- A relationship has a source and target. Both endpoints and its capability must be declared.
- A named authorization requirement is descriptive policy metadata, not a secret and not proof of authorization.

## Interpret the result

A zero-issue result means the in-memory model satisfies these local structural rules. It does **not** mean the repositories exist, are permitted to communicate, have compatible deployed code, or have passing CI. Those facts require authoritative evidence from their respective systems.

See [README](../../README.md) for the safety boundary and [readiness](../READINESS.md) for missing integration work.
