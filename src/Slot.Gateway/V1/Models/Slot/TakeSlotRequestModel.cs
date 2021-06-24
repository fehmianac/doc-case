using System;

namespace Slot.Gateway.V1.Models.Slot
{
    public class TakeSlotRequestModel
    {
        public string FacilityId { get; set; }
        public DateTime StartDate { get; set; }
        public string Comments { get; set; }
        public TakeSlotPatientRequestModel Patient { get; set; }
    }

    public class TakeSlotPatientRequestModel
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}