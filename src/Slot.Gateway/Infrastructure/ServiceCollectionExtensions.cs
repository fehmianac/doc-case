using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Slot.Gateway.Aggregator.Slot;
using Slot.Gateway.Service.Date;
using Slot.Gateway.Service.Json;
using Slot.Gateway.Service.Mapper;
using Slot.Gateway.Service.Slot;
using Slot.Gateway.Settings;
using ISlotApiHttpClient = Slot.Gateway.External.SlotApi.ISlotApiHttpClient;
using SlotApiHttpClient = Slot.Gateway.External.SlotApi.SlotApiHttpClient;

namespace Slot.Gateway.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSlotService(this IServiceCollection service, AppSettings appSettings)
        {
            service.AddHttpClient<ISlotApiHttpClient, SlotApiHttpClient>((config) =>
            {
                var authSchema = appSettings.Externals.SlotApi.Authorization.Split(" ");
                if (authSchema.Length < 2)
                {
                    return;
                }

                config.Timeout = TimeSpan.FromSeconds(appSettings.Externals.SlotApi.Timeout);
                config.BaseAddress = new Uri(appSettings.Externals.SlotApi.Url);
                config.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authSchema[0], authSchema[1]);
            });


            return service;
        }

        public static IServiceCollection AddAggregators(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISlotAggregator, SlotAggregator>();
            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IMapperService, MapperService>();
            serviceCollection.AddSingleton<IDateService, DateService>();
            serviceCollection.AddSingleton<ISlotService, SlotService>();
            serviceCollection.AddSingleton<ISerializationService, SerializationService>();
            return serviceCollection;
        }
    }
}