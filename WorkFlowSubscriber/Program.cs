using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using DataLake.DAL;
using WorkFlowLog;
using WorkFlowStreamAnalytics;
using WorkFlowReflection;
using System.Collections.Generic;
using WorkFlowLog.InterFace;
using WorkFlowCommon;

namespace WorkFlowSubscriber
{

    class Program
    {
        /// <summary>
        /// This is subscriber entry point its read IMessage from subscription and process.It will return APIResponse and call PrepareMessageForAnalytics to print data
        /// </summary>
        static void Main()
        {
            var host = new JobHost();

            ILog log = new Log();
            ReflectionHelper reflectionHelper = new ReflectionHelper();
            EventStreamPublisher eventStreamPublisher = new EventStreamPublisher();


            Console.WriteLine(LogMessage.SubscriberStarted);

            string connectionString = CloudConfigurationManager.GetSetting(LogMessage.ServiceBusConnectionString);

            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, LogMessage.TopicName, LogMessage.SubscriptionName);        

            OnMessageOptions options = new OnMessageOptions();
            options.AutoComplete = false;

            Client.OnMessage((message) =>
            {
                int workflowDefinitionID = 0;
                try
                {
                    var data = message.GetBody<string>();

                    using (var db = new DataLakeEntities())
                    {
                        DateTime startedOn = DateTime.UtcNow;
                        string workflowGuid = Convert.ToString(data);
                        var workFlowURL = db.GetWorkFlowServiceURL(workflowGuid).FirstOrDefault();
                        workflowDefinitionID = Convert.ToInt32(workFlowURL.WorkflowDefinitionID);
                        log.InsertWorkFlowLog(workflowDefinitionID, 3, LogMessage.OnMessageCall, null, null); 

                        var getURLParameter = db.GetWorkFlowURLParameter(workflowGuid).ToList();

                        IDictionary<string, object> parameters = new Dictionary<string, object>();

                     var type= getURLParameter.Where(o => o.ConfigurationKey == "Type").FirstOrDefault();
                     var method = getURLParameter.Where(o => o.ConfigurationKey == "Method").FirstOrDefault();

                        string serviceURL= GenrateURL(getURLParameter);
                     
                        parameters.Add("ServiceURL", serviceURL);
                        parameters.Add("WorkflowDefinitionID", workflowDefinitionID);
                        parameters.Add("StartedOn", startedOn);


                        dynamic getAPIResponse = reflectionHelper.InvokeMethodByReflection("", Convert.ToString(type.ConfigurationValue), Convert.ToString(method.ConfigurationValue), parameters);
                        eventStreamPublisher.PublishMessages(getAPIResponse);
                    }

                    message.Complete();
                }

                catch (Exception ex)
                {
                    var getExceptionType = ex.GetType(); 
                    log.InsertWorkFlowLog(workflowDefinitionID, 1, LogMessage.SubscriberError + "--" + ex.Message, getExceptionType.Name, null); 
                    message.Abandon();

                    Console.WriteLine(ex.Message);

                }

            }, options);

            host.RunAndBlock();
            Console.ReadLine();
        }

        private static string GenrateURL(List<GetWorkFlowURLParameter_Result> getURLParameter)
        {
           string paramurl = string.Empty;
           string baseurl = string.Empty;
            string serviceurl = string.Empty;

            foreach (var item in getURLParameter)
            {
                if (item.ConfigurationKey != "Type" && item.ConfigurationKey != "Method")
                {

                    if (item.ConfigurationKey == "ServiceURL")
                    {
                        baseurl = item.ConfigurationValue;
                    }
                    else
                    {
                        paramurl += "&" + item.ConfigurationKey + "=" + item.ConfigurationValue;
                    }
                }               
            }
            return baseurl + paramurl;
        }
       
    }
}
