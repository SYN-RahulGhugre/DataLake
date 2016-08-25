using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
//using NOAAData;
//using NOAAManager.Managers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NOAADAL;
using NOAABO;
using NOAABAL;
using System.Configuration;
using System.Threading;



namespace NOAAWebService
{
    class Program
    {
        //ProductManager productManager = new ProductManager();
        static void Main(string[] args)
        {
            Console.WriteLine("process Started at " + DateTime.Now);
            
            Program program = new Program();

            //string URL = @"http://www.ncdc.noaa.gov/cdo-web/api/v2/datasets";
            string URL = @"http://www.ncdc.noaa.gov/cdo-web/";

            
            //////////////////// Data  test///////////////////
            /*
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                // New code:
                // Console.WriteLine("process Started");
                HttpResponseMessage response = client.GetAsync("api/v2/data?datasetid=GHCND&startdate=2016-07-22&enddate=2016-07-22").Result;
                HttpResponseMessage response20 = client.GetAsync("api/v2/data?datasetid=GHCND&startdate=2016-07-23&enddate=2016-07-23").Result;
                HttpResponseMessage response21 = client.GetAsync("api/v2/data?datasetid=GHCND&startdate=2016-07-24&enddate=2016-07-24").Result;
                //HttpResponseMessage response22 = client.GetAsync("api/v2/data?datasetid=GHCND&startdate=2016-07-22&enddate=2016-07-22").Result;

                //HttpResponseMessage response = client.GetAsync("api/v2/datasets?datatypeid=TOBS").Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var jsonData20 = response20.Content.ReadAsStringAsync().Result;
                    var jsonData21 = response21.Content.ReadAsStringAsync().Result;
                    //var jsonData22 = response22.Content.ReadAsStringAsync().Result;
                    var product = (JObject)JsonConvert.DeserializeObject(jsonData);                                       
                }
                Console.WriteLine("Dataset updated on  " + DateTime.Now);
            }                    

            //////////// End //////////////////
            */
                        
            ////// calling dataset/////////
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                // New code:
                // Console.WriteLine("process Started");
                HttpResponseMessage response = client.GetAsync("api/v2/datasets").Result;

                //HttpResponseMessage response = client.GetAsync("api/v2/datasets?datatypeid=TOBS").Result;

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var product = (JObject)JsonConvert.DeserializeObject(jsonData);
                    var result = product["results"].Select(item => new
                    Datasets
                    {
                        uid = item["uid"].ToString(),
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        name = item["name"].ToString(),
                        datacoverage = item["datacoverage"].ToString(),
                        id = item["id"].ToString(),
                    }).ToList();

                    program.Datsetinsert(result);
                }
                Console.WriteLine("Dataset updated on  " + DateTime.Now);
            }            
           
            ////Calling Datacategories////            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                // New code:  
                HttpResponseMessage response = client.GetAsync("api/v2/datacategories?limit=42").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var categories = (JObject)JsonConvert.DeserializeObject(jsonData);

