using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NOAABO
{
    public class DataLakeLog
    {
          public string dataset { get; set; }
          
          public string datsetUrl { get; set; }

          public DateTime lastrundate { get; set; }

          public DateTime createddate { get; set; }

          public bool iscomplete { get; set; }

          public string DataLakeLogDescription { get; set; }
    }
}
