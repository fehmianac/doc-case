using System.Linq;
using Slot.Gateway.Service.Slot.Model;
using Slot.Gateway.V1.Models.Slot;
using SlotApiTakeSlotPatientRequestModel = Slot.Gateway.External.SlotApi.Models.Request.SlotApiTakeSlotPatientRequestModel;
using SlotApiTakeSlotRequestModel = Slot.Gateway.External.SlotApi.Models.Request.SlotApiTakeSlotRequestModel;
using WorkDay = Slot.Gateway.External.SlotApi.Models.Response.WorkDay;

namespace Slot.Gateway.Service.Mapper
{
    public class MapperService : IMapperService
    {
        public CalculateSlotRequestModel MapToCalculateSlotRequestModel(WorkDay workDay, int slotDuration)
        {
            if (workDay?.WorkPeriod == null)
                return new CalculateSlotRequestModel(null, null, slotDuration);

            var busySlots = workDay.BusySlots?.Select(q => new CalculateSlotBusySlotsRequestModel(q.Start, q.End)).ToList();
            return new CalculateSlotRequestModel(new CalculateSlotWorkPeriodRequestModel(
                    workDay.WorkPeriod.StartHour,
                    workDay.WorkPeriod.EndHour,
                    workDay.WorkPeriod.LunchStartHour,
                    workDay.WorkPeriod.LunchEndHour),
                busySlots,
                slotDuration
            );
        }

        public SlotApiTakeSlotRequestModel MapToSlotApiTakeRequestModel(TakeSlotRequestModel request,int slotDuration)
        {
            return new()
            {
                FacilityId = request.FacilityId,
                Comments = request.Comments,
                Start = request.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                End = request.StartDate.AddMinutes(slotDuration).ToString("yyyy-MM-dd HH:mm:ss"),
                Patient = new SlotApiTakeSlotPatientRequestModel
                {
                    Email = request.Patient.Email,
                    Name = request.Patient.Name,
                    Phone = request.Patient.Phone,
                    SecondName = request.Patient.SecondName
                }
            };
        }
    }
}