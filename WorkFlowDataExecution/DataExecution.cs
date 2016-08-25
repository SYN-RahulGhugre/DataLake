using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using WorkFlowCommon;
using WorkFlowCommon.Interface;
using WorkFlowLog;

namespace WorkFlowDataExecution
{
    public class DataExecution
    {

        /// <summary>
        /// GetAPIResponse Method read url and get data from API service and return data
        /// </summary>
        /// <param name="URL">Actual URL its come from database</param>
        /// <param name="workflowDefinationID"> Workflow Defination ID Parameter</param>
        /// <param name="workflowTaskId">Workflow TaskId Parameter</param>
        /// <param name="startedOn">Work Flow StartedOn Date</param>
        /// <returns> return jsonResponse</returns>

       public dynamic GetAPIResponse(string URL, int workflowDefinitionID, string workflowTaskId, DateTime startedOn)
       // public dynamic GetAPIResponse(ITaskProcessor taskProcessor)
        {
            
            dynamic jsonResponse = null;
            Run run = new Run();
            Log log = new Log();
            string Token = LogMessage.Token;
            string TokenValue = LogMessage.TokenValue;

            string headerToken = ConfigurationManager.AppSettings[Token].ToString();
            string headerTokenValue = ConfigurationManager.AppSettings[TokenValue].ToString();

            log.InsertWorkFlowLog(workflowDefinitionID, 3, LogMessage.CallGetAPIResponseLogMsg, null, null);

            using (var client = new HttpClient())
            {
                DateTime? completedOn = null;              
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add(headerToken, headerTokenValue);
                    HttpResponseMessage APIResponse = client.GetAsync(URL).Result;
                    bool IsSuccess = APIResponse.IsSuccessStatusCode;
                    jsonResponse = GetJsonResponse(APIResponse, workflowDefinitionID);
                    completedOn = DateTime.UtcNow;
                    run.UpdateWorkFlowRun(workflowDefinitionID, DateTime.UtcNow);
                    log.InsertWorkFlowLog(workflowDefinitionID, 3, LogMessage.UpdateCutOffDateLogMsg, null, null);
                    return jsonResponse;
                }
                catch (Exception ex)
                {
                    var getExceptionType = ex.GetType();
                    run.InsertWorkFlowRun(workflowDefinitionID, 1, startedOn, null, false, null, null);
                    log.InsertWorkFlowLog(workflowDefinitionID, 1, LogMessage.ErrorGetAPIResponseLogMsg + "--" + ex.Message, getExceptionType.Name, null);
                    return jsonResponse;
                }              
            }
        }

        /// <summary>
        /// This method read APIResponse parameter and DeserializeObject 
        /// </summary>
        /// <param name="aPIResponse">APIResponse Parameter</param>
        /// <param name="workflowDefinationID"> Workflow Definition ID Parameter</param>
        /// <returns>return jsonDeserialize Data</returns>
        private dynamic GetJsonResponse(HttpResponseMessage aPIResponse,int workflowDefinitionID)
        {
            Run run = new Run();
            Log log = new Log();
            try
            {
                log.InsertWorkFlowLog(workflowDefinitionID, 3, LogMessage.CallGetJsonResponseLogMsg, null, null);
                var jsonData = aPIResponse.Content.ReadAsStringAsync().Result;
                var jsonDeserializeData = (JObject)JsonConvert.DeserializeObject(jsonData); 
                return jsonDeserializeData;
            }
            catch (Exception ex)
            {
                var getExceptionType = ex.GetType();            
                log.InsertWorkFlowLog(workflowDefinitionID, 1, LogMessage.ErrorGetJsonResponseLogMsg + "--" + ex.Message, getExceptionType.Name, null);
                return ex;
            }           
        }
    }
}
