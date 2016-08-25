using DataLake.DAL;
using System;
using System.Linq;
using WorkFlowLog.InterFace;

namespace WorkFlowLog
{
    public class Log:ILog
    {
        /// <summary>
        /// This method log the data in database
        /// </summary>
        /// <param name="workflowDefinitionID">Workflow Definition ID Parameter</param>
        /// <param name="logLevel">LogLevel Parameter</param>
        /// <param name="message">Message Parameter</param>
        /// <param name="exceptionType">ExceptionType Parameter</param>
        /// <param name="exceptionJSON">ExceptionJSON Parameter</param>
        public void InsertWorkFlowLog(int workflowDefinitionID, int logLevel, string message, string exceptionType, string exceptionJSON)
        {
            using (var db = new DataLakeEntities())
            {
                var logWorkFlowId = db.uspGetLogParameter(workflowDefinitionID).FirstOrDefault();
                int insertWorkflowLog = db.uspInsertWorkflowLog(logWorkFlowId.WorkFlowRunID, logWorkFlowId.WorkflowTaskID, logLevel, message, DateTime.UtcNow, exceptionType, exceptionJSON);
            }
        }
    }
}
