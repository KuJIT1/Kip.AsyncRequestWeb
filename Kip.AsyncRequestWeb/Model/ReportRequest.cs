using System.ComponentModel.DataAnnotations.Schema;

namespace Kip.AsyncReport.Model
{
    [ComplexType]
    public class ReportRequest
    {
        public Guid UserId { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime PeriodEnd { get; set; }
    }
}
