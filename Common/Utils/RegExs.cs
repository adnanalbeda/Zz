using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Zz;

public static class RegExs
{
    [StringSyntax("Regex")]
    public const string CurrencyRegexPattern = "^([a-zA-Z]{3})$";
    private static readonly Regex _CurrencyRegex = new(CurrencyRegexPattern);
    public static Regex CurrencyRegex => _CurrencyRegex;
    public const string CurrencyFormatErrorCode = "NOT_CURRENCY_FORMAT";
    public const string CurrencyFormatErrorMessage =
        "Provided value does not match currency format.";
    public const string CurrencyFormatDetailedErrorMessage =
        "Provided value does not match [ISO4217](https://www.iso.org/iso-4217-currency-codes.html) code format for currency. Check [Iban](https://www.iban.com/currency-codes) or [Wikipedia](https://en.wikipedia.org/wiki/ISO_4217).";

    [StringSyntax("Regex")]
    public const string EmailRegexPattern = "^([^@])+@([^@])+$";
    private static readonly Regex _EmailRegex = new(EmailRegexPattern);
    public static Regex EmailRegex => _EmailRegex;
    public const string EmailFormatErrorCode = "NOT_EMAIL_FORMAT";
    public const string EmailFormatErrorMessage =
        "Provided email value doesn't look like an email. It must be ((name)@(domain)).";

    /*
    ^
    (?:
        (?=.*[a-z])       # 1. there is a lower-case letter ahead,
        (?:               #    and
            (?=.*[A-Z])   #     1.a.i) there is also an upper-case letter, and
            (?=.*[\d\W])  #     1.a.ii) a number (\d) or symbol (\W),
        |                 #    or
            (?=.*\W)      #     1.b.i) there is a symbol, and
            (?=.*\d)      #     1.b.ii) a number ahead
        )
    |                     # OR
        (?=.*\W)          # 2.a) there is a symbol, and
        (?=.*[A-Z])       # 2.b) an upper-case letter, and
        (?=.*\d)          # 2.c) a number ahead.
    )
    .{8,}                 # the password must be at least 8 characters long.
    $
    */
    [StringSyntax("Regex")]
    public const string PasswordRegexPattern =
        @"^(?:(?=.*[a-z])(?:(?=.*[A-Z])(?=.*[\d\W])|(?=.*\W)(?=.*\d))|(?=.*\W)(?=.*[A-Z])(?=.*\d)).{8,}$";
    private static readonly Regex _PasswordRegex = new(PasswordRegexPattern);
    public static Regex PasswordRegex => _PasswordRegex;
    public const string PasswordFormatErrorCode = "NOT_COMPLEX_PASSWORD_FORMAT";
    public const string PasswordFormatErrorMessage =
        "Provided password value is not complex enough.";
}
