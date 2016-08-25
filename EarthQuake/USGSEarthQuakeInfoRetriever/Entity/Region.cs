using System.Runtime.Serialization;

namespace USGSEarthQuakeInfoRetriever.Entity
{
    public class Region
    {
        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }
    }
}
