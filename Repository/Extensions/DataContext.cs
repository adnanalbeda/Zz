using Microsoft.EntityFrameworkCore;
using static Zz.DataBase.EFConverter;

namespace Zz;

public static class DataContextCommonConfigsExtensions
{
    public static ModelConfigurationBuilder UseZzCommonConverters(
        this ModelConfigurationBuilder configurationBuilder
    )
    {
        configurationBuilder.MapId22ToGuid();

        configurationBuilder.Properties<DateTime>().HaveConversion<DateTimeAsUtcValueConverter>();
        configurationBuilder
            .Properties<DateTime?>()
            .HaveConversion<NullableDateTimeAsUtcValueConverter>();

        return configurationBuilder;
    }
}
