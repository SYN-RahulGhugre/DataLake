using System;
using System.Diagnostics.Tracing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Configuration;
using System.Timers;
using System.Globalization;
using USGSEarthQuakeInfoRetriever.Entity;
using System.Diagnostics;
//Need to format all Json
//Properties can be null sometimes
namespace USGSEarthQuakeInfoRetriever
{
    class Program
    {
        public static DateTime StartDateTime = DateTime.UtcNow;
        public static DateTime EndDateTime = DateTime.UtcNow;
        public static int IntervalValue = 0;
        public static int PastDataIntervalValue = 0;
        public static Timer t;
        public static bool IsTimerEnabled;

        static void Main(string[] args)
        {
            // DateTime fromDt = new DateTime(2016, 07, 12, 19, 0, 0);
            Trace.TraceInformation("Application started..");
            GetSettings();
            try
            {

                InitDatesAndLoadData();
                //LoadData(StartDateTime, StartDateTime.AddMinutes(IntervalValue));

            }
            catch (Exception ex)
            {
                Trace.TraceError("Load Data :" + ex.Message);
            }
            finally
            {
                StartDateTime = EndDateTime;
            }

            if (IntervalValue != 0 && IsTimerEnabled)
            {
                t = new Timer((IntervalValue * 60 * 1000));
                t.Elapsed += T_Elapsed;
                StartTimer();
            }

            while (Console.Read() != 'q')
            {

            }

            Trace.TraceInformation("Application ended..");
        }



        static void GetSettings()
        {
            GetLastDateTime();

            var intervalVal = ConfigurationManager.AppSettings["interval"];

            if (intervalVal == null || !int.TryParse(intervalVal, out IntervalValue))
            {
                IntervalValue = 0;
            }


            var enableTimer = ConfigurationManager.AppSettings["enableTimer"];

            if (enableTimer == null || !bool.TryParse(enableTimer, out IsTimerEnabled))
            {
                IsTimerEnabled = false;
            }

            var getPastDataInterval = ConfigurationManager.AppSettings["getPastDataInterval"];

            if (getPastDataInterval == null || !int.TryParse(getPastDataInterval, out PastDataIntervalValue))
            {
                PastDataIntervalValue = 0;
            }

            Trace.TraceInformation(string.Format("Configuration values start date : {0}, Timer enable : {1}, Interval : {2}, Past data interval : {3}", StartDateTime, IsTimerEnabled, IntervalValue, PastDataIntervalValue));

        }

        private static void T_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                t.Stop();

