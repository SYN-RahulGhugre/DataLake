//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataLake.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class FeatureInfo
    {
        public string IdentificationCode { get; set; }
        public decimal Magnitude { get; set; }
        public string Place { get; set; }
        public long TimeInMS { get; set; }
        public long LastUpdatedInMS { get; set; }
        public System.DateTime EventDateTime { get; set; }
        public System.DateTime LastUpdatedDateTime { get; set; }
        public int TimeZoneOffset { get; set; }
        public string Detail { get; set; }
        public Nullable<int> ComputedFeltIntesity { get; set; }
        public Nullable<int> NumOfFeltReported { get; set; }
        public Nullable<int> MaxInstrumentalIntesity { get; set; }
        public Nullable<short> TsunamiFlag { get; set; }
        public Nullable<short> Significancy { get; set; }
        public string PreferredSourceNetworkId { get; set; }
        public string CommaSeparatedSourceNetworkIds { get; set; }
        public string CommaSeparatedProductTypes { get; set; }
        public Nullable<int> NumOfSeismicStations { get; set; }
        public Nullable<double> HorizontalDistance { get; set; }
        public Nullable<double> RmsTravelTime { get; set; }
        public string Title { get; set; }
        public string TypeOfSeismicEvent { get; set; }
        public string HumanReviewedStatus { get; set; }
        public string USGEventPageUrl { get; set; }
        public string AlertLevel { get; set; }
        public Nullable<int> DataReference { get; set; }
    }
}
