using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class DataTypesBAL
    {
        public void Datatypeinsert(List<DataTypes> datatype)
        {
            DataTypesDAL dtype = new DataTypesDAL();
            dtype.InsertDatatype(datatype);
        }
    }

}
