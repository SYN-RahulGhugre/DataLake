using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowCommon
{
   public static class LogMessage
    {
        #region Publisher Start Up Messages
        public static readonly string WorkflowStarted = "Workflow Started"; 
        public static readonly string WorkFlowGUID = "WorkFlowGUID";
        public static readonly string EarthQuake = "EarthQuake";
        public static readonly string PublishStartLogMsg= "Publish Flow Started";
        public static readonly string WorkflowFinished = "Workflow Finished";
        #endregion

        #region DataExecution Messages
        public static readonly string Token = "Token";
        public static readonly string TokenValue = "TokenValue";
        public static readonly string CallGetAPIResponseLogMsg = "Call GetAPIResponse Method";
        public static readonly string UpdateCutOffDateLogMsg = "Update CutOffDateTime";
        public static readonly string ErrorGetAPIResponseLogMsg = "Error in GetAPIResponse";
        public static readonly string CallGetJsonResponseLogMsg = "Call GetJsonResponse Method";
        public static readonly string ErrorGetJsonResponseLogMsg = "Error in GetJsonResponse";
        #endregion


        #region ReflectionHelper Messages
        public static readonly string GetMethod = "ExecuteTask";//"GetAPIResponse";
        public static readonly string Noexists = "No such method exists.";
        public static readonly string GetReflectionType = "WorkFlowCommon.TaskProcessor";//"WorkFlowDataExecution.DataExecution";
        #endregion

        #region WorkFlowSubscriber Messages
        public static readonly string SubscriberStarted = "Subscriber Started";
        public static readonly string ServiceBusConnectionString = "Microsoft.ServiceBus.ConnectionString";
        public static readonly string TopicName = "WorkFlow";
        public static readonly string SubscriptionName = "WorkFlowSubscription";
        public static readonly string OnMessageCall = "Subscriber OnMessage Call";
        public static readonly string SubscriberError = "Error in Subscriber";
   
        #endregion
    }
}
