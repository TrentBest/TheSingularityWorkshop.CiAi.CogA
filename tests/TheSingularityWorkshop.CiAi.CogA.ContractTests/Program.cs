using TheSingularityWorkshop.CiAi.CogA;

var passed = 0;
var catalog = new CapabilityCatalog();
var descriptor = NewDescriptor();

Check("register and exact lookup preserve immutable identity", () =>
{
    catalog.Register(descriptor);
    Assert(catalog.TryFind("uml.generate", "1.0.0", "TrentBest/Gui", out var found), "exact declaration was not found");
    Assert(ReferenceEquals(descriptor, found), "catalog returned a different descriptor");
});
Check("duplicate provider capability version is rejected", () =>
{
    Throws<InvalidOperationException>(() => catalog.Register(NewDescriptor()));
});
Check("different provider can declare the same capability version", () =>
{
    catalog.Register(NewDescriptor(provider: "TrentBest/OtherProvider"));
    Assert(catalog.Count == 2, "different provider declaration was not retained");
});
Check("snapshots are sorted and read-only", () =>
{
    var snapshot = catalog.Snapshot();
    Assert(snapshot.Count == 2 && snapshot[0].ProviderRepository == "TrentBest/Gui", "snapshot ordering was not stable");
    Assert(snapshot is not List<CapabilityDescriptor>, "snapshot exposed mutable list storage");
});
Check("malformed repository and non-pinned revision are rejected", () =>
{
    Throws<ArgumentException>(() => NewDescriptor(provider: "https://github.com/TrentBest/Gui"));
    Throws<ArgumentException>(() => NewDescriptor(revision: "development"));
});
Check("invalid contract version and duplicate media types are rejected", () =>
{
    Throws<ArgumentOutOfRangeException>(() => new CapabilityDescriptor("uml.generate", "1.0.0", "TrentBest/Gui", new string('a', 40), 0, ["text/plain"], ["image/svg+xml"]));
    Throws<ArgumentException>(() => new CapabilityDescriptor("uml.generate", "1.0.0", "TrentBest/Gui", new string('a', 40), 1, ["text/plain", "text/plain"], ["image/svg+xml"]));
});
Check("remove deletes only the exact declaration", () =>
{
    Assert(catalog.Remove("uml.generate", "1.0.0", "TrentBest/Gui"), "expected declaration was not removed");
    Assert(!catalog.TryFind("uml.generate", "1.0.0", "TrentBest/Gui", out _), "removed declaration remains discoverable");
    Assert(catalog.TryFind("uml.generate", "1.0.0", "TrentBest/OtherProvider", out _), "removal affected a different provider");
});

Console.WriteLine($"PASS: {passed} CogA contract checks.");

void Check(string name, Action action)
{
    action();
    passed++;
    Console.WriteLine($"PASS {passed}: {name}");
}

static CapabilityDescriptor NewDescriptor(string provider = "TrentBest/Gui", string revision = "abcdef0123456789abcdef0123456789abcdef01") =>
    new("uml.generate", "1.0.0", provider, revision, 1, ["text/plain"], ["image/svg+xml", "text/markdown"]);

static void Assert(bool condition, string message)
{
    if (!condition) throw new InvalidOperationException(message);
}

static void Throws<TException>(Action action) where TException : Exception
{
    try { action(); }
    catch (TException) { return; }
    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
