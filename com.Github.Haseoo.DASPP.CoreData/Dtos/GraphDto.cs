namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class GraphDto
    {
        public int[][] AdjMatrix { get; set; }
        public int this[int x, int y] => AdjMatrix[x][y];
        public int GraphSize { get => AdjMatrix[0].Length; }
    }
}