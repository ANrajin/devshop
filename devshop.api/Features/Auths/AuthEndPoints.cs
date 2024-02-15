using devshop.api.Features.Auths.Requests;

namespace devshop.api.Features.Auths;

public static class AuthEndPoints
{
    public static void MapAuthEndPoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("signin", async (SigninRequest request) =>
        {
            Results.Ok();
        }).WithTags("Auth");
        
        app.MapPost("signup", async (SignupRequest request) =>
        {
            Results.Ok();
        }).WithTags("Auth");
    }
}