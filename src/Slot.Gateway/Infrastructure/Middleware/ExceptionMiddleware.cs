using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Serilog;
using Serilog.Context;

namespace Slot.Gateway.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await WriteLog(context, ex, (int) HttpStatusCode.InternalServerError);
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync("Server Internal Error");
            }
        }

        private async Task WriteLog(HttpContext context, Exception exception, int statusCode)
        {
            var request = context.Request;
            var requestPathAndQuery = request.GetEncodedPathAndQuery();
            var errorTemplate = $"HTTP {request.Method} {requestPathAndQuery} responded {statusCode}";

            using (LogContext.PushProperty("RequestHost", request.Host.Host))
            using (LogContext.PushProperty("RequestProtocol", request.Protocol))
            using (LogContext.PushProperty("RequestMethod", request.Method))
            using (LogContext.PushProperty("ResponseStatusCode", statusCode))
            using (LogContext.PushProperty("RequestPath", request.Path))
            using (LogContext.PushProperty("RequestPathAndQuery", requestPathAndQuery))
            using (LogContext.PushProperty("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => (object) h.Value.ToString()), true))
            using (LogContext.PushProperty("Exception", exception, true))
            {
                Log.Logger.Error(exception, errorTemplate);
            }

            await Task.FromResult(true);
        }
    }
}