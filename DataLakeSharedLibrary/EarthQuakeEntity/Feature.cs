using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.EarthQuakeEntity
{
    public class Feature
    {
        [DataMember(Name = "mag")]
        public decimal? Magnitude { get; set; }

        [DataMember(Name = "place")]
        public string Place { get; set; }

        [DataMember(Name = "time")]
        public long Time { get; set; }

        [DataMember(Name = "updated")]
        public long LastUpdated { get; set; }

        [DataMember(Name = "tz")]
        public int TimeZoneOffset { get; set; }

        [DataMember(Name = "url")]
        public string USGEventPageUrl { get; set; }

        [DataMember(Name = "felt")]
        public int? NumOfFeltReported { get; set; }

        // 0.0 to 10.0
        [DataMember(Name = "cdi")]
        public float? ComputedFeltIntesity { get; set; }

        // 0.0 to 10.0
        [DataMember(Name = "mmi")]
        public float? MaxInstrumentalIntesity { get; set; }

        //“green”, “yellow”, “orange”, “red”
        [DataMember(Name = "alert")]
        public string AlertLevel { get; set; }


        //“automatic”, “reviewed”, “deleted”
        [DataMember(Name = "status")]
        public string HumanReviewedStatus { get; set; }

        [DataMember(Name = "tsunami")]
        public short? TsunamiFlag { get; set; }

        [DataMember(Name = "sig")]
        public int? Significancy { get; set; }

        //ak, at, ci, hv, ld, mb, nc, nm, nn, pr, pt, se, us, uu, uw        
        [DataMember(Name = "net")]
        public string PreferredSourceNetworkId { get; set; }

        [DataMember(Name = "code")]
        public string IdentificationCode { get; set; }


        [DataMember(Name = "ids")]
        public string CommaSeparatedEventIds { get; set; }

        [DataMember(Name = "sources")]
        public string CommaSeparatedSourceNetworkIds { get; set; }

        //“,cap,dyfi,general-link,origin,p-wave-travel-times,phase-data,”
        //A comma-separated list of product types associated to this event.
        [DataMember(Name = "types")]
        public string CommaSeparatedProductTypes { get; set; }

        [DataMember(Name = "nst")]
        public int? NumOfSeismicStations { get; set; }

        //Horizontal distance from the epicenter to the nearest station (in degrees). 1 degree is approximately 111.2 kilometers. In general, the smaller this number, the more reliable is the calculated depth of the earthquake.
        //[0.4, 7.1]
        [DataMember(Name = "dmin")]
        public float? HorizontalDistance { get; set; }

        /*
         The root-mean-square (RMS) travel time residual, in sec, using all weights. 
         This parameter provides a measure of the fit of the observed arrival times to the predicted arrival times for this location. 
         Smaller numbers reflect a better fit of the data. 
         The value is dependent on the accuracy of the velocity model used to compute the earthquake location, the quality weights assigned to the arrival time data, and the procedure used to locate the earthquake.
         [0.13,1.39]
         */
        [DataMember(Name = "rms")]
        public float? RmsTravelTime { get; set; }

        /*
         The largest azimuthal gap between azimuthally adjacent stations (in degrees). 
         In general, the smaller this number, the more reliable is the calculated horizontal position of the earthquake.
         */
        [DataMember(Name = "gap")]
        public float? MaxAzimuthalGap { get; set; }

        //“Md”, “Ml”, “Ms”, “Mw”, “Me”, “Mi”, “Mb”, “MLg”
        [DataMember(Name = "magtype")]
        public string MagnitudeCalcAlgorithmType { get; set; }

        //“earthquake”, “quarry”
        [DataMember(Name = "type")]
        public string TypeOfSeismicEvent { get; set; }

        public List<Product> Products { get; set; }

        [DataMember(Name = "detail")]
        public string Detail { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        public List<string> GeoServeUrls { get; set; }
        public List<string> NearByCityUrls { get; set; }

        public List<GeoServe> GeoServes { get; set; }

        public DateTime EventDateTime
        {
            get
            {
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return dt.AddMilliseconds(Time);
            }
        }

        public DateTime LastUpdatedDateTime
        {
            get
            {
                var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                return dt.AddMilliseconds(LastUpdated);
            }
        }
    }
}
