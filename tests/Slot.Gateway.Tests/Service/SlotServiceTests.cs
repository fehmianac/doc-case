using System;
using System.Collections.Generic;
using Slot.Gateway.Service.Slot;
using Slot.Gateway.Service.Slot.Model;
using Xunit;

namespace Slot.Gateway.Tests.Service
{
    public class SlotServiceTests
    {
        [Fact]
        public void ShouldReturnEmpty_WhenRequestIsNull()
        {
            var slotService = new SlotService();
            var slots = slotService.CalculateSlot(new CalculateSlotRequestModel(null, null, int.MaxValue));
            Assert.Empty(slots);
        }

        [Theory]
        [InlineData(12, 10, 1, 1)]
        [InlineData(12, 12, 1, 1)]
        public void ShouldReturnEmpty_WhenStartHourGreaterThanOrEqualEndHour(int startHour, int endHour, int lunchStart, int lunchEnd)
        {
            var slotService = new SlotService();
            var slots = slotService.CalculateSlot(new CalculateSlotRequestModel(new CalculateSlotWorkPeriodRequestModel(startHour, endHour, lunchStart, lunchEnd), null, int.MaxValue));
            Assert.Empty(slots);
        }


        [Theory]
        [InlineData(11, 13, 12, 13, 60)]
        [InlineData(14, 16, 14, 15, 60)]
        public void ShouldReturnWhenItem_WhenRemoveLunchBreak(int startHour, int endHour, int lunchStart, int lunchEnd, int slotMinute)
        {
            var slotService = new SlotService();
            var slots = slotService.CalculateSlot(new CalculateSlotRequestModel(new CalculateSlotWorkPeriodRequestModel(startHour, endHour, lunchStart, lunchEnd), null, slotMinute));
            Assert.Single(slots);
        }

        [Theory]
        [InlineData(13, 16, 15, 16, 60, 14, 15)]
        [InlineData(10, 13, 12, 13, 60, 11, 12)]
        public void ShouldReturnWhenItem_WhenRemoveBusySlots(int startHour, int endHour, int lunchStart, int lunchEnd, int slotMinute, int busyStart, int bustEnd)
        {
            var slotService = new SlotService();
            var slots = slotService.CalculateSlot(new CalculateSlotRequestModel(new CalculateSlotWorkPeriodRequestModel(startHour, endHour, lunchStart, lunchEnd), new List<CalculateSlotBusySlotsRequestModel>(new List<CalculateSlotBusySlotsRequestModel>()
            {
                new(DateTime.Now.Date.AddHours(busyStart), DateTime.Now.Date.AddHours(bustEnd))
            }), slotMinute));
            Assert.Single(slots);
        }
    }
}