                    var result = categories["results"].Select(item => new
                   Datacategories
                    {
                        id = item["id"].ToString(),
                        name = item["name"].ToString(),

                    }).ToList();
                    program.DatacategoriesInsert(result);
                }
                Console.WriteLine("Datacategories on  " + DateTime.Now);
                //Console.ReadLine();
            }
            ////////// Calling DataTypes//////////
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");                
                HttpResponseMessage response = client.GetAsync("api/v2/datatypes?limit=1000").Result;
                HttpResponseMessage response1 = client.GetAsync("api/v2/datatypes?&offset=1001&limit=1000").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var datatypes = (JObject)JsonConvert.DeserializeObject(jsonData);
                    var jsonData1 = response1.Content.ReadAsStringAsync().Result;                    
                    var datatypes1 = (JObject)JsonConvert.DeserializeObject(jsonData1);
                    datatypes.Merge(datatypes1);
                    var dataresult = datatypes["results"].Select(item => new
                     DataTypes
                    {
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        name = item["name"].ToString(),
                        datacoverage = Convert.ToDouble(item["datacoverage"]),
                        id = item["id"].ToString(),
                    }).ToList();
                    program.DatatypeInsert(dataresult);
                }
                Console.WriteLine("DataTypes Updated on  " + DateTime.Now);
                //Console.ReadLine();               
            }
                        
            ///// calling Location categories //////
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                HttpResponseMessage response = client.GetAsync("api/v2/locationcategories").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var loc_categories = (JObject)JsonConvert.DeserializeObject(jsondata);
                    var locresult = loc_categories["results"].Select(item => new
                    LocationCategories
                    {
                        id = item["id"].ToString(),
                        name = item["name"].ToString(),
                    }).ToList();
                    program.LocationCategoriesInsert(locresult);
                }
                Console.WriteLine("Location Categories Updated on  " + DateTime.Now);
                //Console.ReadLine();      
            }            
            
            ////// Call Locations//////
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                var jsonmain = client.GetStringAsync("api/v2/locations?limit=1000").Result;
                var main = (JObject)JsonConvert.DeserializeObject(jsonmain);                
                //var count = main["metadata"]["resultset"]["count"];
                int cnt = Convert.ToInt32(main["metadata"]["resultset"]["count"]); 
                for (int i = 1001; i < cnt; i = i + 1000)
                {
                    string offset = i.ToString() ;
                    HttpResponseMessage response;//                 
                   
                     string url1 = "api/v2/locations?offset=" + i + "&limit=1000";
                     response = client.GetAsync(url1).Result;    
                    if (response.IsSuccessStatusCode )
                    {                          
                        var jsondata    = response.Content.ReadAsStringAsync().Result;                        
                        var locations = (JObject)JsonConvert.DeserializeObject(jsondata);                                               
                        main.Merge(locations);
                        jsondata = null;
                        //locations = null;                        
                    }                   
                }
                var locresult = main["results"].Select(item => new
                 Locations
                {
                    mindate = Convert.ToDateTime(item["mindate"]),
                    maxdate = Convert.ToDateTime(item["maxdate"]),
                    name = item["name"].ToString(),
                    datacoverage = Convert.ToDouble(item["datacoverage"]),
                    id = item["id"].ToString(),
                }).ToList();
                program.LocationInsert(locresult);

                Console.WriteLine("Locations Updated on  " + DateTime.Now);
               // Console.ReadLine();
            }           
            
            //// calling Stations///
           //// getting error at 252 limit i.e we are getting error for id "COOP:016278"/////
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                HttpResponseMessage response = client.GetAsync("api/v2/stations?datasetid=GHCND&limit=1000").Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    var stations = (JObject)JsonConvert.DeserializeObject(jsondata);                   

                     int cnt = Convert.ToInt32(stations["metadata"]["resultset"]["count"]);
                    for (int i = 1001; i < cnt; i = i + 1000)
                    {
                        string offset = i.ToString();
                        HttpResponseMessage response1;//                 

                        string url1 = "api/v2/stations?datasetid=GHCND&offset=" + i + "&limit=1000";
                        response1 = client.GetAsync(url1).Result;
                        if (response1.IsSuccessStatusCode)
                        {
                            var jsondata1 = response1.Content.ReadAsStringAsync().Result;
                            var stations1 = (JObject)JsonConvert.DeserializeObject(jsondata1);
                            stations.Merge(stations1);
                            jsondata1 = null;
                            //locations = null;                       
                        }
                    }
                    var staresult = stations["results"].Select(item => new
                    Stations
                    {
                        elevation = item["elevation"]!=null ? Convert.ToDouble(item["elevation"]) : 0.0,
                        mindate = Convert.ToDateTime(item["mindate"]),
                        maxdate = Convert.ToDateTime(item["maxdate"]),
                        latitude = Convert.ToDouble(item["latitude"]),
                        name = item["name"].ToString(),
                        datacoverage = Convert.ToDouble(item["datacoverage"]),
                        id = item["id"].ToString(),
                        elevationUnit = Convert.ToString(item["elevationUnit"]),
                        longitude = Convert.ToDouble(item["longitude"]),
                    }).ToList();
                    program.StationInsert(staresult);
                }
                Console.WriteLine("Stations Updated on  " + DateTime.Now);
                //Console.ReadLine();
            }
            
            /*
            //////calling Data ///////////            
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                    //string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    string lastrundate1 = "";
                    DateTime date1 = Convert.ToDateTime(ConfigurationManager.AppSettings["startdatetime"]);//Convert.ToDateTime("18/07/2016");
                    string startdate = date1.ToString("yyyy-MM-ddTHH:mm:ss");
                    ///string startdate = ConfigurationManager.AppSettings["startdatetime"] ;                    
                    //string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    //DateTime date2 = Convert.ToDateTime(ConfigurationManager.AppSettings["enddatetime"]);
                    string enddate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    //string enddate = date2.ToString("yyyy-MM-ddTHH:mm:ss");
                    DateTime isnulldate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    //DateTime lastrundate = new DateTime();
                    //lastrundate1 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    GetAllDataBAL lastdate = new GetAllDataBAL();
                    DateTime lastrundate = lastdate.getlastrundate();
                    lastrundate1 = lastrundate.ToString("yyyy-MM-ddTHH:mm:ss");
                    DataLakeLog datalog = new DataLakeLog();
                    string URL1;
                    if (lastrundate == isnulldate)
                    {
                        URL1 = "api/v2/data?datasetid=GHCND&startdate=" + startdate + "&enddate=" + enddate + "&limit=1000";
                        lastrundate1 = startdate;
                        //lastrundate = Convert.ToDateTime("07/20/2016THH:mm:ss");   //DateTime.Now;
                        //lastrundate1 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        //enddate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                        URL1 = "api/v2/data?datasetid=GHCND&startdate=" + lastrundate1 + "&enddate=" + enddate + "&limit=1000";
                        //lastrundate1 = lastrundate.ToString();
                    }
                    string dataseturl = URL + URL1;
                    HttpResponseMessage response = client.GetAsync(URL1).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //int len = response.Content.ReadAsStringAsync().Result.Length;
                        string jsondata = response.Content.ReadAsStringAsync().Result;
                        var alldata = (JObject)JsonConvert.DeserializeObject(jsondata);
                        int cnt1 = alldata.Count;
                        if (cnt1 != 0)
                        {
                            int cnt = Convert.ToInt32(alldata["metadata"]["resultset"]["count"]);
                            for (int i = 1001; i < cnt; i = i + 1000)
                            {
                                string offset = i.ToString();
                                HttpResponseMessage response1;//              

                                //string url1 = "api/v2/locations?offset=" + i + "&limit=1000";                            
                                string url1 = "api/v2/data?datasetid=GHCND&startdate=" + lastrundate1 + "&enddate=" + enddate + "&offset=" + i + "&limit=1000";
                                response1 = client.GetAsync(url1).Result;
                                if (response1.IsSuccessStatusCode)
                                {
                                    var jsondata1 = response1.Content.ReadAsStringAsync().Result;
                                    var datanfinal = (JObject)JsonConvert.DeserializeObject(jsondata1);
                                    alldata.Merge(datanfinal);
                                    jsondata1 = null;
                                    //locations = null;                       
                                }
                            }
                            var alldataresult = alldata["results"].Select(item => new
                            ALLData
                            {
                                date = Convert.ToDateTime(item["date"]),
                                datatype = item["datatype"].ToString(),
                                stations = item["station"].ToString(),
                                attributes = item["attributes"].ToString(),
                                value = Convert.ToInt16(item["value"]),
                            }).ToList();
                            program.AllDataInsert(alldataresult);
                            
                            datalog.dataset = "GHCND";
                            datalog.datsetUrl = dataseturl;                           
                            datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                            datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            datalog.iscomplete = true;
                            datalog.DataLakeLogDescription = response.ReasonPhrase;
                            DateLakeLogBAL lakebal = new DateLakeLogBAL();
                            lakebal.insertDatalog(datalog);
                            Console.WriteLine("Data Updated on " + DateTime.Now);
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        else
                        {
                            datalog.dataset = "GHCND";
                            datalog.datsetUrl = dataseturl;
                            datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                            datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            datalog.iscomplete = false;
                            datalog.DataLakeLogDescription = "No data found for given set of parameters";
                            DateLakeLogBAL lakebal = new DateLakeLogBAL();
                            lakebal.insertDatalog(datalog);
                            Console.WriteLine("No data found for given set of parameters");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        datalog.dataset = "GHCND";
                        datalog.datsetUrl = dataseturl;
                        datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                        datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        datalog.iscomplete = false;
                        datalog.DataLakeLogDescription = response.ReasonPhrase;
                        DateLakeLogBAL lakebal = new DateLakeLogBAL();
                        lakebal.insertDatalog(datalog);
                        Console.WriteLine(" " + response.ReasonPhrase + " For given set of parameters");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                Console.WriteLine("error" + ex);
                Console.ReadLine();
            }
            ///////////// End Data ////////////////
            */
            
            TimerCallback callback = new TimerCallback(Tick);
           
            Timer stateTimer = new Timer(callback, null, 0, 600000);            
            for (;;)
            {
                // add a sleep for 100 mSec to reduce CPU usage
                Thread.Sleep(60000);
            }            
            

            //Uri address = new Uri(URL);

            //HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            //request.Method = "GET";
            //request.ContentType = "text/json";
            //request.Headers.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
            //var postStream = request.GetResponse();

            ////string OutputFile = OutputPath + Dataset + "_" + StartRange + "_" + EndRange + "." + FileFormat;
            //// Get response
            //using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            //{
            //    // Get the response stream
            //    StreamReader reader = new StreamReader(response.GetResponseStream());
            //    // Console application output
            //    //Console.WriteLine(reader.ReadToEnd());
            //    string content = reader.ReadToEnd();
            //}
        }
        public void  Datsetinsert(List<Datasets> datsetdata)
        {
            //dataset dst = new dataset();
            DatasetBAL dstbl = new DatasetBAL();
            dstbl.Datasetinsert(datsetdata);
            //dst.Insert(datset);
        }
        public void DatacategoriesInsert(List<Datacategories> datacategories)
        {
            DatacategoriesBAL dstbl = new DatacategoriesBAL();
            dstbl.Datacategoriesinsert(datacategories);
        }
        public void DatatypeInsert(List<DataTypes> dtypes)
        {
            DataTypesBAL dtbal = new DataTypesBAL();
            dtbal.Datatypeinsert(dtypes);
        }
        public void LocationCategoriesInsert(List<LocationCategories> loc_categories)
        {
            LocationCategoresBAL loc_catbal = new LocationCategoresBAL();
            loc_catbal.LocationCategoriesInsert(loc_categories);
        }

        public void LocationInsert(List<Locations> loc)
        {
            LocationsBAL loc_bal = new LocationsBAL();
            loc_bal.Locationsinserts(loc);
        }

        public void StationInsert(List<Stations> stations)
        {
            StationsBAL stbal = new StationsBAL();
            stbal.StationsInsert(stations);
        }
        public void AllDataInsert(List<ALLData> data)
        {
            ALLDataBAL databal = new ALLDataBAL();
            databal.AllDataInsert(data);
        }
        static public void Tick(Object stateInfo)
        {
            //Console.WriteLine("Tick: {0}", DateTime.Now.ToString("h:mm:ss"));
            string URL = @"http://www.ncdc.noaa.gov/cdo-web/";
            Program program = new Program();
           
            //////calling Data old  ///////////
            /*
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                    string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    //string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    string enddate = DateTime.Now.ToString("yyyy-MM-dd");

                    HttpResponseMessage response = client.GetAsync("api/v2/data?datasetid=GHCND&locationid=ZIP:28801&startdate=" + startdate + "&enddate=" + enddate + "").Result;
                    //HttpResponseMessage respons8 = client.GetAsync("api/v2/data?datasetid=GHCND&locationid=ZIP:28801&startdate=2010-05-01&enddate=2010-05-01&limit=50").Result;
                    //HttpResponseMessage response1 = client.GetAsync("api/v2/data?datasetid=ANNUAL&locationid=ZIP:28802&startdate=2010-05-01&enddate=2010-05-01").Result;
                    //HttpResponseMessage response2 = client.GetAsync("api/v2/data?datasetid=GHCNDL&locationid==ZIP:28801&startdate=2010-05-02&enddate=2010-05-02").Result;
                    //HttpResponseMessage response3 = client.GetAsync("api/v2/data?datasetid=GHCNDL&locationid==ZIP:28802&startdate=2010-05-02&enddate=2010-05-02&limit=100").Result;
                    //HttpResponseMessage response3 = client.GetAsync("api/v2/data?datasetid=PRECIP_15&stationid=COOP:010008&units=metric&startdate=2010-05-01&enddate=2010-05-31&limit=100").Result;
                    //HttpResponseMessage response4 = client.GetAsync("api/v2/data?datasetid=GSOM&stationid=GHCND:USC00010008&units=standard&startdate=2010-05-01&enddate=2010-05-31&limit=100").Result;
                    //&locationid=FIPS:38
                    //No data found for below parameters /
                    //HttpResponseMessage response5 = client.GetAsync("api/v2/data?datasetid=ANNUAL&locationid=FIPS:37&startdate=2010-05-02&enddate=2010-05-02").Result;
                    //HttpResponseMessage response6 = client.GetAsync("api/v2/data?datasetid=ANNUAL&locationid=FIPS:38&startdate=2010-05-01&enddate=2010-05-01").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var jsondata = response.Content.ReadAsStringAsync().Result;
                        //var jsondata1 = response1.Content.ReadAsStringAsync().Result;
                        //var jsondata2 = response2.Content.ReadAsStringAsync().Result;
                        // var jsondata3 = response3.Content.ReadAsStringAsync().Result;
                        //var jsondata4 = response4.Content.ReadAsStringAsync().Result;
                        //var jsondata5 = response5.Content.ReadAsStringAsync().Result;
                        //var jsondata6 = response4.Content.ReadAsStringAsync().Result;                    

                        var alldata = (JObject)JsonConvert.DeserializeObject(jsondata);

                        //var alldata = (JObject)JsonConvert.DeserializeObject(jsondata4);
                        //var alldata2 = (JObject)JsonConvert.DeserializeObject(jsondata3);
                        //var alldata1 = (JObject)JsonConvert.DeserializeObject(jsondata);
                        //alldata.Merge(alldata2);
                        //alldata.Merge(alldata1);
                        //var data = (JObject)JsonConvert.DeserializeObject(jsondata4);
                        var alldataresult = alldata["results"].Select(item => new
                        ALLData
                        {
                            date = Convert.ToDateTime(item["date"]),
                            datatype = item["datatype"].ToString(),
                            stations = item["station"].ToString(),
                            attributes = item["attributes"].ToString(),
                            value = Convert.ToInt16(item["value"]),
                        }).ToList();
                        program.AllDataInsert(alldataresult);
                    }
                    Console.WriteLine("Data Updated on " + DateTime.Now);
                    Console.WriteLine(); 
                       
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            ///////////// End Data  old  //////////////// 
            */
            //////calling Data ///////////            
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Token", "MnvlFcXOTrQtlaAYCTghEYNQABKSMiKh");
                    //string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    string lastrundate1 = "";
                    DateTime date1 = Convert.ToDateTime(ConfigurationManager.AppSettings["startdatetime"]);//Convert.ToDateTime("18/07/2016");
                    string startdate = date1.ToString("yyyy-MM-ddTHH:mm:ss");
                    ///string startdate = ConfigurationManager.AppSettings["startdatetime"] ;                    
                    //string startdate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    //DateTime date2 = Convert.ToDateTime(ConfigurationManager.AppSettings["enddatetime"]);
                    string enddate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    //string enddate = date2.ToString("yyyy-MM-ddTHH:mm:ss");
                    DateTime isnulldate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    //DateTime lastrundate = new DateTime();
                    //lastrundate1 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    GetAllDataBAL lastdate = new GetAllDataBAL();
                    DateTime lastrundate = lastdate.getlastrundate();
                    lastrundate1 = lastrundate.ToString("yyyy-MM-ddTHH:mm:ss");
                    DataLakeLog datalog = new DataLakeLog();
                    string URL1;
                    if (lastrundate == isnulldate)
                    {
                        URL1 = "api/v2/data?datasetid=GHCND&startdate=" + startdate + "&enddate=" + enddate + "&limit=1000";
                        lastrundate1 = startdate;
                        //lastrundate = Convert.ToDateTime("07/20/2016THH:mm:ss");   //DateTime.Now;
                        //lastrundate1 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                    else
                    {
                        //enddate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                        URL1 = "api/v2/data?datasetid=GHCND&startdate=" + lastrundate1 + "&enddate=" + enddate + "&limit=1000";
                        //lastrundate1 = lastrundate.ToString();
                    }
                    string dataseturl = URL + URL1;
                    HttpResponseMessage response = client.GetAsync(URL1).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        //int len = response.Content.ReadAsStringAsync().Result.Length;
                        string jsondata = response.Content.ReadAsStringAsync().Result;
                        var alldata = (JObject)JsonConvert.DeserializeObject(jsondata);
                        int cnt1 = alldata.Count;
                        if (cnt1 != 0)
                        {
                            int cnt = Convert.ToInt32(alldata["metadata"]["resultset"]["count"]);
                            for (int i = 1001; i < cnt; i = i + 1000)
                            {
                                string offset = i.ToString();
                                HttpResponseMessage response1;//              

                                //string url1 = "api/v2/locations?offset=" + i + "&limit=1000";                            
                                string url1 = "api/v2/data?datasetid=GHCND&startdate=" + lastrundate1 + "&enddate=" + enddate + "&offset=" + i + "&limit=1000";
                                response1 = client.GetAsync(url1).Result;
                                if (response1.IsSuccessStatusCode)
                                {
                                    var jsondata1 = response1.Content.ReadAsStringAsync().Result;
                                    var datanfinal = (JObject)JsonConvert.DeserializeObject(jsondata1);
                                    alldata.Merge(datanfinal);
                                    jsondata1 = null;
                                    //locations = null;                       
                                }
                            }
                            var alldataresult = alldata["results"].Select(item => new
                            ALLData
                            {
                                date = Convert.ToDateTime(item["date"]),
                                datatype = item["datatype"].ToString(),
                                stations = item["station"].ToString(),
                                attributes = item["attributes"].ToString(),
                                value = Convert.ToInt16(item["value"]),
                            }).ToList();
                            program.AllDataInsert(alldataresult);

                            datalog.dataset = "GHCND";
                            datalog.datsetUrl = dataseturl;
                            datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                            datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            datalog.iscomplete = true;
                            datalog.DataLakeLogDescription = response.ReasonPhrase;
                            DateLakeLogBAL lakebal = new DateLakeLogBAL();
                            lakebal.insertDatalog(datalog);
                            Console.WriteLine("Data Updated on " + DateTime.Now);
                            Console.WriteLine();
                            Console.ReadLine();
                        }
                        else
                        {
                            datalog.dataset = "GHCND";
                            datalog.datsetUrl = dataseturl;
                            datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                            datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                            datalog.iscomplete = false;
                            datalog.DataLakeLogDescription = "No data found for given set of parameters";
                            DateLakeLogBAL lakebal = new DateLakeLogBAL();
                            lakebal.insertDatalog(datalog);
                            Console.WriteLine("No data found for given set of parameters  "+enddate );
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        datalog.dataset = "GHCND";
                        datalog.datsetUrl = dataseturl;
                        datalog.lastrundate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));  //lastrundate;
                        datalog.createddate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                        datalog.iscomplete = false;
                        datalog.DataLakeLogDescription = response.ReasonPhrase;
                        DateLakeLogBAL lakebal = new DateLakeLogBAL();
                        lakebal.insertDatalog(datalog);
                        Console.WriteLine(" " + response.ReasonPhrase + " For given set of parameters  "+ enddate);
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex;
                Console.WriteLine("error" + ex);
                Console.ReadLine();
            }
            ///////////// End Data ////////////////            
        }
    }
}
