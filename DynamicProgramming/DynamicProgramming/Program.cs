using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Graph;
using DynamicProgramming.Graph.MaxBipartite_Problems;
using DynamicProgramming.Greedy;
using DynamicProgramming.Helpers;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            /* <1> Dynamic programming algorithmic technique 
             * var badNeighbor = new BadNeighbors();
             var donations = new int[] { 10, 3, 2, 5, 7, 8 };
             int result = badNeighbor.MaxDonations(donations);
             Console.WriteLine("Max donation = " + result);*/

            /* <2> Dynamic programming algorithmic technique
            //flowergarden
            var heights = new int[] { 239, 878, 182, 109, 510, 581, 643, 402, 470, 849, 196, 707, 810, 913, 758, 261, 451, 240, 624, 60, 671, 363, 300, 40, 22, 229, 755, 82, 8, 949, 906, 411, 17, 566 };
            var blooms = new int[] { 296, 136, 306, 94, 345, 37, 11, 133, 269, 276, 199, 348, 277, 324, 146, 252, 52, 138, 37, 5, 231, 314, 230, 138, 202, 340, 235, 58, 99, 123, 272, 67, 48, 277 }; //{ 1, 5, 10, 15, 20 };
            var wilts = new int[] { 301, 317, 339, 271, 361, 148, 117, 291, 357, 331, 282, 354, 342, 341, 178, 261, 158, 357, 177, 208, 323, 336, 256, 182, 228, 342, 326, 236, 358, 315, 325, 200, 358, 341 }; //{ 5, 10, 14, 20, 25 };
                       
            var result = new FlowerGarden_SelectionSort().Arrangement(heights, blooms, wilts);
*/

            /* <3> Dijkstra
             * //Kiloman Dijkstra Heap
               var kiloManX = new DijkstraHeap2();
               string[] damageChart = new string[] { "1542", "7935", "1139", "8882" };
               int[] bossHealth = new int[] { 150, 150, 150, 150 };

               kiloManX.LeastShots(damageChart, bossHealth);*/


            /* <4> Dijkstra
             //IslandFerry
             Dijkstra_IslandFerries islandFerries = new Dijkstra_IslandFerries();
             String[] legs = new[] { "0-1 0-3", "0-2" };
             String[] prices = new[] { "5 7", "1000 1000", "1000 1000", "1000 1000" };
             string allRoutes = islandFerries.TravelCheap(legs, prices);
             Console.WriteLine(allRoutes);*/

            /* <5> Floyd­-Warshall algorithm
             var teamBuilder = new TeamBuilder();
             var paths = new[] { "0110000", "1000100", "0000001", "0010000", "0110000", "1000010", "0001000" };
             int[] results = teamBuilder.SpecialLocations(paths);
             Console.WriteLine(Helper.Print(results));*/

           /* // <6> 
            //Greddy
            var boxingCredit = new BoxingCredit();
            int[] a = new[] { 100, 200, 300, 1200, 6000 };
            int[] b = new int[0];
            int[] c = new[] { 900, 902, 1200, 4000, 5000, 6001 };
            int[] d = new[] { 0, 2000, 6002 };
            int[] e = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            int result = boxingCredit.MaxCredit(a, b, c, d, e);*/

            /*//<7> Max-Flow problem with Breath-first search
            var graph = new int[,] { {0, 16, 13, 0, 0, 0},
                {0, 0, 10, 12, 0, 0},
                {0, 4, 0, 0, 14, 0},
                {0, 0, 9, 0, 0, 20},
                {0, 0, 0, 7, 0, 4},
                {0, 0, 0, 0, 0, 0}
            };
            var fordFulkersonAlgo = new MaxFlow_Ford_Fulkerson_Method();
            int maxFlow = fordFulkersonAlgo.MaxFlowProblem_FordFulkersonMethod(graph,0,5);
            Console.WriteLine("Max flow = " + maxFlow);*/

            /*//<8> Max-Flow problem with Dijkstra Heap (priority queue) algorithm
            var graph = new int[,] { {0, 16, 13, 0, 0, 0},
                {0, 0, 10, 12, 0, 0},
                {0, 4, 0, 0, 14, 0},
                {0, 0, 9, 0, 0, 20},
                {0, 0, 0, 7, 0, 4},
                {0, 0, 0, 0, 0, 0}
            };
            var fordFulkersonAlgo = new MaxFlow_Ford_Fulkerson_DijkstraHeap();
            int maxFlow = fordFulkersonAlgo.MaxFlowProblem_FordFulkerson_DijkstraHeap(graph, 0, 5);
            Console.WriteLine("Max flow = " + maxFlow);*/

          /*  //<9> Max Bipartite matching problem
            int[,] bpGraph = {  {0, 1, 1, 0, 0, 0},
                        {1, 0, 0, 1, 0, 0},
                        {0, 0, 1, 0, 0, 0},
                        {0, 0, 1, 1, 0, 0},
                        {0, 0, 0, 0, 0, 0},
                        {0, 0, 0, 0, 0, 1}
                      };
            var maxEdges = new MaxBipartiteMatching_JobApplications();
            int totalApplicants = maxEdges.HowMayApplicantsCanGetJobs(bpGraph);
            Console.WriteLine("Maximum number of applicants that can get job is " + totalApplicants);*/

            //<10> Parking spot problem - Max Bipartite matching
            var parkingSpot = new ParkingSpot();
            String[] parkingGraph = new[]
            {
                "XXXXXXXXXXX",
                "X......XPPX",
                "XC...P.XPPX",
                "X......X..X",
                "X....C....X",
                "XXXXXXXXXXX"
            };
            int mintime = parkingSpot.MinTimeToFindParkingSpot(parkingGraph);
            Console.WriteLine("Maximum it takes to find parking spot for all cars is " + mintime);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Total execution time in Milliseconds = " + elapsedMs);

            Console.ReadKey();
        }
    }
}
