using Neac.Common.Dtos.BiddingPackage;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos.ProjectDtos
{
    public class ProjectGetListDto
    {
        public Guid? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime? ProjectDate { get; set; }
        public string Note { get; set; }
        public int? CurrentState { get; set; }
        public List<BiddingPackageDto> BiddingPackageDtos {get;set; }

        public ProjectGetListDto()
        {
            BiddingPackageDtos = new List<BiddingPackageDto>();
        }
    }
}
