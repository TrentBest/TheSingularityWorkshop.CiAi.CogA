using CiAi.CogA;

static RepositoryId Repo(string value) =>
    RepositoryId.TryParse(value, out var parsed) && parsed is not null
        ? parsed
        : throw new ArgumentException($"Invalid repository identity: {value}");

var catalog = new RepositoryCatalog(
    new[] { Repo("TrentBest/TheSingularityWorkshop.CiAi"), Repo("TrentBest/TheSingularityWorkshop.HeadlessAi") },
    new[] { new CapabilityDefinition("normalized-request", "1.0.0") },
    new[]
    {
        new CapabilityDeclaration(
            "TrentBest/TheSingularityWorkshop.CiAi",
            "normalized-request",
            "1.0.0",
            CapabilityRole.Provides)
    },
    new[]
    {
        new RepositoryRelationship(
            "ciai-to-headlessai-request",
            "TrentBest/TheSingularityWorkshop.CiAi",
            "TrentBest/TheSingularityWorkshop.HeadlessAi",
            RelationshipType.Requests,
            "normalized-request",
            "1.0.0",
            "explicit-host-configuration",
            "owner-approved-request")
    });

var issues = CatalogValidator.Validate(catalog);
if (issues.Count == 0)
{
    Console.WriteLine($"Catalog structurally valid: {catalog.Repositories.Count} repositories, {catalog.Relationships.Count} relationship.");
    Console.WriteLine("This is offline structural validation only; it does not authorize or perform a request.");
    return;
}

foreach (var issue in issues)
    Console.Error.WriteLine($"{issue.Code} [{issue.Subject}]: {issue.Message}");

Environment.ExitCode = 1;
