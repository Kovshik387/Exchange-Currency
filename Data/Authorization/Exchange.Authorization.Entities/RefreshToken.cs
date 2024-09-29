namespace Exchange.Authorization.Entities;

public partial class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }  
    public string Device { get; set; }
    
    public Guid? Idaccount { get; set; }
    
    public virtual Account? IdAccountNavigation { get; set; }
}