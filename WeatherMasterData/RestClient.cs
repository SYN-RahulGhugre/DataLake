using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DataLake.DAL;
using DataLakeSharedLibrary.Interface;
using DataLakeSharedLibrary.Log;

namespace WeatherMasterData
{
    public class RestClient
    {

        #region Constants
        const string APIUrlConfigKey = "WeatherApiURl";
        const string DateToGetDataFromConfigKey = "startdatetime";

        #endregion
        #region constructors
        public RestClient()
        {
            _weatherApiURlApiUrl = ConfigurationManager.AppSettings[APIUrlConfigKey].ToString();
            //getAPIResponse();
            InsertMasterData();

        }
        #endregion
        #region Public Properties

        public bool IsSuccess { get; private set; }

        public HttpResponseMessage APIResponse { get; private set; }

        #endregion

        #region Private Properties

        private string _weatherApiURlApiUrl;
        private string _startDate;
        private string _endDate;
        string _url;

        #endregion
        #region Private Methods

        /// <summary>
        /// Reading config keys for storm API service
        /// </summary>


        /// <summary>
        /// Checking API is reachable and responding
        /// </summary>
        private void getAPIResponse(string url)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_weatherApiURlApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                APIResponse = client.GetAsync(url).Result;
                IsSuccess = APIResponse.IsSuccessStatusCode;
            }
        }
        private void InsertMasterData()
        {

            //client.BaseAddress = new Uri(_weatherApiURlApiUrl);
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
            //APIResponse = client.GetAsync(_url).Result;
            //IsSuccess = APIResponse.IsSuccessStatusCode;

            for (int i = 1; i <= 6; i++)
            {
                switch (i)
                {

                    case 1:
                        getAPIResponse("api/v2/datasets");
                        GetWeatheMasterData(i, "Datasets", _weatherApiURlApiUrl + "api/v2/datasets");
                        break;
                    case 2:
                        getAPIResponse("api/v2/datacategories?limit=42");
                        GetWeatheMasterData(i, "Datacategories", _weatherApiURlApiUrl + "api/v2/datacategories?limit=42");
                        break;
                    case 3:
                        getAPIResponse("api/v2/datatypes?limit=1000");
                        GetWeatheMasterData(i, "Datatypes", _weatherApiURlApiUrl + "api/v2/datatypes?limit=1000");
                        break;
                    case 4:
                        getAPIResponse("api/v2/locationcategories");
                        GetWeatheMasterData(i, "Locationcategories", _weatherApiURlApiUrl + "api/v2/locationcategories");
                        break;
                    case 5:
                        getAPIResponse("api/v2/locations?datasetid=GHCND&limit=1000");
                        GetWeatheMasterData(i, "Locations", _weatherApiURlApiUrl + "api/v2/locations?datasetid=GHCND&limit=1000");
                        break;
                    case 6:
                        getAPIResponse("api/v2/stations?datasetid=GHCND&limit=1000");
                        GetWeatheMasterData(i, "Stations", _weatherApiURlApiUrl + "api/v2/stations?datasetid=GHCND&limit=1000");
                        break;

                }
            }

        }


        public void GetWeatheMasterData(int i, string dataset, string url)
        {
            var jsonData = APIResponse.Content.ReadAsStringAsync().Result;
            IManageLog manageLog = new ManageLog();

            // TODO deserialize json into generic collection
            List<dynamic> data = new List<dynamic>();

            var finaldata = (JObject)JsonConvert.DeserializeObject(jsonData);

            switch (i)
            {
                case 1:
                    var weatherDatasetTypesResult = finaldata["results"].Select(item => new
                    WeatherDatasetTypes
                    {
                        uid = item["uid"].ToString(),
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        name = item["name"].ToString(),
                        datacoverage = item["datacoverage"].ToString(),
                        id = item["id"].ToString(),

                    }).ToList();

                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherDatasetTypesResult)
                        {
                            db.uspInsertWeatherDatasetTypesData(item.uid, item.mindate, item.maxdate, item.name, item.datacoverage, item.id);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }
                    Console.WriteLine("Datset Updated");
                    Console.ReadLine();

                    break;
                case 2:
                    var weatherDataCategoriesResult = finaldata["results"].Select(item => new
                 WeatherDataCategories
                    {
                        id = item["id"].ToString(),
                        name = item["name"].ToString(),

                    }).ToList();

                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherDataCategoriesResult)
                        {
                            db.uspInsertWeatherDataCategoriesData(item.id, item.name);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }
                    break;
                case 3:
                    var weatherDataTypesResult = finaldata["results"].Select(item => new
                    WeatherDataTypes
                    {
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        name = item["name"].ToString(),
                        datacoverage = Convert.ToDecimal(item["datacoverage"]),
                        id = item["id"].ToString(),
                    }).ToList();

                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherDataTypesResult)
                        {
                            db.uspInsertWeatherDataTypesData(item.mindate, item.maxdate, item.name, item.datacoverage, item.id);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }
                    break;
                case 4:
                    var weatherLocationCategoriesResult = finaldata["results"].Select(item => new
                   WeatherLocationCategories
                    {
                        id = item["id"].ToString(),
                        name = item["name"].ToString(),
                    }).ToList();

                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherLocationCategoriesResult)
                        {
                            db.uspInsertWeatherLocationCategoriesData(item.id, item.name);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }
                    break;
                case 5:
                    var weatherLocationsResult = finaldata["results"].Select(item => new
                 WeatherLocations
                    {
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        name = item["name"].ToString(),
                        datacoverage = Convert.ToDecimal(item["datacoverage"]),
                        id = item["id"].ToString(),
                    }).ToList();
                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherLocationsResult)
                        {
                            db.uspInsertWeatherLocationsData(item.mindate, item.maxdate, item.name, item.datacoverage, item.id);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }

                    break;
                case 6:
                    var weatherStationsResult = finaldata["results"].Select(item => new
                    WeatherStations
                    {

                        elevation = item["elevation"] != null ? Convert.ToDecimal(item["elevation"]) : 0,
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        latitude = Convert.ToDecimal(item["latitude"]),
                        name = item["name"].ToString(),
                        datacoverage = Convert.ToDecimal(item["datacoverage"]),
                        id = item["id"].ToString(),
                        elevationUnit = Convert.ToString(item["elevationUnit"]),
                        longitude = Convert.ToDecimal(item["longitude"]),
                    }).ToList();

                    using (var db = new DataLakeEntities())
                    {
                        foreach (var item in weatherStationsResult)
                        {
                            db.uspInsertWeatherStationsData(item.elevation, item.mindate, item.maxdate, item.latitude, item.name, item.datacoverage, item.id, item.elevationUnit, item.longitude);
                        }

                        LogData logdata = new LogData();
                        logdata.DatasetName = dataset;
                        logdata.DatasetURL = url;

                        manageLog.AddLog(logdata, true, "Ok");
                    }
                    break;
            }



            //dynamic d = JObject.Parse(jsonData);
            //  WeatherDatasets account = JsonConvert.DeserializeObject<WeatherDatasets>(jsonData);

            //dynamic obj = finaldata["results"];
            // return obj;
            //data =  (JObject)JsonConvert.DeserializeObject <dynamic>(jsonData);
            // Iterating each element in generic collection
            // Pass each entry to queue or topic
        }
        #endregion

    }
}
