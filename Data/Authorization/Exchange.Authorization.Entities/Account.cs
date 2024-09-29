namespace Exchange.Authorization.Entities;

public partial class Account
{
    public Guid Id { get; set; }
    
    public string Email { get; set; } = null!;
    
    public string EmailNormalized { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    
    public virtual ICollection<RefreshToken> Refreshes { get; set; }
}