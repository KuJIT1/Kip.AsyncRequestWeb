using System.Text.Json.Serialization;

namespace Kip.AsyncReport.Model
{
    public class ReportResultView
    {
        [JsonPropertyName("user_id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("count_sign_in")]
        public int CountSignIn {  get; set; }
    }
}
