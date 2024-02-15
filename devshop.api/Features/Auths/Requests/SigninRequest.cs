namespace devshop.api.Features.Auths.Requests;

public class SigninRequest
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}