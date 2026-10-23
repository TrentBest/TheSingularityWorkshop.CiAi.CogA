# RepositoryCatalog — UML

This diagram is derived from the current CogA source types. It models declarations and catalog structure, not runtime connectivity.

```mermaid
classDiagram
    direction TB
    class RepositoryId {
      +string Owner
      +string Name
      +string Value
      +TryParse(value, id) bool
    }
    class CapabilityDefinition {
      +string Id
      +string ContractVersion
    }
    class CapabilityDeclaration {
      +string Repository
      +string CapabilityId
      +string ContractVersion
      +CapabilityRole Role
    }
    class RepositoryRelationship {
      +string Id
      +string SourceRepository
      +string TargetRepository
      +RelationshipType Type
      +string CapabilityId
      +string ContractVersion
      +string Trigger
      +string AuthorizationRequirement
    }
    class RepositoryCatalog {
      +IReadOnlyList~RepositoryId~ Repositories
      +IReadOnlyList~CapabilityDefinition~ Capabilities
      +IReadOnlyList~CapabilityDeclaration~ Declarations
      +IReadOnlyList~RepositoryRelationship~ Relationships
    }
    class CatalogIssue {
      +string Code
      +string Subject
      +string Message
    }
    class CatalogValidator {
      +Validate(catalog) IReadOnlyList~CatalogIssue~
    }

    RepositoryCatalog "1" o-- "*" RepositoryId : contains
    RepositoryCatalog "1" o-- "*" CapabilityDefinition : defines
    RepositoryCatalog "1" o-- "*" CapabilityDeclaration : declares
    RepositoryCatalog "1" o-- "*" RepositoryRelationship : relates
    CatalogValidator ..> RepositoryCatalog : validates
    CatalogValidator ..> CatalogIssue : returns
    CapabilityDeclaration ..> CapabilityDefinition : references
    RepositoryRelationship ..> CapabilityDefinition : references
```

## Use cases

- **Build a catalog:** provide known repository identities, capability vocabulary, declarations, and directed relationships.
- **Validate before integration:** run `CatalogValidator.Validate` and inspect stable issue codes.
- **Explain topology:** future consumers can project the same model into text or a GUI without treating declarations as permissions.
- **Reconcile vocabulary:** send contract proposals to CiAi/ProtocolAi owners before using this experimental vocabulary as a shared wire contract.

## Important distinction

A valid edge is a structurally valid claim. It is not proof of dispatch, authorization, a successful workflow, or current repository state.
