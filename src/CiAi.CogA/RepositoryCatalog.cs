namespace CiAi.CogA;

/// <summary>Canonical owner/name identity for a GitHub repository, without making network calls.</summary>
public sealed record RepositoryId
{
    public string Owner { get; }
    public string Name { get; }
    public string Value => $"{Owner}/{Name}";

    private RepositoryId(string owner, string name) => (Owner, Name) = (owner, name);

    public static bool TryParse(string? value, out RepositoryId? id)
    {
        id = null;
        if (string.IsNullOrWhiteSpace(value) || value.Length > 200 || value.Any(char.IsWhiteSpace))
            return false;

        var parts = value.Split('/');
        if (parts.Length != 2 || !IsSegment(parts[0]) || !IsSegment(parts[1]))
            return false;

        id = new RepositoryId(parts[0], parts[1]);
        return true;
    }

    private static bool IsSegment(string value) =>
        value.Length is > 0 and <= 100 &&
        value.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.');

    public override string ToString() => Value;
}

/// <summary>Application-owned capability vocabulary entry, versioned independently of repository names.</summary>
public sealed record CapabilityDefinition(string Id, string ContractVersion);

/// <summary>Declares a capability that a repository offers or accepts.</summary>
public sealed record CapabilityDeclaration(string CapabilityId, string ContractVersion, CapabilityRole Role);

public enum CapabilityRole
{
    Provides,
    Consumes,
    Requests,
    ReceivesNotifications,
    ExecutesBuilds,
    Coordinates,
    Observes,
    DefinesPolicy,
    Presents
}

public enum RelationshipType
{
    Provides,
    Consumes,
    Requests,
    Notifies,
    ReportsTo
}

/// <summary>A directed, versioned relationship between two known repositories.</summary>
public sealed record RepositoryRelationship(
    string Id,
    string SourceRepository,
    string TargetRepository,
    RelationshipType Type,
    string CapabilityId,
    string ContractVersion,
    string? Trigger = null,
    string? AuthorizationRequirement = null);

/// <summary>A self-contained catalog that can be validated offline.</summary>
public sealed class RepositoryCatalog
{
    public IReadOnlyList<RepositoryId> Repositories { get; }
    public IReadOnlyList<CapabilityDefinition> Capabilities { get; }
    public IReadOnlyList<CapabilityDeclaration> Declarations { get; }
    public IReadOnlyList<RepositoryRelationship> Relationships { get; }

    public RepositoryCatalog(
        IEnumerable<RepositoryId> repositories,
        IEnumerable<CapabilityDefinition> capabilities,
        IEnumerable<CapabilityDeclaration> declarations,
        IEnumerable<RepositoryRelationship> relationships)
    {
        Repositories = Array.AsReadOnly(repositories.ToArray());
        Capabilities = Array.AsReadOnly(capabilities.ToArray());
        Declarations = Array.AsReadOnly(declarations.ToArray());
        Relationships = Array.AsReadOnly(relationships.ToArray());
    }
}

/// <summary>Stable validation issue suitable for tests, logs, and future presentation adapters.</summary>
public sealed record CatalogIssue(string Code, string Subject, string Message);
