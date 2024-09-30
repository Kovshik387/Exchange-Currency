namespace Exchange.Authorization.Entities;

public partial class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Device { get; set; } = string.Empty;
    
    public Guid? Idaccount { get; set; }
    
    public virtual Account? IdAccountNavigation { get; set; }
}