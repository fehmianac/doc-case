using System.Net;

namespace Slot.Gateway.External.Base
{
    public class ExternalApiResponse<T>
    {
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess => StatusCode is >= HttpStatusCode.OK and <= HttpStatusCode.NoContent;
    }
}