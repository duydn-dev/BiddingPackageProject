using FastMember;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Neac.Common.Const.CommonConstant;

namespace Neac.Common.Const
{
    public static class CommonFunction
    {
        public static DataTable RenameHeaderAndConvertToDatatable<T>(this IEnumerable<T> data, List<string> header) where T: class
        {
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            DataTable table = new DataTable();
            if (header?.Count > 0)
            {
                header.ForEach(n => table.Columns.Add(n));
            }
            else
            {
                foreach (PropertyInfo prop in Props)
                {
                    table.Columns.Add(prop.Name);
                }
            }
               
            foreach (T item in data)  
            {  
                var values = new object[Props.Length];  
                for (int i = 0; i < Props.Length; i++)  
                {  
                    values[i] = Props[i].GetValue(item, null);  
                }
                table.Rows.Add(values);  
            }  
              
            return table;  
        }
        public static string DocumentStateName(int? state)
        {
            if (!state.HasValue)
                return "";

            string name;
            switch (state)
            {
                case (int)DocumentState.NotHad:
                    name = "Chưa có";
                    break;
                case (int)DocumentState.Had:
                    name = "Đã có";
                    break;
                case (int)DocumentState.Edited:
                    name = "Đã chỉnh sửa";
                    break;
                default:
                    name = "";
                    break;
            }
            return name;
        }
        public static double ValidRatio(double? divisor, double? dividend)
        {
            if((!dividend.HasValue && !dividend.HasValue) || (divisor != 0 && dividend == 0))
            {
                return 0;
            }
            var result = (divisor / dividend) * 100;
            return (result > 100) ? 100 : (result < 0) ? 0 : Math.Round(result.Value, 2);
        }
    }
}
