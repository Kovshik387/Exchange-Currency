namespace Exchange.Services.Authorization.Data.DTO;

public class SignInDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
}
