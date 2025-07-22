using System.Xml.Serialization;

namespace DBetter.Infrastructure.ApiMarketplace.Timetables.DTOs;

[XmlRoot("stations")]
public class MultipleStationData
{
    [XmlElement("station")]
    public List<Station> Stations { get; set; }
}