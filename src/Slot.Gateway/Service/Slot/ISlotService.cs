using System.Collections.Generic;
using Slot.Gateway.Service.Slot.Model;

namespace Slot.Gateway.Service.Slot
{
    public interface ISlotService
    {
        public List<string> CalculateSlot(CalculateSlotRequestModel request);
    }
}