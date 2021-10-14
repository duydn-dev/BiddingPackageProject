using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.RoleDtos
{
    public class GetRolesAndGroupDto
    {
        public List<Guid> SelectedIds { get; set; }
        public List<DecentralizatedDto> ListRole { get; set; }
    }
    public class DecentralizatedDto
    {
        public Guid? Data { get; set; }
        public string Label { get; set; }
        public IEnumerable<DecentralizatedDto> Children { get; set; }
    }
}
