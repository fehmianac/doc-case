using System.Net;

namespace Slot.Gateway.Aggregator.Models
{
    public class AggregatorResponse<T> : AggregatorResponse
    {
        public T Data { get; set; }
    }

    public class AggregatorResponse
    {
        public AggregatorResponse()
        {
            StatusCode = HttpStatusCode.OK;
        }
        
        public string ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess => StatusCode == HttpStatusCode.OK || StatusCode == HttpStatusCode.Created;
    }
}