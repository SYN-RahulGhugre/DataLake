using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class LocationsBAL
    {
        public void Locationsinserts(List<Locations> loc)
        {
            LocationsDAL locdal = new LocationsDAL();
            locdal.InsertLocations(loc);
        }

    }
}
