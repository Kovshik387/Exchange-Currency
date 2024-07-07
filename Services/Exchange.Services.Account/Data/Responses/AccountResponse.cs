namespace Exchange.Services.Account.Data.Responses;

public class AccountResponse<TData>
{
    public TData? Data { get; set; }

    public string ErrorMessage { get; set; } = string.Empty;
}
