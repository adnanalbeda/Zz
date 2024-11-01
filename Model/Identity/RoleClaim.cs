using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public class RoleClaim : IdentityRoleClaim<Id22>, IModelIdentityType
{
    public Role? Role { get; set; }
}
