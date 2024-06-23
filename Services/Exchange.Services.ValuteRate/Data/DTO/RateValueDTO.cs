using System.Xml.Serialization;

namespace Exchange.Services.ValutaRate.Data.DTO;

[XmlRoot("ValCurs")]
public class RateValueDTO
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    [XmlElement(ElementName = "Valute")]
    public List<VoluteDTO> Volute { get; set; } = [];
}