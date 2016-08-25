using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;

namespace WorkFlowStreamAnalytics
{
    public class EventStreamPublisher
    {
        EventHubClient eventHubClient;
        string eventHubConnectionString;
        string eventHubName;
        public EventStreamPublisher()
        {
            eventHubConnectionString = ConfigurationManager.AppSettings["EventHubConnectionString"];
            eventHubName = ConfigurationManager.AppSettings["EventHubName"];
            eventHubClient = EventHubClient.CreateFromConnectionString(eventHubConnectionString, eventHubName);
        }

        /// <summary>
        /// Publishes received message data to Event Hub
        /// </summary>
        /// <param name="eventData">Message payload</param>
        public void PublishMessages(dynamic eventData)
        {
            try
            {
                eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(Convert.ToString(eventData))));
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error processing EventData - {0}", ex.Message);
            }
        }
    }
}
