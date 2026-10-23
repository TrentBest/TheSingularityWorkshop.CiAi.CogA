using CiAi.CogA;

var passed = 0;
Run("valid catalog", ValidCatalogHasNoIssues);
Run("malformed repository identity rejected", MalformedRepositoryIdentityRejected);
Run("duplicate repository identity rejected", DuplicateRepositoryRejected);
Run("unknown capability rejected", UnknownCapabilityRejected);
Run("incompatible major version rejected", IncompatibleVersionRejected);
Run("unknown edge endpoint rejected", UnknownEndpointRejected);
Run("duplicate relationship ID rejected", DuplicateRelationshipRejected);

Console.WriteLine($"PASS: {passed} catalog validation checks.");

void Run(string name, Action test)
{
    test();
    passed++;
    Console.WriteLine($"PASS {name}");
}

void ValidCatalogHasNoIssues()
{
    var issues = CatalogValidator.Validate(ValidCatalog());
    Check(issues.Count == 0, string.Join("; ", issues.Select(i => i.Code + ": " + i.Message)));
}

void MalformedRepositoryIdentityRejected()
{
    Check(!RepositoryId.TryParse("owner/../repo", out _), "dot-dot segment must be rejected");
    Check(!RepositoryId.TryParse("owner /repo", out _), "whitespace must be rejected");
    Check(!RepositoryId.TryParse("owner/repo/extra", out _), "extra path segment must be rejected");
}

void DuplicateRepositoryRejected()
{
    var catalog = ValidCatalog(repositories: new[] { Repo("workshop/CiAi"), Repo("workshop/CiAi") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "repository.duplicate"), "duplicate repository issue expected");
}

void UnknownCapabilityRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(capability: "missing") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "capability.unknown"), "unknown capability issue expected");
}

void IncompatibleVersionRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(version: "2.0.0") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "version.incompatible"), "incompatible version issue expected");
}

void UnknownEndpointRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(target: "workshop/NotInCatalog") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "relationship.unknown-target"), "unknown target issue expected");
}

void DuplicateRelationshipRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(id: "edge-1"), Edge(id: "edge-1", target: "workshop/HeadlessAi") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "relationship.duplicate"), "duplicate relationship issue expected");
}

RepositoryCatalog ValidCatalog(
    IEnumerable<RepositoryId>? repositories = null,
    IEnumerable<RepositoryRelationship>? relationships = null) =>
    new(
        repositories ?? new[] { Repo("workshop/CiAi"), Repo("workshop/HeadlessAi") },
        new[] { new CapabilityDefinition("request-envelope", "1.0.0") },
        new[] { new CapabilityDeclaration("request-envelope", "1.0.0", CapabilityRole.Provides) },
        relationships ?? new[] { Edge() });

RepositoryRelationship Edge(
    string id = "edge-1",
    string source = "workshop/CiAi",
    string target = "workshop/HeadlessAi",
    string capability = "request-envelope",
    string version = "1.0.0") =>
    new(id, source, target, RelationshipType.Requests, capability, version, "manual", "ci-request-approved");

RepositoryId Repo(string value)
{
    if (!RepositoryId.TryParse(value, out var id) || id is null)
        throw new InvalidOperationException($"Invalid test repository identity: {value}");
    return id;
}

void Check(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}
