using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure;
using Microsoft.ServiceBus.Messaging;
using DataLakeSharedLibrary.EarthQuakeEntity;
using DataLake.DAL;
using Newtonsoft.Json;
using System.Data.Entity.Core.Objects;

namespace USGSEarthQuakeSubscriber
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

            SubscriptionClient Client = SubscriptionClient.CreateFromConnectionString(connectionString, "EarthQuake", "EarthQuakeSubscriptin");   

            // Configure the callback options.
            OnMessageOptions options = new OnMessageOptions();

            options.AutoComplete = false;

            // options.AutoRenewTimeout = TimeSpan.FromMinutes(1);

            Client.OnMessage((message) =>
            {
                try
                {
                   var data = message.GetBody<string>();

                   List<Feature> features = JsonConvert.DeserializeObject<List<Feature>>(data);

                    //  List<Feature> feature = data.ToList();

                    // This code will move to another project                  
                    foreach (var feature in features)
                    {
                        using (var db = new DataLakeEntities())
                        {
                            var id = feature.PreferredSourceNetworkId + feature.IdentificationCode;
                            if (feature.Magnitude.HasValue)
                                feature.Magnitude = feature.Magnitude;
                            else
                                feature.Magnitude= -9999;


                            int insertFeatureInfo = db.uspinsertfeatureinfo(id, feature.Magnitude, feature.Place, feature.Time, feature.LastUpdated, feature.EventDateTime, feature.LastUpdatedDateTime, feature.TimeZoneOffset, feature.Detail,Convert.ToInt32(feature.ComputedFeltIntesity), feature.NumOfFeltReported, Convert.ToInt32(feature.MaxInstrumentalIntesity), feature.AlertLevel, feature.TsunamiFlag,Convert.ToInt16(feature.Significancy), feature.PreferredSourceNetworkId, feature.CommaSeparatedSourceNetworkIds, feature.CommaSeparatedProductTypes, feature.NumOfSeismicStations, feature.HorizontalDistance, feature.RmsTravelTime, feature.Title, feature.TypeOfSeismicEvent, feature.HumanReviewedStatus, feature.USGEventPageUrl);

                            if(feature.Geometry!=null)
                            { 

                            int insertFeatureGeometryInfo = db.uspinsertfeaturegeometryinfo(id, feature.Geometry.Coordinates[0], feature.Geometry.Coordinates[1], feature.Geometry.Coordinates[2], feature.Geometry.Type);
                            }

                            foreach(var geoServes in feature.GeoServes)
                            {
                                if(geoServes.Region.Country==null)
                                {
                                    geoServes.Region.Country = string.Empty;
                                }
                                if (geoServes.Region.State == null)
                                {
                                    geoServes.Region.State = string.Empty;
                                }

                                ObjectParameter oId = new ObjectParameter("OId", typeof(int));

                                var geoservesId = db.GetmaxGeoServeId().FirstOrDefault();

                             int outId= db.uspinsertFeatureGeoServeInfo(geoservesId,id, geoServes.Region.Country, geoServes.Region.State, geoServes.Id,oId);

                                foreach (var city in geoServes.Cities)
                                {
                                    var locgeoservesId = db.GetmaxGeoServeIdlOCATION().FirstOrDefault();
                                    db.uspinsertFeatureGeoLocationInfo(locgeoservesId, city.Distance, city.Latitude, city.Longitude, city.Name, city.Direction, city.Population);
                                }
                            }

                            
                        }
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
