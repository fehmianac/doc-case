using System;
using AutoFixture.Xunit2;
using Slot.Gateway.External.SlotApi.Models.Response;
using Slot.Gateway.Service.Mapper;
using Slot.Gateway.V1.Models.Slot;
using Xunit;

namespace Slot.Gateway.Tests.Service
{
    public class MapperServiceTests
    {
        [Fact]
        public void MapToCalculateSlotRequestModel_ShouldNull_WhenWorkDayIsNull()
        {
            var mapperService = new MapperService();
            var response = mapperService.MapToCalculateSlotRequestModel(null, 1);
            Assert.Null(response.WorkPeriod);
            Assert.Equal(1, response.SlotDurationMinutes);
        }

        [Theory]
        [AutoData]
        public void MapToCalculateSlotRequestModel_ShouldBusySlotNull_WhenBusySlotIsNull(WorkDay workDay, int slotDurationMinutes)
        {
            workDay.BusySlots = null;
            var mapperService = new MapperService();
            var response = mapperService.MapToCalculateSlotRequestModel(workDay, slotDurationMinutes);

            Assert.Null(response.BusySlots);
            Assert.Equal(slotDurationMinutes, response.SlotDurationMinutes);
            Assert.Equal(workDay.WorkPeriod.EndHour, response.WorkPeriod.EndHour);
            Assert.Equal(workDay.WorkPeriod.StartHour, response.WorkPeriod.StartHour);
            Assert.Equal(workDay.WorkPeriod.LunchStartHour, response.WorkPeriod.LunchStartHour);
            Assert.Equal(workDay.WorkPeriod.LunchEndHour, response.WorkPeriod.LunchEndHour);
        }

        [Theory]
        [AutoData]
        public void MapToCalculateSlotRequestModel_ShouldSuccess(WorkDay workDay, int slotDurationMinutes)
        {
            var mapperService = new MapperService();
            var response = mapperService.MapToCalculateSlotRequestModel(workDay, slotDurationMinutes);

            Assert.Equal(workDay.BusySlots.Count, response.BusySlots.Count);
            Assert.Equal(slotDurationMinutes, response.SlotDurationMinutes);
            Assert.Equal(workDay.WorkPeriod.EndHour, response.WorkPeriod.EndHour);
            Assert.Equal(workDay.WorkPeriod.StartHour, response.WorkPeriod.StartHour);
            Assert.Equal(workDay.WorkPeriod.LunchStartHour, response.WorkPeriod.LunchStartHour);
            Assert.Equal(workDay.WorkPeriod.LunchEndHour, response.WorkPeriod.LunchEndHour);
        }
        
        [Theory]
        [AutoData]
        public void MapToSlotApiTakeRequestModel_ShouldSuccess(TakeSlotRequestModel request, int slotDurationMinutes)
        {
            var mapperService = new MapperService();
            var response = mapperService.MapToSlotApiTakeRequestModel(request, slotDurationMinutes);


            Assert.Equal(request.FacilityId, response.FacilityId);
            Assert.Equal(request.Comments, response.Comments);
            Assert.Equal(request.Patient.Email, response.Patient.Email);
            Assert.Equal(request.Patient.Phone, response.Patient.Phone);
            Assert.Equal(request.Patient.SecondName, response.Patient.SecondName);
            Assert.Equal(request.Patient.Name, response.Patient.Name);
            var exceptedEndDate =request.StartDate.AddMinutes(slotDurationMinutes);
            Assert.Equal(exceptedEndDate.ToString("yyyy-MM-dd HH:mm:ss"), response.End);
            Assert.Equal(request.StartDate.ToString("yyyy-MM-dd HH:mm:ss"), response.Start);

        }

    }
}