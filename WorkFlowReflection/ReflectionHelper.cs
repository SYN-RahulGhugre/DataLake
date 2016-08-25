using System;
using System.Collections.Generic;
using System.Reflection;
using WorkFlowCommon;
using WorkFlowCommon.Interface;
using WorkFlowLog.InterFace;

namespace WorkFlowReflection
{
    public class ReflectionHelper
    {       
        public dynamic InvokeMethodByReflection(string fullddlNamePath, string type, string method, IDictionary<string, object> taskParameters)
        {
            dynamic jsonResponse=null;

            Assembly assembly = Assembly.LoadFrom(@"C:\Workspace\Projects\trunk\Application\DataLake\WorkFlowSubscriber\bin\Debug\WorkFlowCommon.dll");

            Type assemblyGetType = assembly.GetType(type);       

            if (assemblyGetType.IsAssignableFrom(assemblyGetType))
            {              
                ITaskProcessor taskProcessor =(ITaskProcessor)Activator.CreateInstance(assemblyGetType);
                jsonResponse = taskProcessor.ExecuteTask(taskParameters);
            }         


            return jsonResponse;
        }
        
     
    }
}
