using System.Text.Json.Serialization;

namespace SalesTracker.Web.Models
{
    public class ChartData
    {
        [JsonPropertyName("labels")]
        public List<string> Labels { get; set; } = new();

        [JsonPropertyName("datasets")]
        public List<ChartDataset> Datasets { get; set; } = new();

    }
}
