using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;

namespace devshop.api.Features.Auths.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> CreateUserAsync(SignupRequest request, CancellationToken cancellationToken = default);

        Task<AuthResponse> AuthenticateUser(SigninRequest request, CancellationToken cancellationToken);

        Task SetTokensInsideCookie(HttpContext context, AuthResponse authResponse);
    }
}
