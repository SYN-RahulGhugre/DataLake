using System.Runtime.Serialization;

namespace DataLakeSharedLibrary.EarthQuakeEntity
{
    public class Geometry
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        //longitude, latitude,depth
        [DataMember(Name = "coordinates")]
        public decimal[] Coordinates { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}
