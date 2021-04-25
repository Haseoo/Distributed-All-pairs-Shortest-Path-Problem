using System;
using System.Collections.Generic;
using System.Text;

namespace com.Github.Haseoo.DASPP.CoreData.Dtos
{
    public class GraphDto
    {
        public int[,] AdjMatrix { get; set; }
        public int this[int x, int y]
        {
            get => AdjMatrix[x, y];
        }
        public int GraphSize { get => AdjMatrix.GetLength(0); }
    }
}
