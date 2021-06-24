using Microsoft.AspNetCore.Mvc;
using Slot.Gateway.Aggregator.Models;

namespace Slot.Gateway.V1
{
    public class BaseController : ControllerBase
    {
        protected IActionResult GatewayResult(AggregatorResponse aggregatorResponse)
        {
            if (!aggregatorResponse.IsSuccess)
            {
                return new ObjectResult(aggregatorResponse.ErrorMessage)
                {
                    StatusCode = (int)aggregatorResponse.StatusCode
                };
            }

            return base.Ok();
        }

        protected IActionResult GatewayResult<T>(AggregatorResponse<T> aggregatorResponse)
        {
            if (!aggregatorResponse.IsSuccess)
            {
                return new ObjectResult(aggregatorResponse.ErrorMessage)
                {
                    Value = aggregatorResponse.ErrorMessage
                };
            }

            return base.Ok(aggregatorResponse.Data);
        }
    }
}