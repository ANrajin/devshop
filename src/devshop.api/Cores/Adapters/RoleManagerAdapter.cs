using devshop.api.Features.Auths.Entities;
using Microsoft.AspNetCore.Identity;

namespace devshop.api.Cores.Adapters;

public class RoleManagerAdapter(
    IRoleStore<Role> store,
    IEnumerable<IRoleValidator<Role>> roleValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    ILogger<RoleManager<Role>> logger)
    : RoleManager<Role>(store, roleValidators, keyNormalizer, errors, logger);