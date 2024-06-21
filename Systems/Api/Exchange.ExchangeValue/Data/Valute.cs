using System.Globalization;
using System.Xml.Serialization;

namespace Exchange.Exchange.ExchangeValute.Data;

public class Valute
{
    [XmlAttribute("ID")]
    public string ID { get; set; } 

    [XmlElement("NumCode")]
    public int NumCode { get; set; }

    [XmlElement("CharCode")]
    public string CharCode { get; set; } = default!;

    [XmlElement("Nominal")]
    public int Nominal { get; set; }
    [XmlElement("Name")]
    public string Name { get; set; } = default!;
    [XmlIgnore]
    public decimal Value { get; set; } = default;

    [XmlElement("Value")]
    public string ValueString
    {
        get => Value.ToString(CultureInfo.InvariantCulture);
        set => Value = decimal.Parse(value, CultureInfo.GetCultureInfo("ru-RU"));
    }
    [XmlIgnore]
    public decimal VunitRate { get; set; } = default;

    [XmlElement("VunitRate")]
    public string VunitRateString {
        get => VunitRate.ToString(CultureInfo.InvariantCulture);
        set => VunitRate = decimal.Parse(value, CultureInfo.GetCultureInfo("ru-RU"));
    }
}
