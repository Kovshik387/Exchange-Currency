namespace Exchange.Services.EmailAction.Data;

public class EmailModel
{
    public AccountModel accountModel { get; set; } = null!;
    public List<VoluteModel> voluteModel { get; set; } = new();

}
