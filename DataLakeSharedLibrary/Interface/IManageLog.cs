using DataLakeSharedLibrary.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.Interface
{
   public interface IManageLog
    {
        int AddLog(LogData logdata, bool isComplete, string description);
    }
}
