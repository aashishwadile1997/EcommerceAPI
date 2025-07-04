using System.Net;

namespace MyShopee.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            List<string> ErrorMessage = new List<string>();
        }
        public HttpStatusCode HttpStatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessage { get; set;}
        public object result { get; set; }

    }
}
