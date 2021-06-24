using System;
using System.Collections.Generic;

namespace Slot.Gateway.Aggregator.Slot.Models
{
    public class SlotWeeklyResponseModel
    {
        public WeeklyFacilityResponseModel Facility { get; set; }
        public List<WeeklySlotDayItemResponseModel> Items { get; set; }
        public DateTime Today { get; set; }
        public DateTime PrevMonday { get; set; }
        public DateTime NextMonday { get; set; }
        public DateTime ThisMonday { get; set; }
        public int SlotDurationMinutes { get; set; }
    }

    public class WeeklyFacilityResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
    
    public class WeeklySlotDayItemResponseModel
    {
        public string DayName { get; set; }
        
        public DateTime Date { get; set; }
        
        public List<string> Slots { get; set; }
    }
}
