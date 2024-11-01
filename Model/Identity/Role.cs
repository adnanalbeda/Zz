using Microsoft.AspNetCore.Identity;

namespace Zz.Model.Identity;

public partial class Role : IdentityRole<Id22>, IModelIdentityType
{
    public Role(Id22 id)
    {
        this.Id = Id22.ValueOrNew(id);
    }

    public IEnumerable<UserRole> Users { get; set; } = [];
    public IEnumerable<RoleClaim> Claims { get; set; } = [];
}
