using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.Graph
{
    public class MaxFlow_Ford_Fulkerson_DijkstraHeap
    {
        // Returns tne maximum flow from s to t in the given graph
        public int MaxFlowProblem_FordFulkerson_DijkstraHeap(int[,] originalGraph, int start, int end)
        {
            int[,] residulaGraph = originalGraph;
            int max_flow = 0;
            do
            {
                var results = DijkstraHeap(residulaGraph, start, end);
                bool pathExists = results.Item1;
                
                if (!pathExists) break;

                Dictionary<int, int> parents = results.Item2;

                //find the minimum path flow
                int minPathFlow = int.MaxValue;
                int currentNode = end;
                int previousNode = parents[currentNode];
                while (currentNode != start)
                {
                    minPathFlow = Math.Min(residulaGraph[previousNode, currentNode], minPathFlow);
                    currentNode = previousNode;
                    previousNode = parents[currentNode];
                }

                //increase flow of current augmenting path in the residual network by this minPathFlow.
                currentNode = end;
                previousNode = parents[currentNode];
                while (currentNode != start)
                {
                    residulaGraph[previousNode, currentNode] -= minPathFlow;
                    residulaGraph[currentNode, previousNode] += minPathFlow;

                    currentNode = previousNode;
                    previousNode = parents[currentNode];
                }

                // Add path flow to overall flow
                max_flow += minPathFlow;

            } while (true);

            return max_flow;
        }

        class Node
        {
            public int ID { get; set; }
            public int MinPathFlow { get; set; }
            public int Parent { get; set; }

            public Node(int id, int minPathFlow, int parent)
            {
                ID = id;
                MinPathFlow = minPathFlow;
                Parent = parent;
            }
        }

        class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                //y > x => -1
                //x > y => 1
                //x == y => 0

                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're 
                        // equal.  
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y 
                        // is greater.  
                        return -1;
                    }
                }
                else
                {
                    // If x is not null... 
                    // 
                    if (y == null)
                    // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, compare the  
                        // lengths of the two strings. 
                        // 
                        if (x.MinPathFlow > y.MinPathFlow) return 1;
                        if (x.MinPathFlow < y.MinPathFlow) return -1;
                        else return 0;
                    }
                }
            }
        }

        private Tuple<bool, Dictionary<int, int>> DijkstraHeap(int[,] residualGraph, int start, int end)
        {
            bool pathExists = false;
            var visited = new List<int>();
            var parents = new Dictionary<int, int>();

            var queue = new Queue<Node>();
            queue.Enqueue(new Node(start, int.MaxValue, -1));
            visited.Add(start);
            parents.Add(start, -1);

            int minFlow = int.MaxValue;
            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                for (int next = 0; next < residualGraph.GetLength(0) ; next++)
                {
                    if (!visited.Contains(next) && residualGraph[currentNode.ID, next] > 0)
                    {
                        visited.Add(next);
                        parents.Add(next, currentNode.ID);

                        if (next == end)
                        {
                            pathExists = true;
                            break;
                        }
                        minFlow = Math.Min(residualGraph[currentNode.ID, next], minFlow);
                        queue.Enqueue(new Node(next, minFlow, currentNode.ID));
                    }
                }
            }
            
            return new Tuple<bool, Dictionary<int, int>>(pathExists, parents);
        }
    }
}
