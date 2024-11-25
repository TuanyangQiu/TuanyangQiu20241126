using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class MyExceptionHandler
{
	private readonly RequestDelegate _next;
	private readonly ILogger<MyExceptionHandler> _logger;

	public MyExceptionHandler(
		RequestDelegate next,
		ILogger<MyExceptionHandler> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An unhandled exception : {Method} {Url}", context.Request.Method, context.Request.Path);

			await HandleExceptionAsync(context);
		}
	}

	private static Task HandleExceptionAsync(HttpContext context)
	{
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		context.Response.ContentType = "application/json";

		var response = new
		{
			message = "An error occurred while processing your request. Please contact our administrator."
		};

		return context.Response.WriteAsJsonAsync(response);
	}
}
