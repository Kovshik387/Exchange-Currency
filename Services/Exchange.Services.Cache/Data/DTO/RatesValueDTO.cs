using System.Xml.Serialization;

namespace Exchange.Services.Cache.Data.DTO
{
    [XmlRoot("ValCurs")]
    public class RatesValueDTO
    {
        [XmlAttribute("ID")]
        public string ID { get; set; } = string.Empty;

        [XmlAttribute("DateRange1")]
        public string DateRange1 { get; set; } = string.Empty;

        [XmlAttribute("DateRange2")]
        public string DateRange2 { get; set; } = string.Empty;

        [XmlAttribute("name")]
        public string Name { get; set; } = string.Empty;
        [XmlElement("Record")]
        public List<RecordDTO> Records { get; set; } = new List<RecordDTO>();
    }
}
