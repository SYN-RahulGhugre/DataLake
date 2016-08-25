using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using NOAADAL;
//using NOAABAL;
using NOAABO;

namespace NOAABAL
{
    public class DatasetBAL
    {
        public void Datasetinsert(List<Datasets> dataset)
        {            
            //datasetDAL dst = new datasetDAL();
            DatasetDAL dst = new DatasetDAL();
            dst.InsertDataset(dataset);
        }
       
       
    }
}
