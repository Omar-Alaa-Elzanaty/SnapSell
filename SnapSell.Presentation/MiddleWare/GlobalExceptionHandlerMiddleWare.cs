using Azure;
using Microsoft.AspNetCore.Http;
using SnapSell.Domain.Extnesions;
using SnapSell.Domain.ResultDtos;
using System.Net;
using System.Text.Json;

namespace SnapSell.Presentation.MiddleWare
{
    public class GlobalExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        public GlobalExceptionHandlerMiddleWare(RequestDelegate next)
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

                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new Result<object>
            {
                Message = ex.Message,
                StatusCode = (HttpStatusCode)context.Response.StatusCode
            };
            var jsonOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var exceptionResult = JsonSerializer.Serialize(response, jsonOptions);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(exceptionResult);
        }
    }
}
