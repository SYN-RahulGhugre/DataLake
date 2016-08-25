using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataFeedSubscriber
{
   public class WeatherDatasets
    {
        public Nullable<System.DateTime> date { get; set; }
        public string datatype { get; set; }
        public string station { get; set; }
        public string attributes { get; set; }
        public int value { get; set; }
    }
}
