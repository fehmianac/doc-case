using System;
using System.Collections.Generic;
using System.Linq;
using Slot.Gateway.Service.Slot.Model;

namespace Slot.Gateway.Service.Slot
{
    public class SlotService : ISlotService
    {
        public List<string> CalculateSlot(CalculateSlotRequestModel request)
        {
            if (request.WorkPeriod == null)
                return new List<string>();

            if (request.WorkPeriod.StartHour >= request.WorkPeriod.EndHour)
                return new List<string>();
            
            var startTime =  TimeSpan.FromHours(request.WorkPeriod.StartHour);
            var endTime =  TimeSpan.FromHours(request.WorkPeriod.EndHour);
            var lunchStartTime =  TimeSpan.FromHours(request.WorkPeriod.LunchStartHour);
            var lunchStartEnd =  TimeSpan.FromHours(request.WorkPeriod.LunchEndHour);
            
            var availableSlots = new List<string>();
            while (startTime < endTime)
            {
                if (startTime >= lunchStartTime && startTime < lunchStartEnd)
                {
                    startTime = startTime.Add(TimeSpan.FromMinutes(request.SlotDurationMinutes));
                    continue;
                }

                if (request.BusySlots != null && request.BusySlots.Any(q => startTime >= q.Start.TimeOfDay && startTime < q.End.TimeOfDay))
                {
                    startTime = startTime.Add(TimeSpan.FromMinutes(request.SlotDurationMinutes));
                    continue;
                }
                availableSlots.Add(startTime.ToString(@"hh\:mm"));
                startTime = startTime.Add(TimeSpan.FromMinutes(request.SlotDurationMinutes));
            }
            return availableSlots;
        }
    }
}
