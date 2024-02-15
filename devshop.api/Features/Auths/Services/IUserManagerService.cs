using devshop.api.Auths.Entities;
using devshop.api.Features.Auths.Requests;
using devshop.api.Features.Auths.Responses;

namespace devshop.api.Features.Auths.Services;

public interface IUserManagerService
{
    Task<AuthResponse> CreateUserAsync(SignupRequest request, CancellationToken cancellationToken=default);
}