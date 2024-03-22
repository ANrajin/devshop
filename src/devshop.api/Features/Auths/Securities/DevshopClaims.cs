namespace devshop.api.Features.Auths.Securities;

public sealed record DevshopClaims
{
    public const string ClaimType = "Permissions";
    private const string ClaimValue = "true";
    
    //BooksClaim
    public const string CanViewBooksClaim = "CanViewBooks";
    public const string CanViewBooksClaimValue = ClaimValue;

    public const string CanCreateBooksClaim = "CanCreateBooks";
    public const string CanCreateBooksClaimValue = ClaimValue;

    public const string CanUpdateBooksClaim = "CanUpdateBooks";
    public const string CanUpdateBooksClaimValue = ClaimValue;

    public const string CanDeleteBooksClaim = "CanDeleteBooks";
    public const string CanDeleteBooksClaimValue = ClaimValue;
};