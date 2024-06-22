namespace Exchange.Services.ValutaRate.Data.DTO;

public class VoluteDTO
{
    public string ID { get; set; } = default!;
    public int NumCode { get; set; }
    public string CharCode { get; set; } = default!;
    public int Nominal { get; set; }
    public string Name { get; set; } = default!;
    public decimal Value { get; set; } = default;
    public decimal VunitRate { get; set; } = default;
}
