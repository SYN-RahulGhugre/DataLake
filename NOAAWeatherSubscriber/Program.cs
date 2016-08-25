using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.Azure;
using Newtonsoft.Json;
using DataLake.DAL;

namespace NOAAWeatherSubscriber
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();

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


            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
