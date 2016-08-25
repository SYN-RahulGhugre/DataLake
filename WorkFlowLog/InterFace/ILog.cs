using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowLog.InterFace
{
   public interface ILog
    {
        void InsertWorkFlowLog(int workflowDefinitionID, int logLevel, string message, string exceptionType, string exceptionJSON);
    }
}
