using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using WorkFlowCommon.Interface;

namespace ServiceBusExecution
{
    public class Publisher : ServiceBusBase
    {
        /// <summary>
        /// This method create Topic 
        /// </summary>
        public void CreateTopic()
        {
            try
            {
                var topicDesc = new TopicDescription(TopicName)
                {
                    MaxSizeInMegabytes = 5120,
                    DefaultMessageTimeToLive = new TimeSpan(0, 20, 0)
                };

                var namespaceManager = NamespaceManager.CreateFromConnectionString(ConnectionString);

                if (!namespaceManager.TopicExists(TopicName))
                {
                    namespaceManager.CreateTopic(topicDesc);
                }        

                if (!namespaceManager.SubscriptionExists(TopicName, SubscriptionName))

                {                  
                    namespaceManager.CreateSubscription(TopicName, SubscriptionName);
                }
            }
            catch (Exception)
            {              

            }

        }

        /// <summary>
        /// This method read data from message and send to subscription
        /// </summary>
        /// <param name="message">Input parameter IMessage</param>
        //public void SendMessage(IMessage message)
        public void SendMessage(object message)
        {
            try
            {
                var client = TopicClient.CreateFromConnectionString(ConnectionString, TopicName);

                BrokeredMessage brokeredmessage = new BrokeredMessage(message);           

                client.Send(brokeredmessage);
            }
            catch (Exception)
            {
              
            }
        }
    }
}
