namespace Zz;

public static class EnumerableHelpers
{
    /// <summary>
    /// Returns values from passed parameters as <see cref="IEnumerable{T}"/>.<br />
    /// This allows null behavior.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="values"></param>
    /// <returns><see cref="IEnumerable{T}"/> non-null values.</returns>
    public static IEnumerable<T> NewEnumerable<T>(params T[] values)
    {
        if (values is null || values.Length == 0)
            return [];

        return values;
    }

    public static IEnumerable<T> ConcatToIf<T>(
        this IEnumerable<T> values,
        params ConditionalValue<T>[] conditionalValues
    )
    {
        if (conditionalValues is null || conditionalValues.Length == 0)
            return values;

        return values.Concat(conditionalValues.Where(x => x.Condition).Select(x => x.Value));
    }

    public record ConditionalValue<T>(T Value, bool Condition);
}
