using System.Security.Claims;
using devshop.api.Features.Auths.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace devshop.api.Cores.Adapters;

public class UserManagerAdapter(
    IUserStore<ApplicationUser> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<ApplicationUser> passwordHasher,
    IEnumerable<IUserValidator<ApplicationUser>> userValidators,
    IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<ApplicationUser>> logger)
    : UserManager<ApplicationUser>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
        keyNormalizer, errors, services, logger), IUserManagerAdapter
{
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        if (user is null)
            throw new InvalidOperationException("You must provide a user");

        if (string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("You must provide a password");

        return await CreateAsync(user, password);
    }

    public async Task<ApplicationUser?> FindByUserNameAsync(string userName) => 
        await FindByNameAsync(userName);
}