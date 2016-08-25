using Microsoft.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusExecution
{
    public abstract class ServiceBusBase
    {
        public static string TopicName = "WorkFlow";
        public static string SubscriptionName = "WorkFlowSubscription";
        protected readonly string ConnectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
    }
}
