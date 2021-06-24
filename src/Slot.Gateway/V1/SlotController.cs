using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Slot.Gateway.Aggregator.Slot;
using Slot.Gateway.Aggregator.Slot.Models;
using Slot.Gateway.V1.Models.Slot;

namespace Slot.Gateway.V1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SlotController : BaseController
    {
        private readonly ISlotAggregator _slotAggregator;

        public SlotController(ISlotAggregator slotAggregator)
        {
            _slotAggregator = slotAggregator;
        }

        [HttpGet("weekly/{date}")]
        [ProducesResponseType(typeof(SlotWeeklyResponseModel), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetWeeklySlot([FromRoute] string date, CancellationToken cancellationToken)
        {
            var response = await _slotAggregator.GetWeeklyAvailableSlots(date, cancellationToken);
            return GatewayResult(response);
        }

        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        public async Task<IActionResult> TakeSlot([FromBody] TakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            var response = await _slotAggregator.TakeSlot(request, cancellationToken);
            return GatewayResult(response);
        }
    }
}
