using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kip.AsyncReport.Model
{
    public class ReportTask
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime DateCreate { get; set; }

        [Required]
        [DefaultValue(TaskStatus.Created)]
        public TaskStatus Status { get; set; }

        public int Percent { get; set; }

        [Required]
        public ReportRequest Request { get; set; }

        public ReportResult? ReportResult { get; set; }
    }
}
