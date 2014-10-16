using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    class TravelRouteComparer : IComparer<TravelRoute>
    {
        public int Compare(TravelRoute x, TravelRoute y)
        {
            //y is greater -1
                //x is greater 1

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
                        if (x.TotalPaidSoFar > y.TotalPaidSoFar) return 1;
                        if (x.TotalPaidSoFar < y.TotalPaidSoFar) return -1;
                        else return 0;
                    }
                }
        }
    }

    class TravelRoute
    {
        public ulong VisitedIslands { get; set; } //bit location = island number, Max 63 islands supported
        public int IslandCurrentlyOn { get; set; }
        public uint TotalPaidSoFar { get; set; }
        public int LastFerryTakenToArriveIslandCurrentlyOn { get; set; } //bit location = ferry number, Max 32 ferries supported

        public TravelRoute(ulong visitedIslands, int islandCurrentlyOn, uint totalPaidSoFar, int lastFerryTakenToArriveIslandCurrentlyOn)
        {
            VisitedIslands = visitedIslands;
            IslandCurrentlyOn = islandCurrentlyOn;
            TotalPaidSoFar = totalPaidSoFar;
            LastFerryTakenToArriveIslandCurrentlyOn = lastFerryTakenToArriveIslandCurrentlyOn;
        }
    }

    class Island
    {
        //commute unique key with formula for each ferry->island combination
        private Dictionary<uint, int> _costToOtherIslands;

        public int ID { get; set; }

        public List<int> AvailableFerries { get; set; }

        public Island()
        {
            _costToOtherIslands = new Dictionary<uint, int>();
            AvailableFerries = new List<int>();
        }

        public void AddToAvailableFerries(int destIsland, int ferry, int cost)
        {
            var key = (uint)(65536 * destIsland + ferry);

            if (!_costToOtherIslands.ContainsKey(key))
            {
                AvailableFerries.Add(ferry);
                _costToOtherIslands.Add(key, cost);
            }
        }

        private int CalculateCost(int destIsland, int ferry)
        {
            var key = (uint)(65536 * destIsland + ferry);

            if (_costToOtherIslands.ContainsKey(key))
                return _costToOtherIslands[key];

            return -1;
        }
    }

    class Ferry
    {
        public int ID { get; set; }

        public int[,] FromToLeg { get; set; } //[from][to]

        public Ferry(int totalIslands)
        {
            FromToLeg = new int[totalIslands, totalIslands];
        }
    }

    public class Dijkstra_IslandFerries
    {
        Dictionary<int, Island> allIslands = new Dictionary<int, Island>();
        Dictionary<int, Ferry> allFerries = new Dictionary<int, Ferry>();

        private void _setup(String[] legs, String[] prices)
        {
            //set up ferries
             for (int ferry = 0; ferry < legs.Length; ferry++)
                {
                    var tmpFerry = new Ferry(prices.Length);
                    tmpFerry.ID = ferry;

                    string[] legsOfThisFerry = legs[ferry].Split(' ');
                    foreach (var tmpleg in legsOfThisFerry)
                    {
                        string fromIsland = tmpleg.Split('-')[0];
                        string toIsland = tmpleg.Split('-')[1];
                        
                        tmpFerry.FromToLeg[int.Parse(fromIsland), int.Parse(toIsland)] = 1;
                    }

                    allFerries.Add(tmpFerry.ID, tmpFerry);
                }

            for (int island = 0; island < prices.Length; island++)
            {
                var tmpIsland = new Island();
                tmpIsland.ID = island;

                foreach (var ferryKey in allFerries.Keys)
                {
                    var ferry = allFerries[ferryKey];
                    for (int from = 0; from < ferry.FromToLeg.GetLength(0); from++)
                    {
                        for (int to = 0; to < ferry.FromToLeg.GetLength(1); to++)
                        {
                            if (island == from)
                            {
                                int costForThisFerryFromThisIsland = int.Parse(prices[island].Split(' ')[ferry.ID]);
                                tmpIsland.AddToAvailableFerries(to, ferry.ID, costForThisFerryFromThisIsland);
                            }
                        }
                    }
                }
                
                allIslands.Add(island, tmpIsland);
            }
        }

        /*
         * There will be up to 40 islands and 10 ferry services. Given the list of legs offered by each ferry service and the prices of tickets on each island, 
         * for each island compute the cost of traveling there from your initial island (island 0), and return the costs as a int[]. 
         * The size of your returned int[] should be one less than the number of islands. If a given island is unreachable, return -1 for the cost to that island.
         */
        public Dictionary<string, int> TravelCheap(String[] legs, String[] prices)
        {
           _setup(legs, prices);

            _dijkstraAlgo();

            return null;
        }

        private uint _dijkstraAlgo(int destinationIsland = 3)
        {
            //start with island 0
            var start = new TravelRoute(0, 0, 0, 0);

            var priorityQueue = new SortedSet<TravelRoute>(new TravelRouteComparer());
            priorityQueue.Add(start);

            while (priorityQueue.Count > 0)
            {
                var currentRoute = priorityQueue.First();
                priorityQueue.Remove(currentRoute);

                if(((currentRoute.VisitedIslands >> destinationIsland) & 1)==1)
                {
                    return currentRoute.TotalPaidSoFar;
                }

                //determing cheapest route to destination island for this current island
                var currentIsland = allIslands[currentRoute.IslandCurrentlyOn];

                //find the cheapest route to all other remaining islands from current island
                for (int remainingIsland = 0; remainingIsland < allIslands.Count; remainingIsland++)
                {
                    //loop through already visited islands to consider buying tickets from those islands
                    for (int visitedIsland = 0; visitedIsland < allIslands.Count; visitedIsland++)
                    {
                        if (visitedIsland == remainingIsland) continue;

                        if (((currentRoute.VisitedIslands >> visitedIsland) & 1) == 1) //if this island is visited on this route then,
                        {
                            
                        }
                    }
                }
            }

        }
    }
}
