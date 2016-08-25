using DataLake.DAL;
using ServiceBusExecution;
using System;
using System.Configuration;
using System.Linq;
using WorkFlowCommon;
using WorkFlowLog;
using WorkFlowLog.InterFace;

namespace WorkFlowPublisher
{
    class Program
    {
        /// <summary>
        /// This is publisher start up  point we create topic and publish IMessage in the subscription and log data 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            DateTime startedOn;
            try
            {
                var publisher = new Publisher();
                Console.WriteLine(LogMessage.WorkflowStarted);
                string WorkFlowGUID = LogMessage.WorkFlowGUID;

                string workflowId = ConfigurationManager.AppSettings[WorkFlowGUID].ToString();

                string[] SpiltworkflowIds = workflowId.Split(',');
          
                IRun run = new Run();
                ILog log = new Log();

                foreach (var spiltworkflowId in SpiltworkflowIds)
                {                 

                    using (var db = new DataLakeEntities())
                    {
                        startedOn = DateTime.UtcNow;
                        var workflowIds = db.uspGetWorkFlowParameter(spiltworkflowId).FirstOrDefault();

                        if (workflowIds.WorkflowDefinationID != null)
                        {
                            run.InsertWorkFlowRun((Convert.ToInt32(workflowIds.WorkflowDefinationID)), 3, startedOn, null, false, null, null);

                            log.InsertWorkFlowLog(Convert.ToInt32(workflowIds.WorkflowDefinationID), 3, LogMessage.PublishStartLogMsg, null, null);

                            publisher.CreateTopic();
                            publisher.SendMessage(spiltworkflowId);

                        }
                    }
                }
                Console.WriteLine(LogMessage.WorkflowFinished);
            }
            catch (Exception)
            {

            }

            Console.ReadLine();
        }
    }
}
