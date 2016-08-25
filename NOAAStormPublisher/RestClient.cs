using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using DataLake.DAL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Data;
using DataLakeSharedLibrary.Log;

namespace NOAAStormPublisher
{
    public class RestClient
    {

        #region Constants
        const string APIUrlConfigKey = "StormApiURl";
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

        private string _stormApiURlApiUrl;
        private string _startDate;
        private string _endDate;
        string _url;
        string _datasetname;
        string APIResponseString;

        #endregion
        #region Private Methods

        /// <summary>
        /// Reading config keys for storm API service
        /// </summary>


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
                    client.BaseAddress = new Uri(_stormApiURlApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                    Console.WriteLine(_url);
                    APIResponse = client.GetAsync(_url).Result;
                    IsSuccess = APIResponse.IsSuccessStatusCode;
                    //var response = await APIResponse.Content.ReadAsStringAsync();

                    //APIResponseString = response.ToString();
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Exception in getAPIResponse method");

                    Console.WriteLine("Exception " + ex.InnerException + "   " + ex.Message);
                }

                Console.WriteLine("Return response getAPIResponse Method");
            }

        }
        private void getConfigValues()
        {
            _stormApiURlApiUrl = ConfigurationManager.AppSettings[APIUrlConfigKey].ToString();
            var _startDate1 = Convert.ToDateTime(ConfigurationManager.AppSettings[DateToGetDataFromConfigKey].ToString());
            //_startDate = _startDate1.ToString(("yyyyMMddhhmm"));
            //_endDate = DateTime.Now.ToString(("yyyyMMddhhmm")); 
            _startDate = _startDate1.ToString("yyyyMMddhhmm");
            _endDate = DateTime.Now.ToString("yyyyMMddhhmm");
            //_url =  "json/" + "warn" + "/" + _startDate  + ":" + _endDate + "";
            
            using (var db = new DataLakeEntities())
            {
                DataTable alldatset = new DataTable();
                DateTime datetime = new DateTime(1900, 1, 1).Add(Convert.ToDateTime(DateTime.MinValue).TimeOfDay);
                  _datasetname  = db.uspGetallStormdatasets().FirstOrDefault() ;                
                
                    
                    var checklastlog = db.uspGetlastrundate(_datasetname).FirstOrDefault();
                    if (checklastlog == datetime)
                    {
                        _url = "json/" + _datasetname + "/" + _startDate + ":" + _endDate + "";
                    }
                    else
                    {
                        string _checklastlog = checklastlog.Value.ToString("yyyyMMddhhmm");
                    _url = "json/" + _datasetname + "/" + _checklastlog + ":" + _endDate + "";
                    }
                
              }            
        }
        public  dynamic GetStormData()
        {
            var jsonData = APIResponse.Content.ReadAsStringAsync().Result;
            var finaldata = (JObject)JsonConvert.DeserializeObject(jsonData);
            //dynamic d = JObject.Parse(jsonData);
            //  WeatherDatasets account = JsonConvert.DeserializeObject<WeatherDatasets>(jsonData);


            if (finaldata["result"].Count() == 0)
            {
               
                return null;
            }
            else
            {
                dynamic obj = finaldata["result"];
                return obj;
            }

                 
        }

        public LogData GetLogData()
        {
            LogData logData = new LogData();
            logData.DatasetName = "WARN";
            logData.DatasetURL = _url;
            return logData;
        }
        #endregion
    }
}
