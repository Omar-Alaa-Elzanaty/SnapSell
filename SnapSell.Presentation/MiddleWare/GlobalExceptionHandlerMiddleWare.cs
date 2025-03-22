using Azure;
using Microsoft.AspNetCore.Http;
using Serilog;
using SnapSell.Domain.Dtos.ResultDtos;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace SnapSell.Presentation.MiddleWare
{
    public class GlobalExceptionHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionHandlerMiddleWare(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;

                var errorId = Guid.NewGuid().ToString();

                _logger.ForContext("TraceIdentifier", errorId);

                if (context.User.FindFirstValue(ClaimTypes.NameIdentifier) is string userId)
                {
                    _logger.Fatal(e, $"Error for user {userId}");
                }
                else
                {
                    _logger.Fatal(e, e.Message);
                }

                var response = new Result<string>()
                {
                    Data = e.GetType().Name,
                    Message = $"Error occur during response, please try again or contact with support with ERORR CODE: {errorId}.",
                    StatusCode = ExceptionStatusCode(e)
                };

                var json = JsonSerializer.Serialize(response);
                context.Response.ContentType = "application/json";
                context?.Response.WriteAsync(json);
            }
        }
        private HttpStatusCode ExceptionStatusCode(Exception ex)
        {
            var exceptionType = ex.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                return HttpStatusCode.Unauthorized;
            }
            //else if (exceptionType == typeof(Shared.Exceptions.KeyNotFoundException))
            //{
            //    return HttpStatusCode.NotFound;
            //}
            //else if (exceptionType == typeof(SerivceErrorException))
            //{
            //    return HttpStatusCode.BadGateway;
            //}
            //else if (exceptionType == typeof(ServerErrorException))
            //{
            //    return HttpStatusCode.InternalServerError;
            //}
            else
            {
                return HttpStatusCode.InternalServerError;
            }
        }
    }
}
