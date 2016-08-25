using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Data;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using DataLake.DAL;

namespace DataFeedPublisher
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
        private string  _startDate;        
        private string _endDate;
        string _url;
        string APIResponseString;

        #endregion

        #region public method  

        /// <summary>
        /// Fetching JSON data from API
        /// </summary>
        public dynamic GetWeatherData()
        {
           // var jsonData = APIResponse.Content.ReadAsStringAsync().Result; 


            // TODO deserialize json into generic collection
            List<dynamic> data = new List<dynamic>();

             var finaldata = (JObject)JsonConvert.DeserializeObject(APIResponseString);
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
            var _startDate1 =  Convert.ToDateTime (ConfigurationManager.AppSettings[DateToGetDataFromConfigKey].ToString());
            //_startDate = _startDate1.ToString(("yyyyMMddhhmm"));
            //_endDate = DateTime.Now.ToString(("yyyyMMddhhmm")); 
            _startDate = _startDate1.ToString("yyyy-MM-ddTHH:mm:ss");
            _endDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            //_url =  "json/" + "warn" + "/" + _startDate  + ":" + _endDate + "";

            using (var db = new DataLakeEntities())
            {
                DateTime datetime=  new DateTime(1900, 1, 1).Add(Convert.ToDateTime(DateTime.MinValue).TimeOfDay);
                var checklastlog = db.uspGetlastrundate("GHCND").FirstOrDefault();
                if (checklastlog == datetime)
                {
                    _url = "api/v2/data?datasetid=GHCND&startdate=" + _startDate + "&enddate=" + _endDate + "";
                }
                else
                {
                    _url = "api/v2/data?datasetid=GHCND&startdate=" + checklastlog + "&enddate=" + _endDate + "";
                }
            }
        }

        /// <summary>
        /// Checking API is reachable and responding
        /// </summary>
        //private void getAPIResponse()
        private async Task getAPIResponse()
        {
            using (var client = new HttpClient())
            {                
                client.BaseAddress = new Uri(_weatherApiURlApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                APIResponse = client.GetAsync(_url).Result;
                IsSuccess = APIResponse.IsSuccessStatusCode;
                var response = await APIResponse.Content.ReadAsStringAsync();
                APIResponseString = response.ToString();

            }
        }
       
        
        #endregion        
    }
}
