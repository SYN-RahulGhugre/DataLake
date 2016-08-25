using DataLakeSharedLibrary.Interface;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.Publisher
{
   public class DataLakePublisher: IDataLakePublisher
    {
        public string AddMessageToTopic(dynamic message, string topic, string subcription)
        {
            // Configure Topic Settings.

            TopicDescription td = new TopicDescription(topic);

            td.MaxSizeInMegabytes = 5120;

            td.DefaultMessageTimeToLive = new TimeSpan(0, 10, 0);

            // Create a new Topic with custom settings.

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);

            if (!namespaceManager.TopicExists(topic))

            {
                namespaceManager.CreateTopic(td);
            }

            string filter = "'" + subcription + "'";

            if (!namespaceManager.SubscriptionExists(topic, subcription))

            {
                //SqlFilter myFilter = new SqlFilter("color='blue'");
                namespaceManager.CreateSubscription(topic, subcription, new SqlFilter("subcription=" + filter));
            }

            TopicClient Client = TopicClient.CreateFromConnectionString(connectionString, topic);

            // TopicClient.Create(connectionString);

            var recordsMessage = Newtonsoft.Json.JsonConvert.SerializeObject(message);

            BrokeredMessage brokeredmessage = new BrokeredMessage(recordsMessage);

            brokeredmessage.Properties["subcription"] = subcription.ToString();

            // Send message to the topic.

            Client.Send(brokeredmessage);

            return "Message Added Successfully";
        }
    }
}
