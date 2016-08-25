using DataLake.DAL;
using DataLakeSharedLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.Log
{
    public class ManageLog : IManageLog
    {
        public int AddLog(LogData logdata, bool isComplete, string description)
        {
            int result = 0;
            using (var db = new DataLakeEntities())
            {
                result = db.uspInsertDatalog(logdata.DatasetName, logdata.DatasetURL, DateTime.Now, DateTime.Now, isComplete, description);
            }

            return result;

        }


    }
}
