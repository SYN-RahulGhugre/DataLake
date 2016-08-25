using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class nx3tvs
    {
        public string CELL_TYPE { get; set; }
        public string SHAPE { get; set; }
        public int MAX_SHEAR { get; set; }
        public string WSR_ID { get; set; }
        public int MXDV { get; set; }

        public string CELL_ID { get; set; }
        public Nullable<System.DateTime> ZTIME { get; set; }

        public int AZIMUTH { get; set; }
        public int RANGE { get; set; }
    }
}
