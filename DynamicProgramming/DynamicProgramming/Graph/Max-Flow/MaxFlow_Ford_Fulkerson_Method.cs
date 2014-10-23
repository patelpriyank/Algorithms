using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.Graph
{
    public class MaxFlow_Ford_Fulkerson_Method
    {
        // Returns tne maximum flow from s to t in the given graph
        public int MaxFlowProblem_FordFulkersonMethod(int[,] originalGraph, int start, int end)
        {
            int[,] residulaGraph = originalGraph;
            int max_flow = 0;
            do
            {
                var results = bfs(residulaGraph, start, end);
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

        private Tuple<bool, Dictionary<int, int>> bfs(int[,] residualGraph, int start, int end)
        {
            bool pathExists = false;
            var visited = new List<int>();
            var parents = new Dictionary<int, int>();

            var queue = new Queue<int>();
            queue.Enqueue(start);
            visited.Add(start);
            parents.Add(start, -1);

            while (queue.Count > 0)
            {
                int currentNode = queue.Dequeue();

                for (int i = 0; i < residualGraph.GetLength(0) ; i++)
                {
                    if (!visited.Contains(i) && residualGraph[currentNode, i] > 0)
                    {
                        visited.Add(i);
                        parents.Add(i, currentNode);
                        queue.Enqueue(i);
                    }
                }
            }

            if (visited.Contains(end))
                pathExists = true;

            return new Tuple<bool, Dictionary<int, int>>(pathExists, parents);
        }
    }
}