                InitDatesAndLoadData();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Timer Load Data : " + ex.Message);
            }
            finally
            {
                //threading can be added
                StartTimer();
            }
        }

        static void InitDatesAndLoadData()
        {
            if (PastDataIntervalValue > 0)
            {
                if (PastDataIntervalValue > 30)
                    PastDataIntervalValue = 30;
                //To fetch all data of past                
                EndDateTime = StartDateTime.AddDays(PastDataIntervalValue);
                if (Math.Abs(DateTime.UtcNow.Subtract(StartDateTime).Minutes) < (IntervalValue + 5))
                    EndDateTime = StartDateTime.AddMinutes(IntervalValue);

                //to get bulk data otherwise go with interval
                if (EndDateTime > DateTime.UtcNow)
                {
                    // This is the link when it gets updated
                    //https://www2.usgs.gov/faq/categories/9826/3451
                    EndDateTime = DateTime.UtcNow.AddMinutes(-3);
                }

                LoadData(StartDateTime, EndDateTime);
            }
        }
        static void StartTimer()
        {
            StartDateTime = EndDateTime;
            if (DateTime.UtcNow > StartDateTime.AddMinutes(IntervalValue))
            {
                t.Interval = 10000;
            }
            else
            {
                t.Interval = (IntervalValue * 60 * 1000);
            }
            t.Start();
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
        static void LoadData(DateTime dt1, DateTime dt2)
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

            InsertData(events, fromDt, toDt, url);
        }


        static void InsertData(List<Feature> features, DateTime from, DateTime to, string url)
        {
            Trace.TraceInformation("Data insertion into the database started ");
            bool isComplete = false;
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["EarthquakeDBCon"].ConnectionString))
            {
                con.Open();
                var tran = con.BeginTransaction();
                try
                {
                    var cmd = con.CreateCommand();

                    //insert features

                    foreach (var f in features)
                    {
                        cmd = con.CreateCommand();
                        cmd.Transaction = tran;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "insert_featureinfo";
                        var id = f.PreferredSourceNetworkId + f.IdentificationCode;
                        cmd.Parameters.AddWithValue("@IdentificationCode", id);

                        if (f.Magnitude.HasValue)
                            cmd.Parameters.AddWithValue("@Magnitude", f.Magnitude);
                        else
                            cmd.Parameters.AddWithValue("@Magnitude", -9999);

                        cmd.Parameters.AddWithValue("@Place", f.Place);
                        cmd.Parameters.AddWithValue("@TimeInMS", f.Time);
                        cmd.Parameters.AddWithValue("@LastUpdatedInMS", f.LastUpdated);
                        cmd.Parameters.AddWithValue("@EventDateTime", f.EventDateTime);
                        cmd.Parameters.AddWithValue("@LastUpdatedDateTime", f.LastUpdatedDateTime);
                        cmd.Parameters.AddWithValue("@TimeZoneOffset", f.TimeZoneOffset);
                        cmd.Parameters.AddWithValue("@Detail", f.Detail);

                        if (f.ComputedFeltIntesity.HasValue)
                            cmd.Parameters.AddWithValue("@ComputedFeltIntesity", f.ComputedFeltIntesity.Value);
                        else
                            cmd.Parameters.AddWithValue("@ComputedFeltIntesity", DBNull.Value);

                        if (f.NumOfFeltReported.HasValue)
                            cmd.Parameters.AddWithValue("@NumOfFeltReported", f.NumOfFeltReported.Value);
                        else
                            cmd.Parameters.AddWithValue("@NumOfFeltReported", DBNull.Value);

                        if (f.MaxInstrumentalIntesity.HasValue)
                            cmd.Parameters.AddWithValue("@MaxInstrumentalIntesity", f.MaxInstrumentalIntesity.Value);
                        else
                            cmd.Parameters.AddWithValue("@MaxInstrumentalIntesity", DBNull.Value);

                        if (f.AlertLevel != null)
                            cmd.Parameters.AddWithValue("@AlertLevel", f.AlertLevel);
                        else
                            cmd.Parameters.AddWithValue("@AlertLevel", DBNull.Value);

                        if (f.TsunamiFlag.HasValue)
                            cmd.Parameters.AddWithValue("@TsunamiFlag", f.TsunamiFlag.Value);
                        else
                            cmd.Parameters.AddWithValue("@TsunamiFlag", DBNull.Value);

                        if (f.Significancy.HasValue)
                            cmd.Parameters.AddWithValue("@Significancy", f.Significancy.Value);
                        else
                            cmd.Parameters.AddWithValue("@Significancy", DBNull.Value);

                        cmd.Parameters.AddWithValue("@PreferredSourceNetworkId", f.PreferredSourceNetworkId);
                        cmd.Parameters.AddWithValue("@CommaSeparatedSourceNetworkIds", f.CommaSeparatedSourceNetworkIds);
                        cmd.Parameters.AddWithValue("@CommaSeparatedProductTypes", f.CommaSeparatedProductTypes);

                        if (f.NumOfSeismicStations.HasValue)
                            cmd.Parameters.AddWithValue("@NumOfSeismicStations", f.NumOfSeismicStations.Value);
                        else
                            cmd.Parameters.AddWithValue("@NumOfSeismicStations", DBNull.Value);

                        if (f.HorizontalDistance.HasValue)
                            cmd.Parameters.AddWithValue("@HorizontalDistance", f.HorizontalDistance.Value);
                        else
                            cmd.Parameters.AddWithValue("@HorizontalDistance", DBNull.Value);

                        if (f.RmsTravelTime.HasValue)
                            cmd.Parameters.AddWithValue("@RmsTravelTime", f.RmsTravelTime.Value);
                        else
                            cmd.Parameters.AddWithValue("@RmsTravelTime", DBNull.Value);

                        cmd.Parameters.AddWithValue("@Title", f.Title);
                        cmd.Parameters.AddWithValue("@TypeOfSeismicEvent", f.TypeOfSeismicEvent);
                        cmd.Parameters.AddWithValue("@HumanReviewedStatus", f.HumanReviewedStatus);
                        cmd.Parameters.AddWithValue("@USGEventPageUrl", f.USGEventPageUrl);

                        SqlParameter alreadyDownloaded = new SqlParameter();
                        alreadyDownloaded.ParameterName = "@IsAlreadyDownloaded";
                        alreadyDownloaded.DbType = System.Data.DbType.Int32;
                        alreadyDownloaded.Direction = System.Data.ParameterDirection.Output;
                        cmd.Parameters.Add(alreadyDownloaded);

                        cmd.ExecuteNonQuery();

                        var alreadyDownloadedValue = (int)cmd.Parameters["@IsAlreadyDownloaded"].Value;
                        if (alreadyDownloadedValue == 1)
                        {
                            Trace.TraceInformation("Already Downloaded");
                            //tran.Rollback();
                            // isComplete = true;
                            continue;
                        }
                        cmd = con.CreateCommand();
                        if (f.Geometry != null)
                        {
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "insert_featuregeometryinfo";
                            cmd.Parameters.AddWithValue("@IdentificationCode", id);
                            cmd.Parameters.AddWithValue("@Longitude", f.Geometry.Coordinates[0]);
                            cmd.Parameters.AddWithValue("@Latitude", f.Geometry.Coordinates[1]);
                            cmd.Parameters.AddWithValue("@Depth", f.Geometry.Coordinates[2]);
                            cmd.Parameters.AddWithValue("@Type", f.Geometry.Type);
                            cmd.ExecuteNonQuery();
                        }
                        foreach (var g in f.GeoServes)
                        {
                            cmd = con.CreateCommand();
                            cmd.Transaction = tran;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "insert_FeatureGeoServeInfo";
                            cmd.Parameters.AddWithValue("@IdentificationCode", id);
                            cmd.Parameters.AddWithValue("@GeoServeCodeId", g.Id);
                            if (g.Region == null) g.Region = new Region();
                            if (g.Region.Country != null)
                                cmd.Parameters.AddWithValue("@Country", g.Region.Country);
                            else
                                cmd.Parameters.AddWithValue("@Country", DBNull.Value);

                            if (g.Region.State != null)
                                cmd.Parameters.AddWithValue("@State", g.Region.State);
                            else
                                cmd.Parameters.AddWithValue("@State", DBNull.Value);

                            SqlParameter pvNewId = new SqlParameter();
                            pvNewId.ParameterName = "@OId";
                            pvNewId.DbType = System.Data.DbType.Int32;
                            pvNewId.Direction = System.Data.ParameterDirection.Output;
                            cmd.Parameters.Add(pvNewId);
                            cmd.ExecuteNonQuery();
                            var oid = (int)cmd.Parameters["@OId"].Value;
                            foreach (var l in g.Cities)
                            {
                                cmd = con.CreateCommand();
                                cmd.Transaction = tran;
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.CommandText = "insert_FeatureGeoLocationInfo";

                                cmd.Parameters.AddWithValue("@GeoServeId", oid);
                                cmd.Parameters.AddWithValue("@Distance", l.Distance.Value);
                                cmd.Parameters.AddWithValue("@Latitude", l.Latitude.Value);
                                cmd.Parameters.AddWithValue("@Longitude", l.Longitude.Value);
                                cmd.Parameters.AddWithValue("@CityName", l.Name);
                                cmd.Parameters.AddWithValue("@Direction", l.Direction);
                                cmd.Parameters.AddWithValue("@Population", l.Population);
                                cmd.ExecuteNonQuery();
                            }

                        }

                    }
                    tran.Commit();
                    isComplete = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    if (con.State == System.Data.ConnectionState.Closed)
                        con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "insert_DataLakeLog";
                    cmd.Parameters.AddWithValue("@DatasetName", "EarthQuake");
                    cmd.Parameters.AddWithValue("@DatasetURL", url);
                    cmd.Parameters.AddWithValue("@LastRunDate", to);
                    cmd.Parameters.AddWithValue("@IsCompete", (isComplete ? 1 : 0));
                    cmd.ExecuteNonQuery();

                    if (con.State == System.Data.ConnectionState.Open)
                        con.Close();
                }

            }
            Trace.TraceInformation("Data insertion into the database finished.");
        }

        static void GetLastDateTime()
        {
            var startDateTimeVal = ConfigurationManager.AppSettings["startdatetime"];
            DateTime? lastUpdated = null;
            using (var sqlCon = new SqlConnection(ConfigurationManager.ConnectionStrings["EarthquakeDBCon"].ConnectionString))
            {
                sqlCon.Open();
                var cmd = sqlCon.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "SELECT MAX(LastRunDate) FROM DataLakeLog Where isCompete=1";
                var dtObject = cmd.ExecuteScalar();

                if (dtObject != null && dtObject != DBNull.Value)
                {
                    lastUpdated = Convert.ToDateTime(dtObject);
                }
            }
            if (lastUpdated.HasValue)
            {
                StartDateTime = lastUpdated.Value;
            }
            else if ((string.IsNullOrWhiteSpace(startDateTimeVal) || !DateTime.TryParseExact(startDateTimeVal, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture,
       DateTimeStyles.None, out StartDateTime)))
            {
                StartDateTime = DateTime.UtcNow;
            }
        }


    }



}