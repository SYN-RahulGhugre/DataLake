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
    
    public partial class USCoastalMarineZone
    {
        public long OBJECTID { get; set; }
        public string ID { get; set; }
        public string WFO { get; set; }
        public string GL_WFO { get; set; }
        public string LocationNAME { get; set; }
        public string AJOIN0 { get; set; }
        public string AJOIN1 { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public Nullable<decimal> ShapeLength { get; set; }
        public Nullable<decimal> ShapeArea { get; set; }
        public System.Data.Entity.Spatial.DbGeometry Geom { get; set; }
    }
}
