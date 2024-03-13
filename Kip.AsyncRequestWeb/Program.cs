using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Kip.AsyncReport.Infrastructure;
using Kip.AsyncReport.Services;
using Kip.AsyncReport.Services.Impl;

namespace Kip.AsyncReport
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddCustomDbContext(builder.Configuration);
            builder.Services.Configure<ReportTaskSettings>(builder.Configuration);
            builder.Services.AddTransient<IReportTaskService, ReportTaskService>();
            builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();


            //       app.UseAuthorization();


            app.MapControllers();

            using(var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ReportTaskContext>()!;
                context.Database.Migrate();
            }

            app.Run();
        }
    }

    public static class CustomExtetionMethods
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ReportTaskContext>(options =>
                {
                    options.UseSqlServer(configuration["ConnectionString"],
                                         sqlServerOptionsAction: sqlOptions =>
                                         {
                                             sqlOptions.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
                                         });
                });
            return services;
        }
    }
}
