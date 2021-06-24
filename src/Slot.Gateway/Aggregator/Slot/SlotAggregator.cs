using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Slot.Gateway.Aggregator.Models;
using Slot.Gateway.Aggregator.Slot.Models;
using Slot.Gateway.External.SlotApi;
using Slot.Gateway.External.SlotApi.Models.Response;
using Slot.Gateway.Service.Date;
using Slot.Gateway.Service.Mapper;
using Slot.Gateway.Service.Slot;
using Slot.Gateway.V1.Models.Slot;

namespace Slot.Gateway.Aggregator.Slot
{
    public class SlotAggregator : ISlotAggregator
    {
        private readonly ISlotApiHttpClient _slotApiHttpClient;
        private readonly IDateService _dateService;
        private readonly ISlotService _slotService;
        private readonly IMapperService _mapperService;

        public SlotAggregator(ISlotApiHttpClient slotApiHttpClient, IDateService dateService, ISlotService slotService, IMapperService mapperService)
        {
            _slotApiHttpClient = slotApiHttpClient;
            _dateService = dateService;
            _slotService = slotService;
            _mapperService = mapperService;
        }

        public async Task<AggregatorResponse<SlotWeeklyResponseModel>> GetWeeklyAvailableSlots(string date, CancellationToken cancellationToken)
        {
            var parseResult = DateTime.TryParseExact(date, "yyyyMMdd", DateTimeFormatInfo.CurrentInfo, DateTimeStyles.None, out var dateTime);
            if (!parseResult)
            {
                return new AggregatorResponse<SlotWeeklyResponseModel>
                {
                    ErrorMessage = "Invalid Date",
                    StatusCode = HttpStatusCode.BadRequest
                };
            }

            var mondayDate = _dateService.FindMondayOfWeek(dateTime);

            var slotHttpResponse = await _slotApiHttpClient.GetAvailableSlots(mondayDate.ToString("yyyyMMdd"), cancellationToken);

            if (!slotHttpResponse.IsSuccess)
            {
                return new AggregatorResponse<SlotWeeklyResponseModel>
                {
                    ErrorMessage = slotHttpResponse.ErrorMessage,
                    StatusCode = slotHttpResponse.StatusCode
                };
            }

            var slotApiResponse = slotHttpResponse.Data;
            var availableSlotItem = new List<WeeklySlotDayItemResponseModel>
            {
                FillSlotItem(slotApiResponse.Monday, slotApiResponse.SlotDurationMinutes, mondayDate, DayOfWeek.Monday),
                FillSlotItem(slotApiResponse.Tuesday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(1), DayOfWeek.Tuesday),
                FillSlotItem(slotApiResponse.Wednesday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(2), DayOfWeek.Wednesday),
                FillSlotItem(slotApiResponse.Thursday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(3), DayOfWeek.Thursday),
                FillSlotItem(slotApiResponse.Friday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(4), DayOfWeek.Friday),
                FillSlotItem(slotApiResponse.Saturday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(5), DayOfWeek.Saturday),
                FillSlotItem(slotApiResponse.Sunday, slotApiResponse.SlotDurationMinutes, mondayDate.AddDays(16), DayOfWeek.Sunday)
            };

            return new AggregatorResponse<SlotWeeklyResponseModel>
            {
                Data = new SlotWeeklyResponseModel
                {
                    Facility = new WeeklyFacilityResponseModel
                    {
                        Address = slotApiResponse.Facility.Address,
                        Id = slotApiResponse.Facility.FacilityId,
                        Name = slotApiResponse.Facility.Name
                    },
                    Items = availableSlotItem,
                    Today = DateTime.Now.Date,
                    ThisMonday = mondayDate,
                    NextMonday = _dateService.FindNextMonday(mondayDate),
                    PrevMonday = _dateService.FindPrevMonday(mondayDate),
                    SlotDurationMinutes = slotApiResponse.SlotDurationMinutes
                },
            };
        }

        public async Task<AggregatorResponse> TakeSlot(TakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            var mondayOfWeek = _dateService.FindMondayOfWeek(request.StartDate.Date);
            var slots = await _slotApiHttpClient.GetAvailableSlots(mondayOfWeek.ToString("yyyyMMdd"), cancellationToken);

            if (!slots.IsSuccess)
            {
                return new AggregatorResponse
                {
                    ErrorMessage = slots.ErrorMessage,
                    StatusCode = slots.StatusCode
                };
            }
            
            var requestModel = _mapperService.MapToSlotApiTakeRequestModel(request, slots.Data.SlotDurationMinutes);
            var response = await _slotApiHttpClient.TakeSlot(requestModel, cancellationToken);
            return new AggregatorResponse
            {
                ErrorMessage = response.ErrorMessage,
                StatusCode = response.StatusCode
            };
        }

        private WeeklySlotDayItemResponseModel FillSlotItem(WorkDay workDay, int slotMinutes, DateTime day, DayOfWeek dayOfWeek)
        {
            var calculateRequest = _mapperService.MapToCalculateSlotRequestModel(workDay, slotMinutes);
            var listOfSlot = _slotService.CalculateSlot(calculateRequest);

            return new WeeklySlotDayItemResponseModel
            {
                Date = day,
                Slots = listOfSlot,
                DayName = dayOfWeek.ToString()
            };
        }
    }
}