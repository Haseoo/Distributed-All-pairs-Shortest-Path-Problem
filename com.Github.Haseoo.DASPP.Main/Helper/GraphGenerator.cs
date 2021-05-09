using com.Github.Haseoo.DASPP.CoreData.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace com.Github.Haseoo.DASPP.Main.Helper
{
    public class GraphGenerator
    {
        private Random Random;
        private readonly int _numberOfNodes;
        private readonly int _maxWeight;
        private const int Density = 10;

        public GraphGenerator(int numberOfNodes, int maxWeight)
        {
            this.Random = new Random();
            this._numberOfNodes = numberOfNodes;
            this._maxWeight = maxWeight;
        }

        public GraphDto GenerateGraph()
        {
            var maxNumberOfEdges = _numberOfNodes / Density;
            var tmp = GetEmptyGraph(_numberOfNodes);
            var consistentGraph = GetConsistentGraph(tmp);
            var graphDto = new GraphDto
            {
                AdjMatrix = ConvertToMatrix(FillRestOfEdges(consistentGraph, maxNumberOfEdges))
            };
            return graphDto;
        }

        private List<Node> FillRestOfEdges(IList<Node> graph, int numberOfEdges)
        {
            var finalGraph = new List<Node>();
            while (graph.Count > 0)
            {
                var firstNode = graph[0];
                graph.Remove(firstNode);
                void FillEdgesForNode()
                {
                    var edgesToCreate = numberOfEdges - firstNode.Edges.Count;
                    for (var j = 0; j < edgesToCreate; j++)
                    {
                        Node secondNode;
                        do
                        {
                            if (graph.Count > edgesToCreate)
                            {
                                var secondNodeIndex = Random.Next(0, graph.Count);
                                secondNode = graph.ElementAtOrDefault(secondNodeIndex);
                            }
                            else
                            {
                                return;
                            }
                        } while (IsNodesAlreadyHaveEdge(firstNode, secondNode));
                        var edge = GenerateEdge(firstNode, secondNode);
                        firstNode.AddEdge(edge);
                        if (secondNode == null) continue;
                        secondNode.AddEdge(edge);
                        if (secondNode.Edges.Count != numberOfEdges) continue;
                        graph.Remove(secondNode);
                        finalGraph.Add(secondNode);
                    }
                }
                FillEdgesForNode();
                finalGraph.Add(firstNode);
            }
            return finalGraph;
        }

        private static bool IsNodesAlreadyHaveEdge(Node first, Node second)
        {
            return first.Edges.Any(edge => second.Edges.Contains(edge));
        }

        private Edge GenerateEdge(Node first, Node second)
        {
            var edge = new Edge
            {
                FirstNode = first.Index,
                SecondNode = second.Index,
                Weight = Random.Next(1, _maxWeight)
            };
            return edge;
        }

        private List<Node> GetConsistentGraph(IList<Node> tmp)
        {
            var random = new Random();
            var graph = new List<Node>();
            var currentNode = tmp[0];
            tmp.RemoveAt(0);
            while (tmp.Count > 0)
            {
                var nextNodeIndex = random.Next(0, tmp.Count);
                var nextNode = tmp[nextNodeIndex];
                tmp.RemoveAt(nextNodeIndex);
                var edge = GenerateEdge(currentNode, nextNode);
                currentNode.AddEdge(edge);
                nextNode.AddEdge(edge);
                graph.Add(currentNode);
                currentNode = nextNode;
            }
            graph.Add(currentNode);
            return graph;
        }

        private static List<Node> GetEmptyGraph(int numberOfNodes)
        {
            var graph = new List<Node>();
            for (var i = 1; i <= numberOfNodes; i++)
            {
                graph.Add(new Node(i));
            }
            return graph;
        }

        private int[][] ConvertToMatrix(List<Node> graph)
        {
            var graphMatrix = getTwoDimensionalArray(graph.Count);
            graph.ForEach(node =>
            {
                node.Edges.ForEach(edge =>
                {
                    var secondIndex = edge.FirstNode == node.Index ? edge.SecondNode : edge.FirstNode;
                    graphMatrix[node.Index - 1][secondIndex - 1] = edge.Weight;
                });
            });
            return graphMatrix;
        }

        private int[][] getTwoDimensionalArray(int size)
        {
            var array = new int[size][];
            for (var i = 0; i < size; i++)
            {
                var innerArray = new int[size];
                array[i] = innerArray;
            }
            return array;
        }
    }
}