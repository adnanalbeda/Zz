namespace Zz.DataBase;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

public partial class EFConverter
{
    public sealed class DateTimeAsUtcValueConverter()
        : ValueConverter<DateTime, DateTime>(v => v.ToUniversalTime(), v => v.ToUniversalTime());

    public sealed class NullableDateTimeAsUtcValueConverter()
        : ValueConverter<DateTime?, DateTime?>(
            v => v.HasValue ? v.Value.ToUniversalTime() : v,
            v => v.HasValue ? v.Value.ToUniversalTime() : v
        );
}
