using CiAi.CogA;

var passed = 0;
Run("valid catalog", ValidCatalogHasNoIssues);
Run("malformed repository identity rejected", MalformedRepositoryIdentityRejected);
Run("duplicate repository identity rejected", DuplicateRepositoryRejected);
Run("unknown capability rejected", UnknownCapabilityRejected);
Run("malformed capability ID rejected", MalformedCapabilityIdRejected);
Run("malformed capability version rejected", MalformedCapabilityVersionRejected);
Run("zero major version rejected", ZeroMajorVersionRejected);
Run("large numeric version components accepted", LargeNumericVersionComponentsAccepted);
Run("incompatible major version rejected", IncompatibleVersionRejected);
Run("unknown edge endpoint rejected", UnknownEndpointRejected);
Run("duplicate relationship ID rejected", DuplicateRelationshipRejected);
Run("malformed relationship ID rejected", MalformedRelationshipIdRejected);
Run("invalid relationship type rejected", InvalidRelationshipTypeRejected);
Run("self-edge rejected", SelfEdgeRejected);
Run("declaration for unknown repository rejected", UnknownDeclarationRepositoryRejected);
Run("invalid declaration role rejected", InvalidDeclarationRoleRejected);

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
    Check(!RepositoryId.TryParse("owner/../repo", out _), "extra path segment must be rejected");
    Check(!RepositoryId.TryParse("owner/..", out _), "dot-dot segment must be rejected");
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

void MalformedCapabilityIdRejected()
{
    var catalog = ValidCatalog(capabilities: new[] { new CapabilityDefinition("bad id", "1.0.0") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "capability.malformed-id"), "malformed capability ID issue expected");
}

void MalformedCapabilityVersionRejected()
{
    var catalog = ValidCatalog(capabilities: new[] { new CapabilityDefinition("request-envelope", "01.0.0") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "version.malformed"), "malformed capability version issue expected");
}

void ZeroMajorVersionRejected()
{
    var catalog = ValidCatalog(capabilities: new[] { new CapabilityDefinition("request-envelope", "0.9.0") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "version.malformed"), "zero major version must be rejected consistently with CogB");
}

void LargeNumericVersionComponentsAccepted()
{
    const string version = "123456789012345678901234567890.2.3";
    var catalog = ValidCatalog(
        capabilities: new[] { new CapabilityDefinition("request-envelope", version) },
        declarations: new[] { new CapabilityDeclaration("workshop/CiAi", "request-envelope", version, CapabilityRole.Provides) },
        relationships: new[] { Edge(version: version) });
    Check(CatalogValidator.Validate(catalog).Count == 0, "canonical numeric components must not be limited by Int32 parsing");
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

void MalformedRelationshipIdRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(id: "bad id") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "relationship.malformed-id"), "malformed relationship ID issue expected");
}

void InvalidRelationshipTypeRejected()
{
    var invalid = (RelationshipType)999;
    var catalog = ValidCatalog(relationships: new[]
    {
        new RepositoryRelationship("edge-1", "workshop/CiAi", "workshop/HeadlessAi",
            invalid, "request-envelope", "1.0.0", "manual", "ci-request-approved")
    });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "relationship.invalid-type"), "invalid relationship type issue expected");
}

void SelfEdgeRejected()
{
    var catalog = ValidCatalog(relationships: new[] { Edge(target: "workshop/CiAi") });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "relationship.self-edge"), "self-edge issue expected");
}

void UnknownDeclarationRepositoryRejected()
{
    var catalog = ValidCatalog(declarations: new[] { new CapabilityDeclaration("workshop/Unknown", "request-envelope", "1.0.0", CapabilityRole.Provides) });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "declaration.unknown-repository"), "unknown declaration repository issue expected");
}

void InvalidDeclarationRoleRejected()
{
    var catalog = ValidCatalog(declarations: new[]
    {
        new CapabilityDeclaration("workshop/CiAi", "request-envelope", "1.0.0", (CapabilityRole)999)
    });
    Check(CatalogValidator.Validate(catalog).Any(i => i.Code == "declaration.invalid-role"), "invalid declaration role issue expected");
}

RepositoryCatalog ValidCatalog(
    IEnumerable<RepositoryId>? repositories = null,
    IEnumerable<CapabilityDefinition>? capabilities = null,
    IEnumerable<RepositoryRelationship>? relationships = null,
    IEnumerable<CapabilityDeclaration>? declarations = null) =>
    new(
        repositories ?? new[] { Repo("workshop/CiAi"), Repo("workshop/HeadlessAi") },
        capabilities ?? new[] { new CapabilityDefinition("request-envelope", "1.0.0") },
        declarations ?? new[] { new CapabilityDeclaration("workshop/CiAi", "request-envelope", "1.0.0", CapabilityRole.Provides) },
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
