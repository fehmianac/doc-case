using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Slot.Gateway.External.SlotApi.Models.Request;
using Slot.Gateway.External.SlotApi.Models.Response;
using Slot.Gateway.Service.Json;

namespace Slot.Gateway.External.SlotApi
{
    public class SlotApiHttpClient : ISlotApiHttpClient
    {
        private readonly List<HttpStatusCode> _httpStatusCodesWorthRetryingForGet = new()
        {
            HttpStatusCode.RequestTimeout,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout,
        };

        private readonly List<HttpStatusCode> _httpStatusCodesWorthRetryingForPost = new()
        {
            HttpStatusCode.InternalServerError,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
        };


        private readonly HttpClient _httpClient;
        private readonly ISerializationService _serializationService;

        public SlotApiHttpClient(HttpClient httpClient, ISerializationService serializationService)
        {
            _httpClient = httpClient;
            _serializationService = serializationService;
        }

        public async Task<Base.ExternalApiResponse<SlotApiAvailableSlotResponseModel>> GetAvailableSlots(string mondayOfWeek, CancellationToken cancellationToken)
        {
            var route = $"GetWeeklyAvailability/{mondayOfWeek}";
            
            var httpResponse = await Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(r => _httpStatusCodesWorthRetryingForGet.Contains(r.StatusCode))
                .RetryAsync(3)
                .ExecuteAsync(async () => await _httpClient.GetAsync(route, cancellationToken));
            
            if (!httpResponse.IsSuccessStatusCode)
            {
                return new Base.ExternalApiResponse<SlotApiAvailableSlotResponseModel>
                {
                    ErrorMessage = await httpResponse.Content.ReadAsStringAsync(cancellationToken),
                    StatusCode = httpResponse.StatusCode
                };
            }

            var content = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            return new Base.ExternalApiResponse<SlotApiAvailableSlotResponseModel>
            {
                Data = _serializationService.Deserialize<SlotApiAvailableSlotResponseModel>(content),
                StatusCode = HttpStatusCode.OK
            };
        }

        public async Task<Base.ExternalApiResponse<bool>> TakeSlot(SlotApiTakeSlotRequestModel request, CancellationToken cancellationToken)
        {
            const string route = "TakeSlot";
            var content = new StringContent(_serializationService.Serialize(request), Encoding.UTF8, "application/json");

            var httpResponse = await Policy
                .Handle<Exception>()
                .OrResult<HttpResponseMessage>(r => _httpStatusCodesWorthRetryingForPost.Contains(r.StatusCode))
                .RetryAsync(3)
                .ExecuteAsync(async () => await _httpClient.PostAsync(route, content, cancellationToken));

            if (!httpResponse.IsSuccessStatusCode)
            {
                return new Base.ExternalApiResponse<bool>
                {
                    StatusCode = httpResponse.StatusCode,
                    Data = false,
                    ErrorMessage = await httpResponse.Content.ReadAsStringAsync(cancellationToken)
                };
            }

            return new Base.ExternalApiResponse<bool>
            {
                Data = true,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}