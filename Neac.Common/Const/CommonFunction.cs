using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Const
{
    public static class CommonFunction
    {
        public static DataTable RenameHeaderAndConvertToDatatable<T>(List<string> header, List<T> data) where T: class
        {
            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(data, string.Join(",", header)))
            {
                table.Load(reader);
            }
            return table;
        }
    }
}
