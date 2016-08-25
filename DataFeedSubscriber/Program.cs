using DataLakeSharedLibrary.Interface;
using DataLakeSharedLibrary.Subscriber;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLake.DAL;

namespace DataFeedSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Subscriber Start");

            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");

            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, "Weather", "WeatherSubcription");

            // Configure the callback options.
            OnMessageOptions options = new OnMessageOptions();

            options.AutoComplete = false;

           // options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            Client.OnMessage((message) =>
            {
                try
                {
                    var data = message.GetBody<string>();

                    WeatherDatasets weatherDatasets = JsonConvert.DeserializeObject<WeatherDatasets>(data);

                    // This code will move to another project                  

                    using (var db = new DataLakeEntities())
                    {
                        int insertWeatherDatasets = db.uspInsertWeatherDatasetsData(weatherDatasets.date, weatherDatasets.datatype, weatherDatasets.station, weatherDatasets.attributes, weatherDatasets.value);
                    }

                    // Remove message from subscription.
                    message.Complete();
                }

                catch (Exception ex)
                {
                    // Indicates a problem, unlock message in subscription.
                    message.Abandon();
                }

            }, options);

          
         
            Console.ReadLine();
        }

    }
}



