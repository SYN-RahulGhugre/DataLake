using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class DateLakeLogBAL
    {

        public void  insertDatalog(DataLakeLog datalog)
        {
            DatalakeLogDAL datalogdal = new DatalakeLogDAL();
            datalogdal.insertDataLog(datalog);
        }
    }
}
