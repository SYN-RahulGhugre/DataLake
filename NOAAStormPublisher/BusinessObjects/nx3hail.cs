using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class nx3hail
    {
        public int PROB { get; set; }
        public string SHAPE { get; set; }
        public string WSR_ID { get; set; }
        public string CELL_ID { get; set; }
        public Nullable<System.DateTime> ZTIME { get; set; }
        public int SEVPROB { get; set; }
        public double MAXSIZE { get; set; }
    }
}
