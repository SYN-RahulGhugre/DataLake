using DataLake.DAL;
using System;
using WorkFlowLog.InterFace;

namespace WorkFlowLog
{
    public class Run:IRun
    {
        /// <summary>
        /// This method read input parameter and insert data in WorkflowRun Table
        /// </summary>
        /// <param name="workflowDefinitionId"></param>
        /// <param name="status"></param>
        /// <param name="startedOn"></param>
        /// <param name="completedOn"></param>
        /// <param name="hasError"></param>
        /// <param name="cutOffValue"></param>
        /// <param name="cutOffDateTime"></param>
        public void InsertWorkFlowRun(int workflowDefinitionId,int status,DateTime startedOn,DateTime? completedOn,bool hasError,string cutOffValue,DateTime? cutOffDateTime)
        {
            using (var db = new DataLakeEntities())
            {
                int insertworkflowRun = db.uspInsertWorkflowRun(workflowDefinitionId, status, startedOn, completedOn, hasError, cutOffValue, cutOffDateTime);
            }
        }

        /// <summary>
        /// This method read input parameter and update cutoffdatetime in WorkflowRun Table
        /// </summary>
        /// <param name="workflowDefinationId"></param>
        /// <param name="datetime"></param>
        public void UpdateWorkFlowRun(int workflowDefinitionId,DateTime datetime)
        {
            using (var db = new DataLakeEntities())
            {
                int insertworkflowRun = db.uspUpdateWorkflowRun(workflowDefinitionId, datetime);
            }
        }
    }
}
