
using Exchange.Exchange.ExchangeValute.Data;
using System.Xml.Serialization;

namespace Exchange.ExchangeValute.Data;

[XmlRoot("ValCurs")]
public class ValCurs
{
    [XmlAttribute("Date")]
    public string Date { get; set; } = string.Empty;

    [XmlAttribute("name")]
    public string Name { get; set; } = string.Empty;
    [XmlElement(ElementName = "Valute")]
    public List<Valute> Valute { get; set; } = new List<Valute>();
}