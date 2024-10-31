namespace Exchange.Services.Storage.Data.Responses;

public class StorageResponse
{
    public bool Success { get; set; }
    public string Url { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
}
