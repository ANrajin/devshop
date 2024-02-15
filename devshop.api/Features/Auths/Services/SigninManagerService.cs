using devshop.api.Cores.Adapters;
using devshop.api.Features.Auths.JWT;
using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;

namespace devshop.api.Features.Auths.Services;

public class SigninManagerService(
    IUserManagerAdapter userManagerAdapter,
    ISigninManagerAdapter signinManagerAdapter,
    IJwtTokenGenerator jwtTokenGenerator) 
    : ISigninManagerService
{
    public async Task<AuthResponse> AuthenticateUser(SigninRequest request, CancellationToken cancellationToken)
    {
        var token = string.Empty;
        
        var user = await userManagerAdapter.FindByUserNameAsync(request.Username)
            ?? throw new ArgumentException("The requested user not found!");

        var validUser = await signinManagerAdapter.CheckPasswordSignInAsync(user, request.Password, false)
            ?? throw new ArgumentException("The requested user not found!");

        if (validUser.Succeeded)
            token = jwtTokenGenerator.GenerateJwtToken(user.Id, user.UserName!, user.Email!);
        
        return new AuthResponse(token);
    }
}