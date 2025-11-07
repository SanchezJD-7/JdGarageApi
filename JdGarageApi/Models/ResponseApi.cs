using System.Net;

namespace JdGarageApi.Models
{
    public class ResponseApi
    {
        public ResponseApi()
        {
            ErrorMessage = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }
    }
}
