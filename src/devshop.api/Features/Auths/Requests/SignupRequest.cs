namespace devshop.api.Features.Auths.Requests;

public class SignupRequest
{
    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
    
    public string ConfirmPassword { get; set; } = string.Empty;
}