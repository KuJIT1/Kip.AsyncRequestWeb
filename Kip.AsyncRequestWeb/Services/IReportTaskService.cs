using Kip.AsyncReport.Model;

namespace Kip.AsyncReport.Services
{
    public interface IReportTaskService
    {
        Task<Guid> RegisterNewTaskAsync(ReportRequest reportRequest, CancellationToken token);

        Task<ReportView?> FindReportAsync(Guid reportTaskId, CancellationToken token);
    }
}
