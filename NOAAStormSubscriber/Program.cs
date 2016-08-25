using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using DataLake.DAL;
using System.Data.Entity.Spatial;

namespace NOAAStormSubscriber
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

            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, "Storm", "StormSubcription");

            // Configure the callback options.
            OnMessageOptions options = new OnMessageOptions();

            options.AutoComplete = false;

            // options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            Client.OnMessage((message) =>
            {
                try
                {
                    var data = message.GetBody<string>();

                    WarnDataset warnDataset = JsonConvert.DeserializeObject<WarnDataset>(data);

                    System.Data.Entity.Spatial.DbGeography shape = DbGeography.PolygonFromText(warnDataset.SHAPE.ToString(), 4326);


                    //System.Data.Entity.Spatial.DbGeography shape= DbGeography.PolygonFromText("POLYGON ((-95.08 38.04, -95.08 37.86, -94.69 37.87, -94.66 38.03, -95.08 38.04))", 4326);

                    // This code will move to another project                  

                    using (var db = new DataLakeEntities())
                    {
                       int insertWeatherDatasets = db.uspInsertWarningData(warnDataset.WARNINGTYPE,warnDataset.MESSAGEID, shape, warnDataset.ZTIME_END,warnDataset.ZTIME_START,warnDataset.ID,warnDataset.ISSUEWFO);

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

            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
