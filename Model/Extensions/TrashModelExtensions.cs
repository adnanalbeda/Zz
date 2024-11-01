using Zz.Model;

namespace Zz;

public static class TrashModelExtensions
{
    public static T SendToTrash<T>(this T trash, string? reason = null, Identity? by = null)
        where T : ITrash<T>
    {
        trash.SendToTrash_(reason, by);
        return trash;
    }

    public static T RestoreFromTrash<T>(this T trash, string? reason, Identity? by = null)
        where T : ITrash<T>
    {
        trash.RestoreFromTrash_(reason, by);
        return trash;
    }

    public static T RestoreFromTrash<T>(this T trash)
        where T : ITrash<T>
    {
        trash.RestoreFromTrash_();
        return trash;
    }
}
