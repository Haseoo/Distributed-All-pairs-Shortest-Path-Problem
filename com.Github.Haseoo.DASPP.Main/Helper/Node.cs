using System.Collections.Generic;

namespace com.Github.Haseoo.DASPP.Main.Helper
{
    public class Node
    {
        public int Index { get; set; }

        public List<Edge> Edges { get; }

        public Node(int index)
        {
            this.Index = index;
            this.Edges = new List<Edge>();
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }
    }
}