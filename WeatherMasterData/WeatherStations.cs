using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherMasterData
{
    public class WeatherStations
    {
        public decimal elevation { get; set; }
        public Nullable<System.DateTime> mindate { get; set; }
        public Nullable<System.DateTime> maxdate { get; set; }
        public decimal latitude { get; set; }
        public string name { get; set; }
        public decimal datacoverage { get; set; }
        public string id { get; set; }
        public string elevationUnit { get; set; }
        public decimal longitude { get; set; }
    }
}
