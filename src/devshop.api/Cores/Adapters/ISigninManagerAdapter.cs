using devshop.api.Auths.Entities;
using Microsoft.AspNetCore.Identity;

namespace devshop.api.Cores.Adapters;

public interface ISigninManagerAdapter
{
    Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure);
}