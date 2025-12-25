using Dapper;
using LoggerService;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using ViewModel;

namespace Rentelligence.AI.Middleware
{
    public static class HeaderExtensions
    {
        public static string ToJson(this IHeaderDictionary headers)
        {
            return JsonConvert.SerializeObject(headers.ToDictionary(h => h.Key, h => h.Value.ToString()));
        }
    }
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger, IHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _logger = logger;
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var log = new ApiLogs
            {
                CreatedOn = DateTime.UtcNow.ToString(),
                RequestMethod = context.Request.Method,
                RequestUrl = context.Request.Path,
                RequestHeaders = context.Request.Headers.ToJson(),
                UserId = context.User.Identity.IsAuthenticated ? int.Parse(context.User.Identity.Name) : 0,
                ClientIpAddress = context.Connection.RemoteIpAddress?.ToString(),
                PageUrl = context.Request.Path,
                RequestSource = "Web",
                OrgId = 1
            };

            context.Request.EnableBuffering();
            using (var reader = new StreamReader(context.Request.Body, leaveOpen: true))
            {
                log.RequestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalResponseBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    log.ExceptionMessage = ex.Message;
                    log.ExceptionStackTrace = ex.StackTrace;
                    await SaveLogAsync(log);

                }
                finally
                {
                    log.ResponseStatusCode = context.Response.StatusCode;
                    log.ResponseHeaders = context.Response.Headers.ToJson();

                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    log.ResponseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                    context.Response.Body.Seek(0, SeekOrigin.Begin);

                    await responseBody.CopyToAsync(originalResponseBodyStream);
                }
            }
        }

        private async Task SaveLogAsync(ApiLogs log)
        {
            var connectionString = _configuration.GetConnectionString("DbCon");
            using (var connection = new SqlConnection(connectionString))
            {
                var query = @"
                INSERT INTO ApiLogs (
                    RequestUrl, RequestMethod, RequestHeaders, RequestBody, ResponseStatusCode, ResponseHeaders, ResponseBody, ExceptionMessage, ExceptionStackTrace, UserId, ClientIpAddress, PageUrl, RequestSource, OrgId, CreatedOn
                )
                VALUES (
                    @RequestUrl, @RequestMethod, @RequestHeaders, @RequestBody, @ResponseStatusCode, @ResponseHeaders, @ResponseBody, @ExceptionMessage, @ExceptionStackTrace, @UserId, @ClientIpAddress, @PageUrl, @RequestSource, @OrgId, @CreatedOn
                )";

                await connection.ExecuteAsync(query, log);
            }
        }
    }
}
