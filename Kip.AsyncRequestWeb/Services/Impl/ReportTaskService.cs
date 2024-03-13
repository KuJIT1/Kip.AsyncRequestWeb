using Microsoft.EntityFrameworkCore;
using Kip.AsyncReport.Infrastructure;
using Kip.AsyncReport.Model;
using Microsoft.Extensions.Options;

namespace Kip.AsyncReport.Services.Impl
{
    // TODO: асинхронщина тут по большому счёту не нужна, полагаю
    public class ReportTaskService : IReportTaskService
    {
        private readonly ReportTaskContext _context;

        private readonly int _workingTime;

        private readonly TimeProvider _timeProvider;

        public ReportTaskService(ReportTaskContext context, IOptionsSnapshot<ReportTaskSettings> settings, TimeProvider timeProvider)
        {
            _context = context;
            _timeProvider = timeProvider;
            _workingTime = settings.Value?.WorkingTime ?? 60;
        }

        public async Task<ReportView?> FindReportAsync(Guid reportTaskId, CancellationToken token)
        {
            var reportTask = await _context.ReportTasks.Include(rt => rt.ReportResult).FirstOrDefaultAsync(rt => rt.Id == reportTaskId, token);
            if (reportTask == null)
            {
                return null;
            }

            // Для простоты не буду делать воркер, который меняет статусы и делает прочую работу, изменяя объект в БД
            if (reportTask.Status != Model.TaskStatus.Finished)
            {
                var reportInWork = _timeProvider.GetUtcNow().DateTime.Subtract(reportTask.DateCreate);
                if (reportInWork.TotalSeconds >= _workingTime)
                {
                    reportTask.Status = Model.TaskStatus.Finished;
                    reportTask.Percent = 100;
                    reportTask.ReportResult = new ReportResult()
                    {
                        CountSignIn = new Random().Next(22, 111),
                        UserId = reportTask.Request.UserId // Тут дублинрование информации, её нужно убрать
                    };
                }
                else
                {
                    reportTask.Status = Model.TaskStatus.Processed;
                    reportTask.Percent = Convert.ToInt32(reportInWork.TotalSeconds / _workingTime * 100);
                }

                await _context.SaveChangesAsync(token);
            }

            var reportView = new ReportView()
            {
                Percent = reportTask.Percent,
                Query = reportTask.Id
            };

            if (reportTask.Status == Model.TaskStatus.Finished)
            {
                reportView.Result = new ReportResultView()
                {
                    CountSignIn = reportTask.ReportResult!.CountSignIn,
                    UserId = reportTask.ReportResult.UserId
                };
            }

            return reportView;
        }

        async public Task<Guid> RegisterNewTaskAsync(ReportRequest reportRequest, CancellationToken token)
        {
            var newReportTask = new ReportTask() { Request = reportRequest, DateCreate = _timeProvider.GetUtcNow().DateTime };
            _context.ReportTasks.Add(newReportTask);
            await _context.SaveChangesAsync(token);
            return newReportTask.Id;
        }
    }
}
