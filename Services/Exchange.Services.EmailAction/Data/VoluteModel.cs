namespace Exchange.Services.EmailAction.Data;

public class VoluteModel
{
    public string CharCode { get; set; } = default!;
    public int Nominal { get; set; }
    public string Name { get; set; } = default!;
    public decimal Value { get; set; } = default;
}
