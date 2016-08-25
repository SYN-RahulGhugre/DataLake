using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFlowCommon.Interface
{
    public interface ITaskProcessor
    {
        dynamic ExecuteTask(IDictionary<string, object> taskParameters);   
    }
}
