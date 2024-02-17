using System.Security.Claims;
using devshop.api.Auths.Entities;
using Microsoft.AspNetCore.Identity;

namespace devshop.api.Cores.Adapters;

public interface IUserManagerAdapter
{
    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
    
    Task<ApplicationUser?> FindByEmailAsync(string email);

    Task<ApplicationUser?> FindByUserNameAsync(string userName);

    Task<IdentityResult> AddClaimAsync(ApplicationUser user, Claim claim);

    Task<IList<Claim>> GetClaimsAsync(ApplicationUser user);
}