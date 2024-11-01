using Microsoft.EntityFrameworkCore;
using Zz.Model.Identity;

namespace Zz;

public static class UserSessionsExtensions
{
    public static async Task<bool> ValidateSessionAsync(
        this IQueryable<UserSession> sessions,
        Id22 sessionId,
        string encryptedSid
    )
    {
        var session = await sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(x => null == x.InvalidatedAt && x.Id == sessionId);

        if (session is null)
            return false;

        return session.ValidateSession(encryptedSid);
    }

    public static Task<int> EndSessionAsync(
        this IQueryable<UserSession> sessions,
        Id22 sessionId
    ) =>
        sessions
            .Where(x => x.Id == sessionId && null == x.InvalidatedAt)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.InvalidatedAt, UtcNow));

    public static Task<int> EndUserSessionsAsync(
        this IQueryable<UserSession> sessions,
        Id22 userId
    ) =>
        sessions
            .Where(x => x.UserId == userId && null == x.InvalidatedAt)
            .ExecuteUpdateAsync(x => x.SetProperty(x => x.InvalidatedAt, UtcNow));
}

// jwt is signed so no need for extra step to get the sid from it.
// sid (stored in LocalStorage and encrypted by secret) will be sent in the header as well.
// if sid is not valid (i.e. not from this secret), invalidate session and remove it.
