![The Singularity Workshop moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.GUI/development/docs/assets/fox-sheepdog-10x10.svg)

# CatalogValidator.cs — source guide

**Purpose:** apply deterministic, offline validation rules to a `RepositoryCatalog`.

## Validation behavior

- Detect duplicate repository, capability, and relationship identities.
- Validate capability IDs and `major.minor.patch` version shape.
- Reject declarations that reference unknown repositories or capabilities.
- Reject relationships with unknown source/target repositories or unknown capabilities.
- Reject self-edges in this initial contract.
- Reject incompatible contract major versions and unrecognized enum values.
- Return structured `CatalogIssue` entries instead of throwing for ordinary catalog defects.

The validator throws for a null catalog argument. It does not check whether a repository exists on GitHub, infer permission from a declaration, authorize dispatch, negotiate semantic-version ranges, or verify workflow evidence.

## UML class card

```text
CatalogValidator
----------------
+ Validate(catalog : RepositoryCatalog) : IReadOnlyList<CatalogIssue>
- AddDuplicates(values, code, issues)
- IsToken(value) : bool
- IsVersion(value) : bool
- SameMajor(left, right) : bool
```

## Use cases

1. Validate an offline catalog during tests or before a future import.
2. Report machine-readable issue codes for duplicate or malformed entries.
3. Fail closed when a relationship references an unknown capability or incompatible major version.

See [CatalogValidator UML](../../docs/UML/CatalogValidatorUML.md).
