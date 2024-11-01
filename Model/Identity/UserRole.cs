using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public partial class UserRole : IdentityUserRole<Id22>, IModelIdentityType
{
    public required User User { get; set; }
    public required Role Role { get; set; }
}
