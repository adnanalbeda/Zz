namespace Zz;

using System;

public static class DateTimeHelpers
{
    public static DateTime UtcNow => DateTime.UtcNow;

    public static string ToIsoString(this DateTime dateTime) =>
        dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

    public static string ToIsoString(this DateTimeOffset dateTime) =>
        dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

    public static string ToNumberId(this DateTime dateTime) => dateTime.Ticks.ToString();

    public static string ToShortId(
        this DateTime dateTime,
        ShortIdEndWith part = ShortIdEndWith._
    ) =>
        dateTime
            .ToUniversalTime()
            .ToString(
                part switch
                {
                    ShortIdEndWith.Month => "yyMM",
                    ShortIdEndWith.Day => "yyMMdd",
                    ShortIdEndWith.Hours => "yyMMddHH",
                    ShortIdEndWith.Minutes => "yyMMddHHmm",
                    ShortIdEndWith.Millisecond => "yyMMddHHmmssfff",
                    _ => "yyMMddHHmmss",
                }
            );

    public static string ToRandomId(this DateTime dateTime) =>
        string.Concat(dateTime.ToShortId(), Random4().ToString("0000"));

    public static string ToSecureRandomId(this DateTime dateTime) =>
        string.Concat(
            dateTime.ToShortId(),
            NextRandom(10_000_000, 100_000_000).ToString("00000000")
        );

    public enum ShortIdEndWith
    {
        _,
        Millisecond,
        Seconds,
        Minutes,
        Hours,
        Day,
        Month,
    }
}
