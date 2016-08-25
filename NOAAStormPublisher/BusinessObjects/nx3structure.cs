using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class nx3structure
    {
        public int MAX_REFLECT { get; set; }
        public string SHAPE { get; set; }
        public int VIL { get; set; }
        public string WSR_ID { get; set; }
        public string CELL_ID { get; set; }
        public Nullable<System.DateTime> ZTIME { get; set; }
        public int AZIMUTH { get; set; }
        public int RANGE { get; set; }
    }
}
