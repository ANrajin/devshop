using devshop.api.Cores.Adapters;
using devshop.api.Features.Auths.Entities;
using devshop.api.Features.Auths.JWT;
using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;
using devshop.api.Features.Auths.Securities;
using System.Security.Claims;

namespace devshop.api.Features.Auths.Services;

public class AuthService(
    IUserManagerAdapter userManagerAdapter,
    ISigninManagerAdapter signinManagerAdapter,
    IJwtTokenGenerator jwtTokenGenerator) 
    : IAuthService
{
    public async Task<AuthResponse> CreateUserAsync(
    SignupRequest request,
    CancellationToken cancellationToken = default)
    {
        var token = string.Empty;

        var applicationUser = new ApplicationUser
        {
            UserName = request.Username,
            Email = request.Email,
            EmailConfirmed = true
        };

        var user = await userManagerAdapter.CreateUserAsync(applicationUser, request.Password);

        if (!user.Succeeded) return new AuthResponse(token);

        var claim = new Claim(DevshopClaims.CanViewBooksClaim, DevshopClaims.CanViewBooksClaimValue);

        await userManagerAdapter.AddClaimAsync(applicationUser, claim);

        token = jwtTokenGenerator.GenerateJwtToken(applicationUser.Id,
            applicationUser.UserName,
            applicationUser.Email,
            [claim]);

        return new AuthResponse(token);
    }

    public async Task<AuthResponse> AuthenticateUser(SigninRequest request, CancellationToken cancellationToken)
    {
        var token = string.Empty;
        
        var user = await userManagerAdapter.FindByUserNameAsync(request.Username)
            ?? throw new ArgumentException("The requested user not found!");

        var validUser = await signinManagerAdapter.CheckPasswordSignInAsync(user, request.Password, false)
            ?? throw new ArgumentException("The requested user not found!");

        if (!validUser.Succeeded)
            return new AuthResponse(token);

        var claims = await userManagerAdapter.GetClaimsAsync(user);
        
        token = jwtTokenGenerator.GenerateJwtToken(user.Id, user.UserName!, user.Email!, claims);
        
        return new AuthResponse(token);
    }

    public async Task SetTokensInsideCookie(HttpContext context, AuthResponse authResponse)
    {
        context.Response.Cookies.Append("accessToken", authResponse.Token, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(5),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });

        await Task.CompletedTask;
    }
}