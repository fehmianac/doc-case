using System;

namespace Slot.Gateway.Service.Date
{
    public class DateService : IDateService
    {
        public DateTime FindMondayOfWeek(DateTime input)
        {
            if (input.DayOfWeek == DayOfWeek.Monday)
            {
                return input;
            }
            var diff = (7 + (input.DayOfWeek - DayOfWeek.Monday)) % 7;
            return input.AddDays(-1 * diff).Date;
        }

        public DateTime FindNextMonday(DateTime input)
        {
            var monday = FindMondayOfWeek(input);
            return monday.AddDays(7);
        }
        
        
        public DateTime FindPrevMonday(DateTime input)
        {
            var monday = FindMondayOfWeek(input);
            return monday.AddDays(-7);
        }
    }
}
