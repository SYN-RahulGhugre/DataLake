using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAADAL;

namespace NOAABAL
{
    public class GetAllDataBAL
    {

        public DateTime getlastrundate()
        {
            DateTime lastrundate =new DateTime();
            GetAlldataDAL dt = new GetAlldataDAL();
            lastrundate =dt.GeLastRunDate();
            return lastrundate;
        }
    }


}
