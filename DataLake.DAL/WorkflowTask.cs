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
    
    public partial class WorkflowTask
    {
        public int WorkflowTaskID { get; set; }
        public int WorkflowDefinitionID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int StepNumber { get; set; }
        public string TaskType { get; set; }
        public bool IsDisabled { get; set; }
    }
}