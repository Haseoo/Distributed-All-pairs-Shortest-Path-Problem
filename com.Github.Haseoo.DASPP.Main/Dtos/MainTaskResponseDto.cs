namespace com.Github.Haseoo.DASPP.Main.Dtos
{
    public class MainTaskResponseDto
    {
        public int BestVertexIndex { get; set; }
        public int BestVertexRoadCost { get; set; }
        public long TotalTaskTimeMs { get; set; }
        public long CalculationTimeMs { get; set; }
        public long CommunicationTimeMs { get; set; }
    }
}