using DataLakeSharedLibrary.Interface;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.Subscriber
{
   public class DataLakeSubscriber : IDataLakeSubscriber
    {
        //Not in use 
      public string SubscribeMessage(string Topic, string Subcription)
        {         

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, Topic, Subcription);

            // Configure the callback options.

            OnMessageOptions options = new OnMessageOptions();
         
            options.AutoComplete = false;

            options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            string msg = string.Empty;
            Client.OnMessage((message) =>
            {
                try

                {
                    // Process message from subscription.

                    // Console.WriteLine("\n**High Messages**");
                    msg = message.GetBody<string>();
                    //Console.WriteLine("Body: " + message.GetBody<string>());

                    //Console.WriteLine("MessageID: " + message.MessageId);

                    //Console.WriteLine("Message Number: " + message.Properties["MessageNumber"]);

                    // Remove message from subscription.

                    message.Complete();                   
                }

                catch (Exception)
                {
                    // Indicates a problem, unlock message in subscription.

                    message.Abandon();

                }

            }, options);

            return msg;
        }

       
    }
}
