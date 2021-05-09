namespace com.Github.Haseoo.DASPP.Main.Dtos
{
    public class MainTaskResponseDto
    {
        public int BestVertexIndex { get; set; }
        public int BestVertexRoadCost { get; set; }
        public int TotalTaskTimeMs { get; set; }
        public int CalculationTimeMs { get; set; }
        public int CommunicationTimeMs { get; set; }
    }
}