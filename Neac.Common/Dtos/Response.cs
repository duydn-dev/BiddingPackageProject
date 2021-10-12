using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.Common.Dtos
{
    public class Response<T>
    {
        public T ResponseData { get; set; }
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Response(bool success, int statusCode, string message, T data)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
            ResponseData = data;
        }
        public static Response<T> CreateSuccessResponse(T data)
        {
            return new Response<T>(true, 200, "success !", data);
        }
        public static Response<T> CreateErrorResponse(Exception exception = null)
        {
            return new Response<T>(false, 500, exception.ToString(), default(T));
        }
    }
    public class GetListResponseModel<T>
    {
        public T Data { get; set; }
        public int TotalData { get; set; }
        public int PageCount { get; set; }
        public GetListResponseModel(int totalData, int pageSize)
        {
            TotalData = totalData;
            PageCount = (int)Math.Ceiling(Convert.ToDecimal(totalData) / Convert.ToDecimal(pageSize));
        }
    }
}
