using DataLakeSharedLibrary.EarthQuakeEntity;
using DataLakeSharedLibrary.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NOAAEarthQuakePublisher
{
   public class RestClient
    {

        public RestClient()
        {           

        }

        public LogData GetLogData()
        {
            LogData logData = new LogData();
            logData.DatasetName = "EarthQuake";
            logData.DatasetURL = "http://earthquake.usgs.gov/fdsnws/event/1/";
            return logData;
        }


        public List<Feature> LoadData(DateTime dt1, DateTime dt2)
        {
            Trace.TraceInformation(string.Format("Data downloading started for start time {0} and end time {1}", dt1, dt2));

            DateTime fromDt = dt1;
            DateTime toDt = dt2;
            string url = string.Format(
                "http://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime={0}&endtime={1}&eventtype=earthquake", fromDt.ToString("yyyy-MM-ddTHH:mm:ss"), toDt.ToString("yyyy-MM-ddTHH:mm:ss"));


            var jsondata = RequestData(url, "Summary");



            var geoJsonSummary = (JObject)JsonConvert.DeserializeObject(jsondata);
            var count = geoJsonSummary["metadata"]["count"].ToObject<int>();
            Trace.TraceInformation(string.Format("{0} events data are downloaded", count));

            var events = new List<Feature>();
            foreach (var evnt in geoJsonSummary["features"].ToArray())
            {
                var item = (JObject)evnt["properties"];
                var feature = new Feature();

                feature.Magnitude = item["mag"].ToObject<Nullable<decimal>>();
                feature.Place = item["place"].ToString();
                feature.Time = item["time"].ToObject<long>();
                feature.LastUpdated = item["updated"].ToObject<long>();
                feature.TimeZoneOffset = item["tz"].ToObject<int>();
                feature.Detail = item["detail"].ToString();

                feature.ComputedFeltIntesity = GetTokenValue<Nullable<int>>("cdi", item);
                feature.NumOfFeltReported = GetTokenValue<Nullable<int>>("felt", item);
                feature.MaxInstrumentalIntesity = GetTokenValue<Nullable<int>>("mmi", item);
                feature.AlertLevel = GetTokenValue<string>("alert", item);
                feature.TsunamiFlag = GetTokenValue<Nullable<short>>("tsunami", item);
                feature.Significancy = GetTokenValue<Nullable<short>>("sig", item);
                feature.PreferredSourceNetworkId = GetTokenValue<string>("net", item);
                feature.IdentificationCode = GetTokenValue<string>("code", item);
                feature.CommaSeparatedSourceNetworkIds = GetTokenValue<string>("sources", item);
                feature.CommaSeparatedProductTypes = GetTokenValue<string>("types", item);
                feature.NumOfSeismicStations = GetTokenValue<Nullable<int>>("nst", item);
                feature.HorizontalDistance = GetTokenValue<Nullable<float>>("dmin", item);
                feature.RmsTravelTime = GetTokenValue<Nullable<float>>("rms", item);
                feature.MaxAzimuthalGap = GetTokenValue<Nullable<float>>("gap", item);
                feature.MagnitudeCalcAlgorithmType = GetTokenValue<string>("ml", item);
                feature.Title = GetTokenValue<string>("title", item);
                feature.TypeOfSeismicEvent = GetTokenValue<string>("type", item);
                feature.HumanReviewedStatus = GetTokenValue<string>("status", item);
                feature.USGEventPageUrl = GetTokenValue<string>("url", item);
                feature.Geometry = GetTokenValue<Geometry>("geometry", (evnt as JObject));

                events.Add(feature);
            }

            Trace.TraceInformation("Each event details are getting downloaded");
            foreach (var evnt in events)
            {
                evnt.GeoServeUrls = new List<string>();
                evnt.GeoServes = new List<GeoServe>();
                if (!string.IsNullOrWhiteSpace(evnt.Detail))
                {
                    jsondata = RequestData(evnt.Detail, "Detail");
                    var eventDetail = (JObject)JsonConvert.DeserializeObject(jsondata);
                    var geoServes = eventDetail.SelectToken("properties.products.geoserve", true).ToArray();
                    foreach (var geoServe in geoServes)
                    {
                        var contentUrl = ((JObject)geoServe["contents"])["geoserve.json"]["url"].ToString();
                        if (!string.IsNullOrWhiteSpace(contentUrl))
                        {
                            evnt.GeoServeUrls.Add(contentUrl);
                            jsondata = RequestData(contentUrl, "GeoServe");
                            var geoServeData = (JObject)JsonConvert.DeserializeObject(jsondata);
                            evnt.GeoServes.Add(
                                new GeoServe
                                {
                                    Cities = geoServeData["cities"].ToObject<List<Location>>(),
                                    Region = geoServeData["region"].ToObject<Region>(),
                                    Id = geoServe["id"].ToString()

                                }
                                );
                        }
                    }

                    /*
                     * http://earthquake.usgs.gov/archive/product/geoserve/nc72660516/us/1468283800710/geoserve.json
                     */
                }
            }

            Trace.TraceInformation(string.Format("All events details downloading are finished for start time {0} and end time {1}", dt1, dt2));

            return events;
        }

        static string RequestData(string url, string type)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                Trace.TraceInformation(string.Format("Request Type : {0}, Request : {1}, Status : {2}", type, url, response.StatusCode));
                return response.Content.ReadAsStringAsync().Result;
            }
            else if (!response.IsSuccessStatusCode)
                Trace.TraceInformation(string.Format("Request Type : {0}, Request : {1}, Status : {2}, Exception : {3}", type, url, response.StatusCode, response.ReasonPhrase + " :: " + response.Content.ReadAsStringAsync().Result));

            return null;
        }

        static TReturn GetTokenValue<TReturn>(string propName, JObject jObject)
        {
            try
            {
                JToken retVal;
                if (jObject.TryGetValue(propName, StringComparison.OrdinalIgnoreCase, out retVal))
                {
                    return retVal.ToObject<TReturn>();
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Token Evaluation Error {0} Of JObject {1}, error : {2}", propName, jObject.ToString(), ex.Message));
                throw;
            }
            return default(TReturn);

        }
    }
}
