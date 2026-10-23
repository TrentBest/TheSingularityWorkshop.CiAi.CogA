namespace CiAi.CogA;

/// <summary>Deterministic validation rules for an offline repository catalog.</summary>
public static class CatalogValidator
{
    public static IReadOnlyList<CatalogIssue> Validate(RepositoryCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        var issues = new List<CatalogIssue>();

        var repositoryIds = catalog.Repositories.Select(r => r.Value).ToArray();
        AddDuplicates(repositoryIds, "repository.duplicate", issues);

        var capabilityIds = catalog.Capabilities.Select(c => c.Id).ToArray();
        AddDuplicates(capabilityIds, "capability.duplicate", issues);

        foreach (var capability in catalog.Capabilities)
        {
            if (!IsToken(capability.Id))
                issues.Add(new("capability.malformed-id", capability.Id, "Capability IDs must be non-empty ASCII tokens."));
            if (!IsVersion(capability.ContractVersion))
                issues.Add(new("version.malformed", capability.Id, $"Invalid contract version '{capability.ContractVersion}'. Use major.minor.patch."));
        }

        var knownRepositories = repositoryIds.ToHashSet(StringComparer.OrdinalIgnoreCase);
        var knownCapabilities = catalog.Capabilities
            .Where(c => IsToken(c.Id) && IsVersion(c.ContractVersion))
            .ToDictionary(c => c.Id, StringComparer.Ordinal);
        var declarations = new HashSet<(string Repository, string Capability, CapabilityRole Role)>();

        foreach (var declaration in catalog.Declarations)
        {
            if (!knownCapabilities.TryGetValue(declaration.CapabilityId, out var definition))
            {
                issues.Add(new("capability.unknown", declaration.CapabilityId, "Declaration references an undefined capability."));
                continue;
            }

            if (!IsVersion(declaration.ContractVersion) ||
                !SameMajor(definition.ContractVersion, declaration.ContractVersion))
                issues.Add(new("version.incompatible", declaration.CapabilityId, "Declaration contract version is invalid or has a different major version from its capability definition."));

            if (!Enum.IsDefined(declaration.Role))
                issues.Add(new("declaration.invalid-role", declaration.CapabilityId, "Declaration role is not recognized."));
        }

        var relationshipIds = catalog.Relationships.Select(r => r.Id).ToArray();
        AddDuplicates(relationshipIds, "relationship.duplicate", issues);

        foreach (var relationship in catalog.Relationships)
        {
            if (!IsToken(relationship.Id))
                issues.Add(new("relationship.malformed-id", relationship.Id, "Relationship IDs must be non-empty ASCII tokens."));
            if (!knownRepositories.Contains(relationship.SourceRepository))
                issues.Add(new("relationship.unknown-source", relationship.Id, $"Unknown source repository '{relationship.SourceRepository}'."));
            if (!knownRepositories.Contains(relationship.TargetRepository))
                issues.Add(new("relationship.unknown-target", relationship.Id, $"Unknown target repository '{relationship.TargetRepository}'."));
            if (string.Equals(relationship.SourceRepository, relationship.TargetRepository, StringComparison.OrdinalIgnoreCase))
                issues.Add(new("relationship.self-edge", relationship.Id, "Self-relationships are not permitted in this initial catalog contract."));
            if (!Enum.IsDefined(relationship.Type))
                issues.Add(new("relationship.invalid-type", relationship.Id, "Relationship type is not recognized."));

            if (!knownCapabilities.TryGetValue(relationship.CapabilityId, out var definition))
            {
                issues.Add(new("capability.unknown", relationship.Id, $"Unknown capability '{relationship.CapabilityId}'."));
                continue;
            }

            if (!IsVersion(relationship.ContractVersion) ||
                !SameMajor(definition.ContractVersion, relationship.ContractVersion))
                issues.Add(new("version.incompatible", relationship.Id, "Relationship contract version is invalid or has a different major version from its capability definition."));
        }

        return issues.AsReadOnly();
    }

    private static void AddDuplicates(IEnumerable<string> values, string code, ICollection<CatalogIssue> issues)
    {
        foreach (var duplicate in values.GroupBy(v => v, StringComparer.OrdinalIgnoreCase).Where(g => g.Count() > 1))
            issues.Add(new(code, duplicate.Key, "Identity must be unique within its catalog collection."));
    }

    private static bool IsToken(string? value) =>
        !string.IsNullOrWhiteSpace(value) &&
        value.Length <= 100 &&
        value.All(c => char.IsAsciiLetterOrDigit(c) || c is '.' or '_' or '-');

    private static bool IsVersion(string? value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        var parts = value.Split('.');
        return parts.Length == 3 && parts.All(p =>
            p.Length > 0 && p.All(char.IsAsciiDigit) &&
            (p.Length == 1 || p[0] != '0'));
    }

    private static bool SameMajor(string left, string right) =>
        IsVersion(left) && IsVersion(right) &&
        string.Equals(left.Split('.')[0], right.Split('.')[0], StringComparison.Ordinal);
}
