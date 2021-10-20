using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.ProjectDtos
{
    public class CurrentStateResponseDto
    {
        public Guid? PackageId { get; set; }
        public bool? IsStartState { get; set; }
        public bool? IsFinalState { get; set; }
    }
}
