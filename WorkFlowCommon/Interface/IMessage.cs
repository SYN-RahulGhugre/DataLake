using System;

namespace WorkFlowCommon.Interface
{
    /// <summary>
    /// IMessage Interface
    /// </summary>
    public  interface IMessage
    {
        string From { get; set; }

        Guid WorkFlowGUID { get; set; }

        string To { get; set; }

        DateTime Timestamp { get; set; }

        object MessagePayLoad { get; set; }

    }
}
