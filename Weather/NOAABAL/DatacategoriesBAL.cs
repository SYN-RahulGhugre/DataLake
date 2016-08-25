using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class DatacategoriesBAL
    {
        public void Datacategoriesinsert(List<Datacategories> categories)
        {
            DatacategoriesDAL  dst = new DatacategoriesDAL();
            dst.InsertDataCategories(categories);
        }
    }
}
