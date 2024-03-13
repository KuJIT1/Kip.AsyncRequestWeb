using System.ComponentModel.DataAnnotations;

namespace Kip.AsyncReport.Model
{
    public class ReportResult
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public int CountSignIn { get; set; }
    }
}
