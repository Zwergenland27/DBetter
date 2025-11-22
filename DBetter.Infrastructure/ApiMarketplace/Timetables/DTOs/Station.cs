using System.Xml.Serialization;

namespace DBetter.Infrastructure.ApiMarketplace.Timetables.DTOs;

public class Station
{
    [XmlAttribute("ds100")]
    public required string Ds100 { get; set; }
}