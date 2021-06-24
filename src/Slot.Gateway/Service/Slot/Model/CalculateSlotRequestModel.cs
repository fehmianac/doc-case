using System;
using System.Collections.Generic;

namespace Slot.Gateway.Service.Slot.Model
{
    public record CalculateSlotRequestModel(CalculateSlotWorkPeriodRequestModel WorkPeriod, List<CalculateSlotBusySlotsRequestModel> BusySlots , int SlotDurationMinutes);
    
    public record CalculateSlotWorkPeriodRequestModel(int StartHour, int EndHour, int LunchStartHour, int LunchEndHour);
    
    public record CalculateSlotBusySlotsRequestModel(DateTime Start, DateTime End);
    
}
