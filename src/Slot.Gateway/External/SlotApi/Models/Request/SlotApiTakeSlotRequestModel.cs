namespace Slot.Gateway.External.SlotApi.Models.Request
{
    public class SlotApiTakeSlotRequestModel
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Comments { get; set; }
        public SlotApiTakeSlotPatientRequestModel Patient { get; set; }
        public string FacilityId { get; set; }
    }

    public class SlotApiTakeSlotPatientRequestModel
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
