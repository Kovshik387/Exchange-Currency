namespace Exchange.Services.EmailAction.Data;

public class AccountModel
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Email { get; set; } = null!;
}
