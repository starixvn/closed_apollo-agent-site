using System.Collections.Generic;
using System.Data;

/*
 * Created 20120216@Leo
 * Revision:
 * ?@? - ... 
 * 
 */

namespace Fanex.Age.Utility.Data
{
    public static class DataExtension
    {
        public static DataTable ToDataTable(this IList<DataRow> list)
        {
            DataTable result = list[0].Table.Copy();
            foreach (DataRow dr in list)
            {
                result.ImportRow(dr);
            }
            return result;
        }
    }
}
