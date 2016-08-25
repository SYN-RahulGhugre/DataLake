using System.Runtime.Serialization;

namespace DataLakeSharedLibrary.EarthQuakeEntity
{
    public class Location
    {
        [DataMember(Name = "distance")]
        public decimal? Distance { get; set; }

        [DataMember(Name = "latitude")]
        public decimal? Latitude { get; set; }

        [DataMember(Name = "longitude")]
        public decimal? Longitude { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "direction")]
        public string Direction { get; set; }

        [DataMember(Name = "population")]
        public long? Population { get; set; }
    }


}
