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
    
    public partial class FeatureGeoLocationInfo
    {
        public int GeoServeId { get; set; }
        public Nullable<decimal> Distance { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public string CityName { get; set; }
        public string Direction { get; set; }
        public Nullable<long> Population { get; set; }
        public Nullable<long> Datareference { get; set; }
    }
}