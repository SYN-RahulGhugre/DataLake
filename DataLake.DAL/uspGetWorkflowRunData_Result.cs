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
    
    public partial class uspGetWorkflowRunData_Result
    {
        public string CodeShortDescription { get; set; }
        public System.DateTime Startedon { get; set; }
        public Nullable<System.DateTime> CompletedOn { get; set; }
        public bool HasErrors { get; set; }
        public Nullable<System.DateTime> CutoffDateTime { get; set; }
    }
}