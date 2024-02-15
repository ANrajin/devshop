using devshop.api.Auths.Entities;
using devshop.api.Cores.Adapters;
using devshop.api.Features.Auths.JWT;
using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;

namespace devshop.api.Features.Auths.Services;

public class UserManagerService(
    IUserManagerAdapter userManagerAdapter,
    IJwtTokenGenerator jwtTokenGenerator)
    : IUserManagerService
{
    public async Task<AuthResponse> CreateUserAsync(
        SignupRequest request, 
        CancellationToken cancellationToken=default)
    {
        var token = string.Empty;
        
        var applicationUser = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var user = await userManagerAdapter.CreateUserAsync(applicationUser, request.Password);

        if (user.Succeeded)
        {
            token = jwtTokenGenerator.GenerateJwtToken
                (applicationUser.Id, applicationUser.UserName, applicationUser.Email);
        }

        return new AuthResponse(token);
    }
}