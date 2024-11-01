using Microsoft.EntityFrameworkCore;
using Zz.Model;
using Zz.Model.Identity;

namespace Zz;

public static partial class IdentityPersistenceLinqExtensions
{
    public static Task<int> ExecuteUpdateUserProfileAsync<T>(
        this IQueryable<T> query,
        UserProfile profile
    )
        where T : User =>
        query
            .Where(x => x.Id == profile.Id)
            .ExecuteUpdateAsync(e =>
                e.SetProperty(x => x.Profile, profile)
                    .SetProperty(x => x.MetaData.Track_UpdatedAt_, UtcNow)
            );

    public static Task<int> ExecuteUpdateUserSettingAsync<T>(
        this IQueryable<T> query,
        UserSettings settings
    )
        where T : User =>
        query
            .Where(x => x.Id == settings.Id)
            .ExecuteUpdateAsync(e =>
                e.SetProperty(x => x.Settings, settings)
                    .SetProperty(x => x.MetaData.Track_UpdatedAt_, UtcNow)
            );
}
