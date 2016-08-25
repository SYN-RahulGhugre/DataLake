using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// Not yet used classes
/// </summary>
namespace USGSEarthQuakeInfoRetriever.Entity
{
    public class EarthQuakeDetailResponseJson
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

    }
    public class EarthQuakeSummaryResponseJson
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "metadata")]
        public Metadata Metadata { get; set; }

        [DataMember(Name = "features")]
        public List<Feature> Features { get; set; }
    }

    class EarthQuakeEventMsg
    {

        [DataMember(Name = "properties")]
        public Feature Properties { get; set; }
    }

    public class Metadata
    {
        [DataMember(Name = "api")]
        public string VersionOfApi { get; set; }

        [DataMember(Name = "count")]
        public int NumberOfEarthQuakes { get; set; }

        //milliseconds
        [DataMember(Name = "generated")]
        public long MostRecentlyUpdatedTime { get; set; }

        //“USGS Magnitude 1+ Earthquakes, Past Day”, “USGS Magnitude 4.5+ Earthquakes, Past Month”
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        public string ApiUrl { get; set; }

        [DataMember(Name = "status")]
        public int HttpStatusCode { get; set; }
    }

    public class Product
    {
        //string id
    }

    public class Content
    {

    }
}
