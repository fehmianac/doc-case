using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using AutoFixture;
using AutoFixture.Xunit2;
using Moq;
using Slot.Gateway.Aggregator.Slot;
using Slot.Gateway.External.Base;
using Slot.Gateway.External.SlotApi;
using Slot.Gateway.External.SlotApi.Models.Response;
using Slot.Gateway.Service.Date;
using Slot.Gateway.Service.Mapper;
using Slot.Gateway.Service.Slot;
using Slot.Gateway.Service.Slot.Model;
using Xunit;

namespace Slot.Gateway.Tests.Aggregator.Slot
{
    public class GetSlotWeeklyAggregatorTest
    {
        private readonly Fixture _fixture;

        public GetSlotWeeklyAggregatorTest()
        {
            _fixture = new Fixture();
        }

        [Theory]
        [AutoData]
        public async Task ShouldReturnNull_WhenCallWithInvalidDateFormat(string date, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();
            var response = await slotAggregator.GetWeeklyAvailableSlots(date, cancellationToken);
            Assert.Null(response.Data);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [AutoData]
        public async Task ShouldReturnNull_WhenExternalApiFails(DateTime date, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();

            var apiResponseData = _fixture.Create<ExternalApiResponse<SlotApiAvailableSlotResponseModel>>();
            apiResponseData.StatusCode = HttpStatusCode.InternalServerError;

            var slotApiMock = mock.Mock<ISlotApiHttpClient>();
            slotApiMock.Setup(q => q.GetAvailableSlots(It.IsAny<string>(), cancellationToken)).ReturnsAsync(apiResponseData);

            var dateServiceMock = mock.Mock<IDateService>();
            var dateServiceResponse = new DateTime(2021, 06, 14);
            dateServiceMock.Setup(q => q.FindMondayOfWeek(date.Date)).Returns(dateServiceResponse);

            var response = await slotAggregator.GetWeeklyAvailableSlots(date.ToString("yyyyMMdd"), cancellationToken);

            slotApiMock.Verify(q => q.GetAvailableSlots(dateServiceResponse.ToString("yyyyMMdd"), cancellationToken), Times.Once);
            Assert.Null(response.Data);
            Assert.Equal(apiResponseData.ErrorMessage,response.ErrorMessage);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        [Theory]
        [AutoData]
        public async Task ShouldReturnSuccess(DateTime date, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();

            var slotApiMock = mock.Mock<ISlotApiHttpClient>();
            var dateServiceMock = mock.Mock<IDateService>();
            var mapperServiceMock = mock.Mock<IMapperService>();
            var slotServiceMock = mock.Mock<ISlotService>();


            var apiResponseData = _fixture.Create<ExternalApiResponse<SlotApiAvailableSlotResponseModel>>();

            slotApiMock.Setup(q => q.GetAvailableSlots(It.IsAny<string>(), cancellationToken)).ReturnsAsync(() =>
            {
                apiResponseData.StatusCode = HttpStatusCode.OK;
                return apiResponseData;
            });


            var dateServiceResponse = new DateTime(2021, 06, 14);
            dateServiceMock.Setup(q => q.FindMondayOfWeek(date.Date)).Returns(dateServiceResponse);
            dateServiceMock.Setup(q => q.FindNextMonday(dateServiceResponse)).Returns(DateTime.Now.Date);
            dateServiceMock.Setup(q => q.FindPrevMonday(dateServiceResponse)).Returns(DateTime.Now.Date);

            var calculateSlotRequest = _fixture.Create<CalculateSlotRequestModel>();
            mapperServiceMock.Setup(q => q.MapToCalculateSlotRequestModel(It.IsAny<WorkDay>(), It.IsAny<int>())).Returns(calculateSlotRequest);

            var slotResponseList = _fixture.Create<List<string>>();
            slotServiceMock.Setup(q => q.CalculateSlot(It.IsAny<CalculateSlotRequestModel>())).Returns(slotResponseList);

            var response = await slotAggregator.GetWeeklyAvailableSlots(date.ToString("yyyyMMdd"), cancellationToken);
            Assert.NotNull(response);
            Assert.Equal(apiResponseData.Data.Facility.Address,response.Data.Facility.Address);
            Assert.Equal(apiResponseData.Data.Facility.Name,response.Data.Facility.Name);
            Assert.Equal(apiResponseData.Data.Facility.FacilityId,response.Data.Facility.Id);
            Assert.Equal(7,response.Data.Items.Count);
            Assert.Equal(DateTime.Now.Date,response.Data.Today);
            Assert.Equal(DateTime.Now.Date,response.Data.NextMonday);
            Assert.Equal(DateTime.Now.Date,response.Data.PrevMonday);
            Assert.Equal(dateServiceResponse,response.Data.ThisMonday);
            Assert.Equal(apiResponseData.Data.SlotDurationMinutes,response.Data.SlotDurationMinutes);

            slotApiMock.Verify(q => q.GetAvailableSlots(dateServiceResponse.ToString("yyyyMMdd"), cancellationToken), Times.Once);
            mapperServiceMock.Verify(q => q.MapToCalculateSlotRequestModel(It.IsAny<WorkDay>(), It.IsAny<int>()), Times.Exactly(7));
            slotServiceMock.Verify(q => q.CalculateSlot(It.IsAny<CalculateSlotRequestModel>()), Times.Exactly(7));
            dateServiceMock.Verify(q => q.FindMondayOfWeek(It.IsAny<DateTime>()), Times.Once);
            dateServiceMock.Verify(q => q.FindPrevMonday(It.IsAny<DateTime>()), Times.Once);
            dateServiceMock.Verify(q => q.FindNextMonday(It.IsAny<DateTime>()), Times.Once);
        }
    }
}