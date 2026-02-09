using System.Text.Json.Serialization;

namespace SalesTracker.Web.Models
{
    public class ChartDataset
    {
        [JsonPropertyName("label")]
        public string Label { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public List<decimal> Data { get; set; } = new();

        [JsonPropertyName("backgroundColor")]
        public string BackgroundColor { get; set; } = string.Empty;

        [JsonPropertyName("borderColor")]
        public string? BorderColor { get; set; }
    }
}
