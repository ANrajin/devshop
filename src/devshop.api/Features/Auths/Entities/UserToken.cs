using Microsoft.AspNetCore.Identity;

namespace devshop.api.Features.Auths.Entities;

public sealed class UserToken : IdentityUserToken<Guid>
{
}
