using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public class UserToken : IdentityUserToken<Id22>, IModelIdentityType
{
    public User? User { get; set; }
}
