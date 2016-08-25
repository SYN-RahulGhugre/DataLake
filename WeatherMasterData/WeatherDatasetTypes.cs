using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMasterData
{
    public class WeatherDatasetTypes
    {
        public string uid { get; set; }
        public Nullable<System.DateTime> mindate { get; set; }
        public Nullable<System.DateTime> maxdate { get; set; }
        public string name { get; set; }
        public string datacoverage { get; set; }
        public string id { get; set; }
    }
}
