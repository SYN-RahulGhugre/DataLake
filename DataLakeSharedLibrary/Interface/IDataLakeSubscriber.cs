using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLakeSharedLibrary.Interface
{
    public interface IDataLakeSubscriber
    {
        string SubscribeMessage(string Topic, string Subcription);
    }
}
