using Microsoft.AspNetCore.Identity;

namespace devshop.api.Features.Auths.Entities;

public sealed class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName) : base(roleName)
    {
    }
}
