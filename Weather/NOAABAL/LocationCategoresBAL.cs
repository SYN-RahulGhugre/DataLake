using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NOAABO;
using NOAADAL;

namespace NOAABAL
{
    public class LocationCategoresBAL
    {
        public void LocationCategoriesInsert(List<LocationCategories> loc_cat)
        {
            LocationCategoriesDAL locdal = new LocationCategoriesDAL();
            locdal.InsertLocationcategories(loc_cat);
        }

    }
}
