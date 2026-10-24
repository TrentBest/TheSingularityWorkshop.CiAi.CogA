using System.Collections.ObjectModel;

namespace TheSingularityWorkshop.CiAi.CogA;

/// <summary>A validated, immutable declaration of a versioned capability provider.</summary>
public sealed class CapabilityDescriptor
{
    public CapabilityDescriptor(
        string capabilityId,
        string capabilityVersion,
        string providerRepository,
        string implementationRevision,
        int contractVersion,
        IEnumerable<string> inputMediaTypes,
        IEnumerable<string> outputMediaTypes,
        bool enabled = true)
    {
        CapabilityId = RequireToken(capabilityId, nameof(capabilityId), allowDots: true);
        CapabilityVersion = RequireToken(capabilityVersion, nameof(capabilityVersion));
        ProviderRepository = RequireRepository(providerRepository);
        ImplementationRevision = RequireSha(implementationRevision);
        if (contractVersion < 1) throw new ArgumentOutOfRangeException(nameof(contractVersion));

        ContractVersion = contractVersion;
        InputMediaTypes = ReadMediaTypes(inputMediaTypes, nameof(inputMediaTypes));
        OutputMediaTypes = ReadMediaTypes(outputMediaTypes, nameof(outputMediaTypes));
        Enabled = enabled;
    }

    public string CapabilityId { get; }
    public string CapabilityVersion { get; }
    public string ProviderRepository { get; }
    public string ImplementationRevision { get; }
    public int ContractVersion { get; }
    public IReadOnlyList<string> InputMediaTypes { get; }
    public IReadOnlyList<string> OutputMediaTypes { get; }
    public bool Enabled { get; }

    internal string Key => $"{CapabilityId}\n{CapabilityVersion}\n{ProviderRepository}";

    private static string RequireToken(string value, string parameter, bool allowDots = false)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 128 ||
            value.Any(c => !(char.IsAsciiLetterOrDigit(c) || c is '-' or '_' || (allowDots && c == '.'))))
            throw new ArgumentException("Value must be a bounded identifier containing only ASCII letters, digits, '-', '_' and permitted dots.", parameter);
        return value;
    }

    private static string RequireRepository(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > 200)
            throw new ArgumentException("Repository must be owner/repository.", nameof(value));
        var parts = value.Split('/');
        if (parts.Length != 2 || !IsRepoPart(parts[0]) || !IsRepoPart(parts[1]))
            throw new ArgumentException("Repository must be a valid owner/repository identity.", nameof(value));
        return value;
    }

    private static bool IsRepoPart(string value) =>
        value.Length is > 0 and <= 100 &&
        value.All(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.');

    private static string RequireSha(string value)
    {
        if (value is null || value.Length != 40 || !value.All(Uri.IsHexDigit))
            throw new ArgumentException("Implementation revision must be a full 40-character hexadecimal commit SHA.", nameof(value));
        return value.ToLowerInvariant();
    }

    private static IReadOnlyList<string> ReadMediaTypes(IEnumerable<string> values, string parameter)
    {
        ArgumentNullException.ThrowIfNull(values, parameter);
        var copy = values.ToArray();
        if (copy.Length == 0 || copy.Any(v => string.IsNullOrWhiteSpace(v) || v.Length > 128 || v.Any(char.IsControl)))
            throw new ArgumentException("At least one bounded media type is required.", parameter);
        if (copy.Distinct(StringComparer.OrdinalIgnoreCase).Count() != copy.Length)
            throw new ArgumentException("Media types must be unique.", parameter);
        return new ReadOnlyCollection<string>(copy);
    }
}

/// <summary>
/// In-memory catalog of explicitly registered capability declarations.
/// It performs no discovery network calls and grants no execution permission.
/// </summary>
public sealed class CapabilityCatalog
{
    private readonly object _gate = new();
    private readonly Dictionary<string, CapabilityDescriptor> _entries = new(StringComparer.Ordinal);

    /// <summary>Number of distinct provider/capability/version declarations currently registered.</summary>
    public int Count { get { lock (_gate) return _entries.Count; } }

    /// <summary>Registers a declaration; duplicate provider/capability/version keys are rejected.</summary>
    public void Register(CapabilityDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        lock (_gate)
        {
            if (!_entries.TryAdd(descriptor.Key, descriptor))
                throw new InvalidOperationException("The same provider, capability ID, and version are already registered.");
        }
    }

    /// <summary>Finds one exact provider/capability/version declaration.</summary>
    public bool TryFind(string capabilityId, string capabilityVersion, string providerRepository, out CapabilityDescriptor? descriptor)
    {
        var key = $"{capabilityId}\n{capabilityVersion}\n{providerRepository}";
        lock (_gate) return _entries.TryGetValue(key, out descriptor);
    }

    /// <summary>Returns a stable sorted snapshot; callers cannot mutate catalog storage through it.</summary>
    public IReadOnlyList<CapabilityDescriptor> Snapshot()
    {
        lock (_gate)
        {
            return Array.AsReadOnly(_entries.Values
                .OrderBy(x => x.CapabilityId, StringComparer.Ordinal)
                .ThenBy(x => x.CapabilityVersion, StringComparer.Ordinal)
                .ThenBy(x => x.ProviderRepository, StringComparer.Ordinal)
                .ToArray());
        }
    }

    /// <summary>Removes one exact declaration. Removal does not affect already-issued requests.</summary>
    public bool Remove(string capabilityId, string capabilityVersion, string providerRepository)
    {
        var key = $"{capabilityId}\n{capabilityVersion}\n{providerRepository}";
        lock (_gate) return _entries.Remove(key);
    }
}
