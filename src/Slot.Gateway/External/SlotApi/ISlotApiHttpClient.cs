using System.Threading;
using System.Threading.Tasks;
using Slot.Gateway.External.SlotApi.Models.Request;
using Slot.Gateway.External.SlotApi.Models.Response;

namespace Slot.Gateway.External.SlotApi
{
    public interface ISlotApiHttpClient
    {
        Task<Base.ExternalApiResponse<SlotApiAvailableSlotResponseModel>> GetAvailableSlots(string mondayOfWeek, CancellationToken cancellationToken);


        Task<Base.ExternalApiResponse<bool>> TakeSlot(SlotApiTakeSlotRequestModel request, CancellationToken cancellationToken);
    }
}