using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLakeSharedLibrary.Interface;
using DataLakeSharedLibrary.Publisher;

namespace DataFeedPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Publish Started");
            var restClient = new RestClient();

            if (restClient.IsSuccess) 
            {              
                dynamic getWeatherDatas = restClient.GetWeatherData();
                foreach (var getWeatherData in getWeatherDatas)
                {
                    IDataLakePublisher obj = new DataLakePublisher();
                    obj.AddMessageToTopic(getWeatherData, "Weather", "WeatherSubcription");
                }
            }
            else
            {
                Console.WriteLine("Storm API Service not working");
            }

            Console.WriteLine("Publish Finished");
            Console.ReadLine();
        }
    }
}
