using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Services;

namespace devshop.api.Features.Auths;

public static class AuthEndPoints
{
    public static void MapAuthEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("signin", async (
            ISigninManagerService signinManagerService, 
            SigninRequest request,
            CancellationToken cancellationToken) =>
        {
            var token = await signinManagerService.AuthenticateUser(request, cancellationToken);
            
            return Results.Ok(token);
        }).WithTags("Auth");
        
        app.MapPost("signup", async (
            IUserManagerService userManagerService, 
            SignupRequest request,
            CancellationToken cancellationToken) =>
        {
            var token = await userManagerService.CreateUserAsync(request, cancellationToken);
            
            return Results.Ok(token);
        }).WithTags("Auth");
    }
}