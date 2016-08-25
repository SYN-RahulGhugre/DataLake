using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class ALLDataBAL
    {
        public void AllDataInsert(List<ALLData> data )
        {
            ALLDataDAL datadal = new ALLDataDAL();
            datadal.InsertAllData(data);  
        }
    }


}
