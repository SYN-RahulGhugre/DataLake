using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class plsr
    {
        public int MAGNITUDE { get; set; }
        public string SHAPE { get; set; }
        public string CITY { get; set; }
        public string SOURCE { get; set; }
        public string STATE { get; set; }
        public string ID { get; set; }
        public Nullable<System.DateTime> ZTIME { get; set; }
        public string EVENT { get; set; }
        public string COUNTY { get; set; }
    }
}
