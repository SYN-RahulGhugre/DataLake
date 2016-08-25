using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAAStormPublisher.BusinessObjects
{
    public class nx3meso
    {
        public double TOP_HEIGHT { get; set; }
        public string WSR_ID { get; set; }
        public string CELL_ID { get; set; }
        public double HEIGHT { get; set; }
        public double BASE_HEIGHT { get; set; }
        public string CELL_TYPE { get; set; }
        public int SHEAR { get; set; }
        public string SHAPE { get; set; }
        public double AZ_DIAM { get; set; }
        public double RADIAL_DIAM { get; set; }
        public Nullable<System.DateTime> ZTIME { get; set; }
        public int AZIMUTH { get; set; }
        public int RANGE { get; set; }

    }
}
