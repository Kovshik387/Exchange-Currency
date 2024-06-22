namespace Exchange.Entities;

public partial class RateValue
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Volute> Volutes { get; set; } = new List<Volute>();
}
