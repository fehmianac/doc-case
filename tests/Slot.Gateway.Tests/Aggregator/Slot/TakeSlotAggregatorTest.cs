using System;
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
using Slot.Gateway.External.SlotApi.Models.Request;
using Slot.Gateway.External.SlotApi.Models.Response;
using Slot.Gateway.Service.Date;
using Slot.Gateway.Service.Mapper;
using Slot.Gateway.V1.Models.Slot;
using Xunit;

namespace Slot.Gateway.Tests.Aggregator.Slot
{
    public class TakeSlotAggregatorTest
    {
        private readonly Fixture _fixture;

        public TakeSlotAggregatorTest()
        {
            _fixture = new Fixture();
        }


        [Theory]
        [AutoData]
        public async Task ShouldReturnServerError_WhenExternalApiFailsGettingSlots(TakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();

            var apiResponseData = _fixture.Create<ExternalApiResponse<SlotApiAvailableSlotResponseModel>>();
            apiResponseData.StatusCode = HttpStatusCode.InternalServerError;

            var slotApiMock = mock.Mock<ISlotApiHttpClient>();
            slotApiMock.Setup(q => q.GetAvailableSlots(It.IsAny<string>(), cancellationToken)).ReturnsAsync(apiResponseData);

            var dateServiceMock = mock.Mock<IDateService>();
            var dateServiceResponse = new DateTime(2021, 06, 14);
            dateServiceMock.Setup(q => q.FindMondayOfWeek(request.StartDate.Date)).Returns(dateServiceResponse);

            var response = await slotAggregator.TakeSlot(request, cancellationToken);

            slotApiMock.Verify(q => q.GetAvailableSlots(dateServiceResponse.ToString("yyyyMMdd"), cancellationToken), Times.Once);
            dateServiceMock.Verify(q => q.FindMondayOfWeek(request.StartDate.Date), Times.Once);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        [Theory]
        [AutoData]
        public async Task ShouldReturnServerError_WhenExternalApiFailsTakingSlots(TakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();

            var getSlotApiResponseData = _fixture.Create<ExternalApiResponse<SlotApiAvailableSlotResponseModel>>();
            getSlotApiResponseData.StatusCode = HttpStatusCode.OK;

            var mapperResponseData = _fixture.Create<SlotApiTakeSlotRequestModel>();
            var mapperMock = mock.Mock<IMapperService>();
            mapperMock.Setup(q => q.MapToSlotApiTakeRequestModel(request, getSlotApiResponseData.Data.SlotDurationMinutes)).Returns(mapperResponseData);

            var slotApiMock = mock.Mock<ISlotApiHttpClient>();
            slotApiMock.Setup(q => q.GetAvailableSlots(It.IsAny<string>(), cancellationToken)).ReturnsAsync(getSlotApiResponseData);

            var takeSlotApiResponseData = _fixture.Create<ExternalApiResponse<bool>>();
            takeSlotApiResponseData.StatusCode = HttpStatusCode.InternalServerError;
            slotApiMock.Setup(q => q.TakeSlot(mapperResponseData, cancellationToken)).ReturnsAsync(takeSlotApiResponseData);

            var dateServiceMock = mock.Mock<IDateService>();
            var dateServiceResponse = new DateTime(2021, 06, 14);
            dateServiceMock.Setup(q => q.FindMondayOfWeek(request.StartDate.Date)).Returns(dateServiceResponse);

            var response = await slotAggregator.TakeSlot(request, cancellationToken);

            slotApiMock.Verify(q => q.GetAvailableSlots(dateServiceResponse.ToString("yyyyMMdd"), cancellationToken), Times.Once);
            slotApiMock.Verify(q => q.TakeSlot(mapperResponseData, cancellationToken), Times.Once);
            dateServiceMock.Verify(q => q.FindMondayOfWeek(request.StartDate.Date), Times.Once);
            mapperMock.Verify(q => q.MapToSlotApiTakeRequestModel(request, getSlotApiResponseData.Data.SlotDurationMinutes), Times.Once);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.Equal(takeSlotApiResponseData.ErrorMessage,response.ErrorMessage);
            Assert.False(response.IsSuccess);
        }
        
        
        
        [Theory]
        [AutoData]
        public async Task ShouldReturnSucces(TakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            var mock = AutoMock.GetStrict();
            var slotAggregator = mock.Create<SlotAggregator>();

            var getSlotApiResponseData = _fixture.Create<ExternalApiResponse<SlotApiAvailableSlotResponseModel>>();
            getSlotApiResponseData.StatusCode = HttpStatusCode.OK;

            var mapperResponseData = _fixture.Create<SlotApiTakeSlotRequestModel>();
            var mapperMock = mock.Mock<IMapperService>();
            mapperMock.Setup(q => q.MapToSlotApiTakeRequestModel(request, getSlotApiResponseData.Data.SlotDurationMinutes)).Returns(mapperResponseData);

            var slotApiMock = mock.Mock<ISlotApiHttpClient>();
            slotApiMock.Setup(q => q.GetAvailableSlots(It.IsAny<string>(), cancellationToken)).ReturnsAsync(getSlotApiResponseData);

            var takeSlotApiResponseData = _fixture.Create<ExternalApiResponse<bool>>();
            takeSlotApiResponseData.StatusCode = HttpStatusCode.OK;
            slotApiMock.Setup(q => q.TakeSlot(mapperResponseData, cancellationToken)).ReturnsAsync(takeSlotApiResponseData);

            var dateServiceMock = mock.Mock<IDateService>();
            var dateServiceResponse = new DateTime(2021, 06, 14);
            dateServiceMock.Setup(q => q.FindMondayOfWeek(request.StartDate.Date)).Returns(dateServiceResponse);

            var response = await slotAggregator.TakeSlot(request, cancellationToken);

            slotApiMock.Verify(q => q.GetAvailableSlots(dateServiceResponse.ToString("yyyyMMdd"), cancellationToken), Times.Once);
            slotApiMock.Verify(q => q.TakeSlot(mapperResponseData, cancellationToken), Times.Once);
            dateServiceMock.Verify(q => q.FindMondayOfWeek(request.StartDate.Date), Times.Once);
            mapperMock.Verify(q => q.MapToSlotApiTakeRequestModel(request, getSlotApiResponseData.Data.SlotDurationMinutes), Times.Once);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}