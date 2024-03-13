using Microsoft.AspNetCore.Mvc;
using Kip.AsyncReport.Infrastructure;
using Kip.AsyncReport.Model;
using Kip.AsyncReport.Services;

namespace Kip.AsyncReport.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController: ControllerBase
    {
        private readonly ReportTaskContext _reportTaskContext;
        private readonly IReportTaskService _reportTaskService;

        public ReportController(ReportTaskContext reportTaskContext, IReportTaskService reportTaskService)
        {
            _reportTaskContext = reportTaskContext;
            _reportTaskService = reportTaskService;
        }

        [Route("user_statistics")]
        [HttpPost]
        public async Task<IActionResult> Statistic([FromBody] ReportRequest reportRequest, CancellationToken token)
        {
            // тут по идее должна быть валидация reportRequest. Точнее валидация должна быть в сервисе, а тут отлов и обработка ошибок валидации сервиса
            var requestGuid = await _reportTaskService.RegisterNewTaskAsync(reportRequest, token);

            return CreatedAtAction(nameof(Info), requestGuid);
        }

        [Route("info")]
        [HttpGet]
        public async Task<IActionResult> Info([FromQuery] Guid taskGuid, CancellationToken token)
        {
            var reportView = await _reportTaskService.FindReportAsync(taskGuid, token);
            if (reportView == null) 
            {
                return NotFound();
            }

            return Ok(reportView);
        }
    }
}
