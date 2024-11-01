namespace Zz;

public static class AppErrorCodes
{
    public const string INVALID_DATA = nameof(INVALID_DATA);
    public const string NOT_FOUND = nameof(NOT_FOUND);

    public static class Identity
    {
        public const string USERNAME_USED = nameof(USERNAME_USED);

        public const string ACCOUNT_NOT_FOUND = nameof(ACCOUNT_NOT_FOUND);

        public const string ACCOUNT_NOT_REGISTERED = nameof(ACCOUNT_NOT_REGISTERED);
        public const string EMAIL_NOT_USED = nameof(EMAIL_NOT_USED);

        public const string LOCKED_OUT = nameof(LOCKED_OUT);
        public const string WRONG_PASSWORD = nameof(WRONG_PASSWORD);
        public const string PASSWORDS_NEW_REPEAT_NOT_EQUAL = nameof(PASSWORDS_NEW_REPEAT_NOT_EQUAL);
    }
}
