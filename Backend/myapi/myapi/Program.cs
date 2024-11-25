
using Microsoft.Extensions.Configuration;
using myapi.Model;
using myapi.Service;
using Serilog;

namespace myapi
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();

			//configure and use seriLog
			builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
			Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
			.Enrich.FromLogContext().CreateLogger();
			builder.Host.UseSerilog();


			builder.Services.AddScoped<IUserService, UserService>();

			builder.Services.Configure<StorageOptions<User>>(builder.Configuration.GetSection("StorageOptions:User"));

			//Assuming that there are multiple different data that need to be saved in the future,
			//you only need to simply inject this service
			//builder.Services.Configure<StorageOptions<Order>>(configuration.GetSection("StorageOptions:Order"));

			builder.Services.AddSingleton(typeof(IStorage<>), typeof(StorageService<>));


			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowSpecificOrigins",
					policy =>
					{
						policy.WithOrigins("http://localhost:4200")
							  .AllowAnyHeader()
							  .AllowAnyMethod();
					});

			});
			var app = builder.Build();


			//use my own exception handler to avoid exposing the exception details to the client
			app.UseMiddleware<MyExceptionHandler>();

			if (!app.Environment.IsDevelopment())
			{
				app.UseHsts();
			}

			app.UseCors("AllowSpecificOrigins");


			app.UseRouting();
			app.MapControllers();

			app.Run();
		}
	}
}
