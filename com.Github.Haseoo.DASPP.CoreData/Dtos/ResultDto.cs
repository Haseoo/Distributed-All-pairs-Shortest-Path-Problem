﻿namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class ResultDto
    {
        public int RoadCost { get; set; }
        public int Vertex { get; set; }

        public ResultDto(int roadCost, int vertex)
        {
            RoadCost = roadCost;
            Vertex = vertex;
        }
    }
}