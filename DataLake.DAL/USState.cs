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
    
    public partial class USState
    {
        public long OBJECTID { get; set; }
        public string STATE { get; set; }
        public string STATENAME { get; set; }
        public string FIPS { get; set; }
        public Nullable<decimal> LONGITUDE { get; set; }
        public Nullable<decimal> LATITUDE { get; set; }
        public Nullable<decimal> ShapeLeng { get; set; }
        public Nullable<decimal> ShapeArea { get; set; }
        public System.Data.Entity.Spatial.DbGeometry Geom { get; set; }
    }
}
