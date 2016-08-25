using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowLog.InterFace
{
   public interface IRun
    {
        void InsertWorkFlowRun(int workflowDefinitionId, int status, DateTime startedOn, DateTime? completedOn, bool hasError, string cutOffValue, DateTime? cutOffDateTime);
        void UpdateWorkFlowRun(int workflowDefinitionId, DateTime datetime);

    }
}
