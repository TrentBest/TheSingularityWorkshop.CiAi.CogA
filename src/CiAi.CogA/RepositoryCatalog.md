![The Singularity Workshop moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.GUI/development/docs/assets/fox-sheepdog-10x10.svg)

# RepositoryCatalog.cs — source guide

**Purpose:** defines the offline catalog's typed data model. It makes repository identities, application-owned capabilities, repository declarations, relationship direction, contract versions, and validation issues explicit.

## Types in this source

- `RepositoryId` parses a canonical `owner/name` identity without network access.
- `CapabilityDefinition` names an application-owned capability and its contract version.
- `CapabilityDeclaration` ties a role and version to a specific repository.
- `CapabilityRole` describes a repository's declared responsibility; roles can coexist.
- `RelationshipType` defines the meaning of a directed edge.
- `RepositoryRelationship` carries source, target, capability, version, optional trigger, and a named authorization requirement.
- `RepositoryCatalog` groups the definitions and declarations to be validated.
- `CatalogIssue` carries a stable issue code, subject, and human-readable explanation.

## UML class card

```text
RepositoryCatalog
-----------------
+ Repositories : IReadOnlyList<RepositoryId>
+ Capabilities : IReadOnlyList<CapabilityDefinition>
+ Declarations : IReadOnlyList<CapabilityDeclaration>
+ Relationships : IReadOnlyList<RepositoryRelationship>
+ RepositoryCatalog(repositories, capabilities, declarations, relationships)
```

## Use cases

1. Parse a repository identifier before admitting it to a catalog.
2. Declare a repository's provided or consumed capability at a contract version.
3. Describe a directed relationship without asserting that a live workflow has run.
4. Return structured validation issues for tests or a future presentation adapter.

See [RepositoryCatalog UML and relationship diagram](../../docs/UML/RepositoryCatalogUML.md). The source is deliberately offline and provider-neutral.
