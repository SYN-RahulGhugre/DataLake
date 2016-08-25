using DataLake.DAL;
using DataLakeSharedLibrary.Log;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NOAAWeatherPublisher
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
            getConfigValues();
            getAPIResponse();

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
        string APIResponseString;

        #endregion

        #region public method  



        public LogData GetLogData()
        {
            LogData logData = new LogData();
            logData.DatasetName = "GHCND";
            logData.DatasetURL = _url;
            return logData;
        }



        /// <summary>
        /// Fetching JSON data from API
        /// </summary>
        public dynamic GetWeatherData()
        {
            var jsonData = APIResponse.Content.ReadAsStringAsync().Result;

            // TODO deserialize json into generic collection
            List<dynamic> data = new List<dynamic>();

            var finaldata = (JObject)JsonConvert.DeserializeObject(jsonData);
            //dynamic d = JObject.Parse(jsonData);
            //  WeatherDatasets account = JsonConvert.DeserializeObject<WeatherDatasets>(jsonData);

            dynamic obj = finaldata["results"];

            return obj;
            //data =  (JObject)JsonConvert.DeserializeObject <dynamic>(jsonData);
            // Iterating each element in generic collection
            // Pass each entry to queue or topic
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Reading config keys for storm API service
        /// </summary>
        private void getConfigValues()
        {
            _weatherApiURlApiUrl = ConfigurationManager.AppSettings[APIUrlConfigKey].ToString();
            var _startDate1 = Convert.ToDateTime(ConfigurationManager.AppSettings[DateToGetDataFromConfigKey].ToString());
            //_startDate = _startDate1.ToString(("yyyyMMddhhmm"));
            //_endDate = DateTime.Now.ToString(("yyyyMMddhhmm")); 
            _startDate = _startDate1.ToString("yyyy-MM-ddTHH:mm:ss");
            _endDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            //_url =  "json/" + "warn" + "/" + _startDate  + ":" + _endDate + "";

            using (var db = new DataLakeEntities())
            {
                DateTime datetime = new DateTime(1900, 1, 1).Add(Convert.ToDateTime(DateTime.MinValue).TimeOfDay);
                var checklastlog = db.uspGetlastrundate("GHCND").FirstOrDefault();
                if (checklastlog == datetime)
                {
                    _url = "api/v2/data?datasetid=GHCND&startdate=" + _startDate + "&enddate=" + _endDate + "&limit=50";
                }
                else
                {
                    string _checklastlog = checklastlog.Value.ToString("yyyy-MM-ddTHH:mm:ss");
                    _url = "api/v2/data?datasetid=GHCND&startdate=" + _checklastlog + "&enddate=" + _endDate + "&limit=50";
                }
            }
        }

        /// <summary>
        /// Checking API is reachable and responding
        /// </summary>

        //private async Task getAPIResponse()
        private void getAPIResponse()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_weatherApiURlApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                    Console.WriteLine(_url);
                    APIResponse = client.GetAsync(_url).Result;
                    IsSuccess = APIResponse.IsSuccessStatusCode;
                    // var response = await APIResponse.Content.ReadAsStringAsync();              

                    // APIResponseString = response.ToString();
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Exception in getAPIResponse method");

                    Console.WriteLine("Exception " + ex.InnerException + "   " + ex.Message);
                }

                Console.WriteLine("Return response getAPIResponse Method");
            }

        }


        #endregion
    }
}
