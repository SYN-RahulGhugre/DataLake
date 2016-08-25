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
using WorkFlowCommon.Interface;
using WorkFlowLog;
using WorkFlowLog.InterFace;

namespace WorkFlowCommon
{
   public class TaskProcessor: ITaskProcessor
    { 
        public dynamic ExecuteTask(IDictionary<string, object> taskParameter)
        {
            var serviceURL = taskParameter.FirstOrDefault(o => o.Key == "ServiceURL").Value;
            var workflowDefinitionId = taskParameter.FirstOrDefault(o => o.Key == "WorkflowDefinitionID").Value;
            var startedOn = taskParameter.FirstOrDefault(o => o.Key == "StartedOn").Value;
            var tokan = taskParameter.FirstOrDefault(o => o.Key == "Tokan").Value;

            return GetAPIResponse(Convert.ToString(serviceURL),Convert.ToInt32(workflowDefinitionId),Convert.ToDateTime(startedOn),Convert.ToString(tokan));
        }


        private dynamic GetAPIResponse(string serviceURL, int workflowDefinitionId, DateTime startedOn,string token)       
        {

            dynamic jsonResponse = null;
            IRun Objrun = new Run();
            ILog Objlog = new Log();

            Objlog.InsertWorkFlowLog(workflowDefinitionId, 3, LogMessage.CallGetAPIResponseLogMsg, null, null);

            using (var client = new HttpClient())
            {
                DateTime? completedOn = null;
                try
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Add("Token", token);
                    }

                    HttpResponseMessage APIResponse = client.GetAsync(serviceURL).Result;
                    bool IsSuccess = APIResponse.IsSuccessStatusCode;
                    jsonResponse = GetJsonResponse(APIResponse, workflowDefinitionId);
                    completedOn = DateTime.UtcNow;
                    Objrun.UpdateWorkFlowRun(workflowDefinitionId, DateTime.UtcNow);
                    Objlog.InsertWorkFlowLog(workflowDefinitionId, 3, LogMessage.UpdateCutOffDateLogMsg, null, null);
                    return jsonResponse;
                }
                catch (Exception ex)
                {
                    var getExceptionType = ex.GetType();
                   Objrun.InsertWorkFlowRun(workflowDefinitionId, 1, startedOn, null, false, null, null);
                    Objlog.InsertWorkFlowLog(workflowDefinitionId, 1, LogMessage.ErrorGetAPIResponseLogMsg + "--" + ex.Message, getExceptionType.Name, null);
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
        private dynamic GetJsonResponse(HttpResponseMessage aPIResponse,int workflowDefinitionId)
        {          
            ILog Objlog = new Log();
            try
            {
               Objlog.InsertWorkFlowLog(workflowDefinitionId, 3, LogMessage.CallGetJsonResponseLogMsg, null, null);
                var jsonData = aPIResponse.Content.ReadAsStringAsync().Result;
                var jsonDeserializeData = (JObject)JsonConvert.DeserializeObject(jsonData);
                return jsonDeserializeData;
            }
            catch (Exception ex)
            {
                var getExceptionType = ex.GetType();
                Objlog.InsertWorkFlowLog(workflowDefinitionId, 1, LogMessage.ErrorGetJsonResponseLogMsg + "--" + ex.Message, getExceptionType.Name, null);
                return ex;
            }
        }

    }
}
