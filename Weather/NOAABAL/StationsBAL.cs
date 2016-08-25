using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class StationsBAL
    {
        public void StationsInsert(List<Stations> stations)
        {
            try
            {
                StationsDAL stdal = new StationsDAL();
                stdal.Insertstations(stations);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
    }
}
