using System.Text.Json.Serialization;

namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class GraphDto
    {
        public int[][] AdjMatrix { get; set; }

        public int this[int x, int y] => AdjMatrix[x][y];

        [JsonIgnore]
        public int GraphSize => AdjMatrix?.Length ?? 0;
    }
}