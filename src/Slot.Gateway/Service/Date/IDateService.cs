using System;

namespace Slot.Gateway.Service.Date
{
    public interface IDateService
    {
        DateTime FindMondayOfWeek(DateTime input);

        DateTime FindNextMonday(DateTime input);

        DateTime FindPrevMonday(DateTime input);
        
    }
}
