using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMasterData
{
    public class WeatherDataTypes
    {
        public Nullable<System.DateTime> mindate { get; set; }
        public Nullable<System.DateTime> maxdate { get; set; }
        public string name { get; set; }
        public decimal datacoverage { get; set; }
        public string id { get; set; }
    }
}
