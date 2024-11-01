using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public class UserClaim : IdentityUserClaim<Id22>, IModelIdentityType
{
    public User? User { get; set; }
}
