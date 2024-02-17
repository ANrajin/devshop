using Microsoft.AspNetCore.Authorization;

namespace devshop.api.Features.Auths.Securities;

public class BooksRequirementHandler : AuthorizationHandler<BooksRequirement>
{
    protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, BooksRequirement requirement)
    {
        if (context.User.HasClaim(x => x.Type.Equals(DevshopClaims.ClaimType) && 
                                       x.Value.Equals(DevshopClaims.CanCreateBooksClaim)))
        {
            context.Succeed(requirement);
        }
        
        return Task.CompletedTask;
    }
}