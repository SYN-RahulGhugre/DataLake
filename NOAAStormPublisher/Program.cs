using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using DataLakeSharedLibrary.Interface;
using DataLakeSharedLibrary.Publisher;
using System.Threading;
using DataLakeSharedLibrary.Log;

namespace NOAAStormPublisher
{
    // To learn more about Microsoft Azure WebJobs SDK, please see http://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var host = new JobHost();
            // The following code ensures that the WebJob will be running continuously


            Console.WriteLine("Web job Start");
            while (true)
            {
                try
                {
                    IManageLog manageLog = new ManageLog();
                    var restClient = new RestClient();
                    var logdata = restClient.GetLogData();
                    Console.WriteLine(restClient.IsSuccess);
                    if (restClient.IsSuccess)
                    {
                        var getStormDatas = restClient.GetStormData();

                        //if (getStormDatas != 0)
                            if (getStormDatas != null)
                            {
                                foreach (var getStormData in getStormDatas)
                                {
                                IDataLakePublisher obj = new DataLakePublisher();
                                obj.AddMessageToTopic(getStormData, "Storm", "StormSubcription");
                            }
                                manageLog.AddLog(logdata, true, "Ok");
                            }
                            else
                            {
                                manageLog.AddLog(logdata, false, "No data found for this set of parameter");
                            }
                    }
                    else
                    {
                        Console.WriteLine("Storm API Service not working");
                    }

                }
                catch (Exception ex)
                {

                    Console.WriteLine("Exception..." + ex.InnerException + "......." + ex.Message);
                }
                Thread.Sleep(5 * 60 * 1000);
            }
            // The following code ensures that the WebJob will be running continuously

            //host.RunAndBlock();
        }

    }
}
