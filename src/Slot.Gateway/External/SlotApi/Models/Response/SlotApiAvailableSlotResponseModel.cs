using System;
using System.Collections.Generic;

namespace Slot.Gateway.External.SlotApi.Models.Response
{
    public class SlotApiAvailableSlotResponseModel
    {
        public Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        public WorkDay Monday { get; set; }
        public WorkDay Tuesday { get; set; }
        public WorkDay Wednesday { get; set; }
        public WorkDay Thursday { get; set; }
        public WorkDay Friday { get; set; }
        public WorkDay Saturday { get; set; }
        public WorkDay Sunday { get; set; }
    }
    
    public class Facility
    {
        public string FacilityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class WorkPeriod
    {
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public int LunchStartHour { get; set; }
        public int LunchEndHour { get; set; }
    }

    public class BusySlot
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class WorkDay
    {
        public WorkPeriod WorkPeriod { get; set; }
        public List<BusySlot> BusySlots { get; set; }
    }
    
}
