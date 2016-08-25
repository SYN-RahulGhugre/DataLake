using System;
using WorkFlowCommon.Interface;

namespace WorkFlowCommon
{
    /// <summary>
    /// Concrete class inherited from IMessage
    /// </summary>
    public class Message:IMessage
    {
      public  string From { get; set; }

      public  Guid WorkFlowGUID { get; set; }

     public  string To { get; set; }

      public  DateTime Timestamp { get; set; }

      public  object MessagePayLoad { get; set; }

    }
}
