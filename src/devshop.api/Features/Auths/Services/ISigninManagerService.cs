using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;

namespace devshop.api.Features.Auths.Services;

public interface ISigninManagerService
{
    Task<AuthResponse> AuthenticateUser(SigninRequest request, CancellationToken cancellationToken);
}