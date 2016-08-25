using System.Runtime.Serialization;

namespace DataLakeSharedLibrary.EarthQuakeEntity
{
    public class Region
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }
    }
}
