namespace Exchange.Services.EmailAction.Data;

public class EmailModel
{
    public AccountModel AccountModel { get; set; } = null!;
    public List<VoluteModel> VoluteModel { get; set; } = new();
    public string Date { get; set; } = default!;
}
