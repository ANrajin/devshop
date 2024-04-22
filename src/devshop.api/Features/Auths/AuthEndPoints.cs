using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Services;

namespace devshop.api.Features.Auths;

public static class AuthEndPoints
{
    public static void MapAuthEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("signin", async (
            IAuthService authService,
            SigninRequest request,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var authResponse = await authService.AuthenticateUser(request, cancellationToken);

            if (string.IsNullOrEmpty(authResponse.Token))
                return Results.BadRequest();

            await authService.SetTokensInsideCookie(httpContext, authResponse);

            return Results.Ok();
        }).WithTags("Auth")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireRateLimiting("fixed");
        
        app.MapPost("signup", async (
            IAuthService authService, 
            SignupRequest request,
            HttpContext httpContext,
            CancellationToken cancellationToken) =>
        {
            var authResponse = await authService.CreateUserAsync(request, cancellationToken);

            await authService.SetTokensInsideCookie(httpContext, authResponse);

            return Results.Ok();
        }).WithTags("Auth")
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .RequireRateLimiting("fixed");
    }
}