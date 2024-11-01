using System.ComponentModel.DataAnnotations.Schema;

namespace Zz;

/// <summary>
/// Use <see cref="DFlag{T}.Value"/> as flag by its null status.<br/>
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Value"></param>
[ComplexType]
public record DFlag<T>(T? Value) : ICommonType
    where T : struct
{
    public bool Flagged() => !Equals(default(T), Value);
}
