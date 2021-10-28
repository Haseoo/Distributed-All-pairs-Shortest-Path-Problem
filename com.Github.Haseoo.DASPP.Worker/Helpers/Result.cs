﻿namespace com.Github.Haseoo.DASPP.Main.Helpers
{
    public class Result
    {
        public int RoadCost { get; set; }
        public int Vertex { get; set; }

        public Result(int roadCost, int vertex)
        {
            RoadCost = roadCost;
            Vertex = vertex;
        }
    }
}