using System.Text.Json.Serialization;

namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class ResultDto
    {
        [JsonPropertyName("roadCost")]
        public int RoadCost { get; set; }

        [JsonPropertyName("vertex")]
        public int Vertex { get; set; }

        [JsonPropertyName("calculatingTimeMs")]
        public long CalculatingTimeMs { get; set; }
    }
}