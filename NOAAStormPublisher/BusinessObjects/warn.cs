using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class warn
    {
        public string WARNINGTYPE { get; set; }
        public string MESSAGEID { get; set; }
        public string SHAPE { get; set; }
        public Nullable<System.DateTime> ZTIME_END { get; set; }
        public Nullable<System.DateTime> ZTIME_START { get; set; }
        public string ID { get; set; }
        public string ISSUEWFO { get; set; }
    }
}
