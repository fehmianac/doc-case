using Slot.Gateway.Service.Slot.Model;
using Slot.Gateway.V1.Models.Slot;
using SlotApiTakeSlotRequestModel = Slot.Gateway.External.SlotApi.Models.Request.SlotApiTakeSlotRequestModel;
using WorkDay = Slot.Gateway.External.SlotApi.Models.Response.WorkDay;

namespace Slot.Gateway.Service.Mapper
{
    public interface IMapperService
    {
        CalculateSlotRequestModel MapToCalculateSlotRequestModel(WorkDay workDay, int slotDuration);
        SlotApiTakeSlotRequestModel MapToSlotApiTakeRequestModel(TakeSlotRequestModel request, int slotDuration);
    }
}