using System.Threading;
using System.Threading.Tasks;
using Slot.Gateway.Aggregator.Models;
using Slot.Gateway.Aggregator.Slot.Models;
using Slot.Gateway.V1.Models.Slot;

namespace Slot.Gateway.Aggregator.Slot
{
    public interface ISlotAggregator
    {
        Task<AggregatorResponse<SlotWeeklyResponseModel>> GetWeeklyAvailableSlots(string date, CancellationToken cancellationToken);
        Task<AggregatorResponse> TakeSlot(TakeSlotRequestModel request, CancellationToken cancellationToken);
    }
}
