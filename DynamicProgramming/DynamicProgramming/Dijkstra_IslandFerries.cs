using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{

    class Traveler
    {
        public ulong VisitedIslands { get; set; } //bit location = island number, Max 63 islands supported
        public uint IslandCurrentlyOn { get; set; }
        public uint TotalPaidSoFar { get; set; }
        public int LastFerryTakenToArriveIslandCurrentlyOn { get; set; } //bit location = ferry number, Max 32 ferries supported

        public Traveler(ulong visitedIslands, uint islandCurrentlyOn, uint totalPaidSoFar, int lastFerryTakenToArriveIslandCurrentlyOn)
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

        public void InsertCost(int destIsland, int ferry, int cost)
        {
            var key = (uint)(65536 * destIsland + ferry);
            _costToOtherIslands.Add(key, cost);
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

        public Ferry(int connectiongIslands)
        {
            FromToLeg = new int[connectiongIslands, connectiongIslands];
        }
    }

    public class Dijkstra_IslandFerries
    {
        

        public Dictionary<string, int> TravelCheap(String[] legs, String[] prices)
        {
            var allIslands = new Dictionary<int, Island>();
            var allFerries = new Dictionary<int, Ferry>();
            for(int island=0; island<prices.Length; island++)
            {
                var tmpIsland = new Island();
                for (int ferry = 0; ferry < legs.Length; ferry++)
                {
                    string[] tmplegs = legs[ferry].Split(',');
                    var tmpFerry = new Ferry(tmplegs.Length);

                    int id = 0;
                    foreach (var tmpleg in tmplegs)
                    {
                        string fromIsland = tmpleg.Split('-')[0];
                        string toIsland = tmpleg.Split('-')[1];

                        tmpFerry.ID = id++;
                        tmpFerry.FromToLeg[int.Parse(fromIsland), int.Parse(toIsland)] = 1;
                        allFerries.Add(tmpFerry.ID, tmpFerry);

                        if (island == int.Parse(fromIsland))
                        {
                            tmpIsland.AvailableFerries.Add(tmpFerry.ID);

                            int costForThisFerryFromThisIsland = int.Parse(prices[island].Split(' ')[ferry]);
                            tmpIsland.InsertCost(int.Parse(toIsland), tmpFerry.ID, costForThisFerryFromThisIsland);
                        }
                    }
                    //int[] connectedIslandsWithThisFerry =
                }
                
            }
            
        }
    }
}
