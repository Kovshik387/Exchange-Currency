namespace Exchange.Services.MessageSendler.Data.DTO;

public class AccountDTO
{
    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Email { get; set; } = null!;
}
