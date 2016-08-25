using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using DataLakeSharedLibrary.EarthQuakeEntity;
using System.Threading;
using DataLakeSharedLibrary.Interface;
using DataLakeSharedLibrary.Log;
using DataLake.DAL;

namespace NOAAEarthQuakePublisher
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();
            while (true)
            {
                IManageLog manageLog = new ManageLog();
                RestClient restClient = new RestClient();
                var logdata = restClient.GetLogData();
                DateTime startdate = Convert.ToDateTime("2016-08-12T13:00:00");
                DateTime enddate = DateTime.Now;

                using (var db = new DataLakeEntities())
                {
                    var checklastlog = db.uspGetlastrundate("EarthQuake").FirstOrDefault();
                    DateTime datetime = new DateTime(1900, 1, 1).Add(Convert.ToDateTime(DateTime.MinValue).TimeOfDay);
                    if (checklastlog != datetime)
                    {
                        startdate =Convert.ToDateTime(checklastlog);
                    }
                }  

                List<Feature> feature = restClient.LoadData(startdate, enddate);
                EarthQuakePublisher earthQuakePublisher = new EarthQuakePublisher();

                if (feature.Count > 0)
                {
                    earthQuakePublisher.AddMessageToTopic(feature, "EarthQuake", "EarthQuakeSubscriptin");

                    manageLog.AddLog(logdata, true, "Ok");
                }
                else
                {
                    manageLog.AddLog(logdata, false, "No data found for this set of parameter");
                }

                Thread.Sleep(5 * 60 * 1000);
            }
            // The following code ensures that the WebJob will be running continuously
           // host.RunAndBlock();
        }
    }
}
