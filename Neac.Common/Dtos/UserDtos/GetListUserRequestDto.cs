using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos
{
    public class GetListUserRequestDto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string TextSearch { get; set; }
        public int? Status { get; set; }

        public GetListUserRequestDto()
        {
            PageSize = 10;
            PageIndex = 1;
        }
    }
}
