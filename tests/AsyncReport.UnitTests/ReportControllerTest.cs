using Kip.AsyncReport.Controllers;
using Kip.AsyncReport.Infrastructure;
using Kip.AsyncReport.Model;
using Kip.AsyncReport.Services.Impl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;

namespace Kip.AsyncReport.UnitTests
{
    public class ReportControllerTest
    {
        private readonly DbContextOptions<ReportTaskContext> _dbOptions;

        public ReportControllerTest()
        {
            _dbOptions = new DbContextOptionsBuilder<ReportTaskContext>()
                .UseInMemoryDatabase(databaseName: "in-memory")
            .Options;
        }

        [Fact]
        public async void User_statistic_success()
        {
            //Arrange
            var reportRequest = new ReportRequest()
            {
                UserId = Guid.NewGuid(),
                PeriodStart = DateTime.UtcNow,
                PeriodEnd = DateTime.UtcNow
            };

            var reportTaskContext = new ReportTaskContext(_dbOptions);
            var reportTaskSettings = new TestReportTaskSettings();
            var timeProvider = new FakeTimeProvider();
            var reportTaskService = new ReportTaskService(reportTaskContext, reportTaskSettings, timeProvider);

            //Act
            timeProvider.SetUtcNow(new DateTimeOffset(new DateTime(2022, 10, 10, 10, 10, 10)));

            var reportController = new ReportController(reportTaskContext, reportTaskService);
            var actionResult = await reportController.Statistic(reportRequest, default);

            //Asserts
            Assert.IsType<CreatedAtActionResult>(actionResult);
            var result = Assert.IsAssignableFrom<CreatedAtActionResult>(actionResult);
            Assert.IsType<Guid>(result.Value);
        }

        [Fact]
        public async void User_statistic_in_db_created()
        {
            //Arrange
            var reportRequest = new ReportRequest()
            {
                UserId = Guid.NewGuid(),
                PeriodStart = DateTime.UtcNow,
                PeriodEnd = DateTime.UtcNow
            };

            // InMemoryDB падает ошибки при попытке получить данные из неё. Не разобрался в чём проблема
            // Очевидно, так оставлять нельзя
            var opt2 = new DbContextOptionsBuilder<ReportTaskContext>()
                .UseSqlServer("Server=tcp:127.0.0.1,1433;Initial Catalog=master;User Id=sa;Password=Pass@word;TrustServerCertificate=True")
            .Options;

            var reportTaskContext = new ReportTaskContext(opt2 /*_dbOptions*/);
            var reportTaskSettings = new TestReportTaskSettings();
            var timeProvider = new FakeTimeProvider();
            var reportTaskService = new ReportTaskService(reportTaskContext, reportTaskSettings, timeProvider);


            //Act
            timeProvider.SetUtcNow(new DateTimeOffset(new DateTime(2022, 10, 10, 10, 10, 10)));

            var reportController = new ReportController(reportTaskContext, reportTaskService);
            var actionResult = await reportController.Statistic(reportRequest, default);

            //Asserts
            Assert.IsType<CreatedAtActionResult>(actionResult);
            var result = Assert.IsAssignableFrom<CreatedAtActionResult>(actionResult);
            var reportGuid = Assert.IsAssignableFrom<Guid>(result.Value);

            var createdTask = reportTaskContext.ReportTasks.FirstOrDefault(rt => rt.Id == reportGuid);
            Assert.NotNull(createdTask);
        }

        [Fact]
        public async void Statistic_not_found()
        {
            //Arrange

            // InMemoryDB падает ошибки при попытке получить данные из неё. Не разобрался в чём проблема
            // Очевидно, так оставлять нельзя
            var opt2 = new DbContextOptionsBuilder<ReportTaskContext>()
                .UseSqlServer("Server=tcp:127.0.0.1,1433;Initial Catalog=master;User Id=sa;Password=Pass@word;TrustServerCertificate=True")
            .Options;

            var reportTaskContext = new ReportTaskContext(opt2 /*_dbOptions*/);
            var reportTaskSettings = new TestReportTaskSettings();
            var timeProvider = new FakeTimeProvider();
            var reportTaskService = new ReportTaskService(reportTaskContext, reportTaskSettings, timeProvider);

            //Act
            timeProvider.SetUtcNow(new DateTimeOffset(new DateTime(2022, 10, 10, 10, 10, 10)));

            var reportController = new ReportController(reportTaskContext, reportTaskService);
            var actionResult = await reportController.Info(Guid.Empty, default);

            //Asserts
            Assert.IsType<NotFoundResult>(actionResult);
        }

        [Fact]
        public async void Statistic_in_50_percent()
        {
            //Arrange
            var dateCreate = new DateTime(2022, 10, 10, 10, 10, 10);
            var dateOffset = dateCreate.AddSeconds(10);
            var reportTaskId = Guid.NewGuid();
            var resultPercent = 50;

            // InMemoryDB падает ошибки при попытке получить данные из неё. Не разобрался в чём проблема
            // Очевидно, так оставлять нельзя
            var opt2 = new DbContextOptionsBuilder<ReportTaskContext>()
                .UseSqlServer("Server=tcp:127.0.0.1,1433;Initial Catalog=master;User Id=sa;Password=Pass@word;TrustServerCertificate=True")
            .Options;

            var reportTaskContext = new ReportTaskContext(opt2 /*_dbOptions*/);
            var reportTaskSettings = new TestReportTaskSettings();
            var timeProvider = new FakeTimeProvider();
            var reportTaskService = new ReportTaskService(reportTaskContext, reportTaskSettings, timeProvider);

            reportTaskContext.ReportTasks.Add(new ReportTask()
            {
                DateCreate = dateCreate,
                Id = reportTaskId,
                Request = new ReportRequest()
                {
                    PeriodEnd = DateTime.Now,
                    PeriodStart = DateTime.Now,
                    UserId = Guid.NewGuid()
                }
            });

            reportTaskContext.SaveChanges();

            //Act
            timeProvider.SetUtcNow(new DateTimeOffset(dateOffset));

            var reportController = new ReportController(reportTaskContext, reportTaskService);
            var actionResult = await reportController.Info(reportTaskId, default);

            //Asserts
            Assert.IsType<OkObjectResult>(actionResult);
            var result = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            var viewResult = Assert.IsAssignableFrom<ReportView>(result.Value);

            Assert.Equal(resultPercent, viewResult.Percent);
        }

        public class TestReportTaskSettings : IOptionsSnapshot<ReportTaskSettings>
        {
            public ReportTaskSettings Value => new ReportTaskSettings
            {
                WorkingTime = 20
            };

            public ReportTaskSettings Get(string name) => Value;
        }
    }

}
