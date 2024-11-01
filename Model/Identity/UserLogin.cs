using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public class UserLogin : IdentityUserLogin<Id22>, IModelIdentityType
{
    public User? User { get; set; }
}
