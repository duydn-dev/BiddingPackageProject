using Neac.Common.Dtos;
using Neac.Common.Dtos.DocumentDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IDocumentRepository
    {
        Task<Response<GetListResponseModel<List<Document>>>> GetFilter(string filter);
        Task<Response<List<Document>>> GetDropdownAsync();
        Task<Response<List<Document>>> GetDropdownByPackageIdAsync(DocumentByPackageIdDto request);
        Task<Response<Document>> GetByIdAsync(Guid projectId);
        Task<Response<DocumentDto>> CreateAsync(DocumentDto request);
        Task<Response<DocumentDto>> UpdateAsync(DocumentDto request);
        Task<Response<bool>> DeleteAsync(Guid projectId);
    }
}
