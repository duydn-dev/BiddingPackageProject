using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.PositionDtos
{
    public class PositionGetFilterDto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string TextSearch { get; set; }

        public PositionGetFilterDto()
        {
            PageSize = 10;
            PageIndex = 1;
        }
    }
    public class PositonGetDropdownViewDto
    {
        public Guid? UserPositionId { get; set; }
        public string UserPositionName { get; set; }
        public bool? IsAdministrator { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
    }
}
