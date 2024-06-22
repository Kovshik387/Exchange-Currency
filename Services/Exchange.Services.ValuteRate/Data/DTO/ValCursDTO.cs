namespace Exchange.Services.ValutaRate.Data.DTO;

public class ValCursDTO
{
    public DateOnly Date { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public List<VoluteDTO> Valuta { get; set; } = [];
}