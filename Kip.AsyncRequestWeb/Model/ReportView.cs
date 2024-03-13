namespace Kip.AsyncReport.Model
{
    public class ReportView
    {
        public Guid Query { get; set; }

        public int Percent { get; set; }

        public ReportResultView? Result { get; set; }
    }
}
