using Core.CrossCuttingConcerns.Exceptions.Handlers;
using Core.CrossCuttingConcerns.Logging;
using Core.CrossCuttingConcerns.Serilog;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Core.CrossCuttingConcerns.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpExceptionHandler _httpExceptionHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LoggerServiceBase _loggerService;
        public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerService)
        {
            _httpExceptionHandler = new HttpExceptionHandler();
            _next = next;
            _httpContextAccessor = httpContextAccessor;
            _loggerService = loggerService;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await LogException(context, ex);
                await HandleExceptionAsync(context.Response, ex);
            }
        }

        private Task LogException(HttpContext context, Exception ex)
        {
            List<LogParameter> logParameters = new()
            {
                new LogParameter{Type=context.GetType().Name, Value=ex.ToString()}
            };
            LogDetailWithException logDetail = new()
            {
                ExceptionMessage=ex.Message,
                MethodName = _next.Method.Name,
                Parameters = logParameters,
                User = _httpContextAccessor.HttpContext?.User.Identity?.Name??"No User"
            };
            _loggerService.Error(JsonSerializer.Serialize(logDetail));

            return Task.CompletedTask;
        }

        private Task HandleExceptionAsync(HttpResponse response, Exception exception)
        {
            response.ContentType="application/json";
            _httpExceptionHandler.Response=response;
            return _httpExceptionHandler.HandleExceptionAsync(exception);
        }

    }
}
