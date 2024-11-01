namespace Zz;

public enum RemoveResultStatus
{
    /// <summary>
    /// An unknown error occurred.
    /// </summary>
    UnknownError = -5,

    /// <summary>
    /// No values specified for remove/trash.
    /// </summary>
    NotSpecified = -4,

    /// <summary>
    /// Can't purge item directly.
    /// <br/>
    /// Must be trashed first, then removed.
    /// </summary>
    CannotPurge = -3,

    /// <summary>
    /// Value(s) can't be removed as other values are depending on them.
    /// </summary>
    BlockedForDependencies = -2,

    /// <summary>
    /// Value(s) are not found. It's either removed or didn't exist.
    /// </summary>
    NotFound = -1,

    /// <summary>
    /// Value is trashed, or already in trash.
    /// </summary>
    Trashed = 0,

    /// <summary>
    /// Value(s) is removed successfully.
    /// </summary>
    Removed = 1,
}
