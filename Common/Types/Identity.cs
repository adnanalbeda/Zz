using System.ComponentModel.DataAnnotations.Schema;

namespace Zz;

[ComplexType]
public record Identity(IdentityKind IdKind, string? Value) : ICommonType
{
    public bool IsUnknown() => Unknown.Equals(this);

    private static readonly Identity _unknown = new(IdentityKind.Unknown, string.Empty);
    public static Identity Unknown => _unknown;
}

public enum IdentityKind
{
    Unknown = 0,
    Id,
    Username,
    Email,
    Guid,
    Automated,
    _ = Unknown,
}
