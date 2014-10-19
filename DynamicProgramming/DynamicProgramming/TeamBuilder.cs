using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    public class TeamBuilder
    {
        public int[] SpecialLocations(String[] paths)
        {
            uint[,] adj = new uint[paths.Length, paths.Length];

            //initialize
            for (int i = 0; i < paths.Length; i++)
                for (int j = 0; j < paths.Length; j++)
                {
                    if (i == j)
                        adj[i, j] = 0;

                    adj[i, j] = uint.MaxValue;

                    if (paths[i].ToCharArray()[j] != '0')
                        adj[i, j] = uint.Parse(paths[i].ToCharArray()[j].ToString());
                }

            //Floyd­-Warshall algorithm
            for (int k = 0; k < paths.Length; k++)
                for (int i = 0; i < paths.Length; i++)
                    for (int j = 0; j < paths.Length; j++)
                    {
                        adj[i, j] = Math.Min(adj[i, j], adj[i, k] + adj[k, j]);
                    }

            int[] incomingPaths = new int[paths.Length];
            int[] outgoingPaths = new int[paths.Length];
            int incomingPathsCounter = 0;
            int outgoingPathsCounter = 0;
            for (int i = 0; i < paths.Length; i++)
            {
                int counter = 0;
                for (int j = 0; j < paths.Length; j++)
                {
                    if(i == j) continue;

                    //if there is a route between these two nodes - direct or indirect
                    if (adj[i, j] >= 1 && adj[i, j] <= paths.Length-1) 
                    {
                        counter++;
                        incomingPaths[j]++;
                        if (incomingPaths[j] == paths.Length-1)
                            incomingPathsCounter++;
                    }
                }

                if (counter == paths.Length-1)
                    outgoingPathsCounter++;
            }

            return new int[]{outgoingPathsCounter, incomingPathsCounter};
        }
    }
}
