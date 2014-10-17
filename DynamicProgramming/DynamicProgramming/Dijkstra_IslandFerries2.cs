using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    internal class TravelRouteComparer : IComparer<TravelRoute>
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

    internal class TravelRoute
    {
        public List<Island> VisitedIslandsSoFar { get; set; }
        public int IslandCurrentlyOn { get; set; }
        public int TotalPaidSoFar { get; set; }

        public TravelRoute(int islandCurrentlyOn, int totalPaidSoFar)
        {
            VisitedIslandsSoFar = new List<Island>();
            IslandCurrentlyOn = islandCurrentlyOn;
            TotalPaidSoFar = totalPaidSoFar;
        }
    }

    internal class Island
    {
        //commute unique key with formula for each ferry->island combination
        private Dictionary<uint, int> _costToOtherIslands;

        public int ID { get; set; }

        public List<Ferry> AvailableFerries { get; set; }
        public int ArrivalFerryTaken { get; set; }
        public int DepartureFerryTaken { get; set; }
        public uint TicketsInHand { get; set; } //bit location = that ferry number's ticket

        public Island(uint ticketsInHand)
        {
            _costToOtherIslands = new Dictionary<uint, int>();
            AvailableFerries = new List<Ferry>();
            TicketsInHand = ticketsInHand;
        }

        public void AddToAvailableFerries(int destIsland, Ferry ferry, int cost)
        {
            var key = (uint) (65536*destIsland + ferry.ID);

            if (!_costToOtherIslands.ContainsKey(key))
            {
                _costToOtherIslands.Add(key, cost);

                if (AvailableFerries.All(e => e.ID != ferry.ID))
                    AvailableFerries.Add(ferry);

            }
        }

        public int CalculateCost(int destIsland, int ferry)
        {
            var key = (uint) (65536*destIsland + ferry);

            if (_costToOtherIslands.ContainsKey(key))
                return _costToOtherIslands[key];

            return -1;
        }
    }

    internal class Ferry
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
        private const int MAX_TICKETS_ALLOWED = 3;
        private uint MAX_TICKETS_HASH;

        private List<Island> allIslands = new List<Island>();
        private Dictionary<int, Ferry> allFerries = new Dictionary<int, Ferry>();

        private void _setup(String[] legs, String[] prices)
        {
            MAX_TICKETS_HASH = Convert.ToUInt32(Math.Pow(2, MAX_TICKETS_ALLOWED) - 1);

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
                var tmpIsland = new Island(0);
                tmpIsland.ID = island;
                tmpIsland.ArrivalFerryTaken = -1;
                tmpIsland.DepartureFerryTaken = -1;

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
                                tmpIsland.AddToAvailableFerries(to, ferry, costForThisFerryFromThisIsland);
                            }
                        }
                    }
                }

                allIslands.Add(tmpIsland);
            }
        }

        /*
         * There will be up to 40 islands and 10 ferry services. Given the list of legs offered by each ferry service and the prices of tickets on each island, 
         * for each island compute the cost of traveling there from your initial island (island 0), and return the costs as a int[]. 
         * The size of your returned int[] should be one less than the number of islands. If a given island is unreachable, return -1 for the cost to that island.
         */

        public Dictionary<string, int> TravelCheap(String[] legs, String[] prices, int destinationIsland)
        {
            _setup(legs, prices);

            _dijkstraAlgo(destinationIsland);

            return null;
        }

        private int _dijkstraAlgo(int destinationIsland)
        {
            //start with island 0 - setting first bit to 1 as '00001'
            var startIsland = allIslands[0];
            var start = new TravelRoute(0, 0);
            start.VisitedIslandsSoFar.Add(startIsland);

            var priorityQueue = new SortedSet<TravelRoute>(new TravelRouteComparer());
            priorityQueue.Add(start);

            while (priorityQueue.Count > 0)
            {
                var currentRoute = priorityQueue.First();
                priorityQueue.Remove(currentRoute);

                if (currentRoute.VisitedIslandsSoFar.Any(e => e.ID == destinationIsland))
                {
                    return currentRoute.TotalPaidSoFar;
                }

                //determing cheapest route to destination island for this current island
                var currentIsland = currentRoute.VisitedIslandsSoFar.First(e => e.ID == currentRoute.IslandCurrentlyOn);

                //find the cheapest route to all other remaining islands from current island
                //for (int remainingIsland = 0; remainingIsland < allIslands.Count; remainingIsland++)
                var remainingIslands = allIslands.Where(e => currentRoute.VisitedIslandsSoFar.All(v => v.ID != e.ID));
                foreach (var remainingIsland in remainingIslands)
                {
                    //compare ticket price of each ferry that leaves from this island to this remainingIsland between price of all visited islands ticket price for the same ferry
                    foreach (var availableFerry in currentIsland.AvailableFerries)
                    {
                        int cheapestTicketCostForThisFerryFromAllVisitedIsland = int.MaxValue;
                        int visitedIslandOfferingCheapestTicket = -1;


                        //loop through already visited islands to check the price for this ferry
                        foreach (var visitedIsland in currentRoute.VisitedIslandsSoFar)
                        {
                            //if this ferry does not have a route between these two islands then continue to next ferry
                            if (availableFerry.FromToLeg[visitedIsland.ID, remainingIsland.ID] == 0)
                                continue;

                            if (visitedIsland.ID == remainingIsland.ID) continue;

                            //you can buy ticket for this ferry from this island and carry it with you only if you didn't board this same ferry to go to next island on route.
                            //you don't already have 3 tickets and you don't have a ticket for the same ferry already in hand - either carried from previous islands or bought one to go to next island
                            if (((visitedIsland.TicketsInHand >> availableFerry.ID) & 1) == 1 ||
                                ((visitedIsland.TicketsInHand & MAX_TICKETS_HASH) == MAX_TICKETS_HASH))
                                continue;

                            //what are the scenarios when i would have been able to purchase ticket for this ferry from this island?
                            //1. I did not travel through this ferry between this visitedIsland to currentIsland, hence allowing me to carry that ticket all the way here.
                            //2. i did not have 3 tickets in hand already
                            //Hint. Store the route taken to individual Island object. 

                            //if (visitedIsland.DepartureFerryTaken != availableFerry.ID)
                            //{
                            int ferryCostToRemainingIsland = visitedIsland.CalculateCost(remainingIsland.ID, availableFerry.ID);
                            if (ferryCostToRemainingIsland < cheapestTicketCostForThisFerryFromAllVisitedIsland)
                            {
                                cheapestTicketCostForThisFerryFromAllVisitedIsland = ferryCostToRemainingIsland;
                                visitedIslandOfferingCheapestTicket = visitedIsland.ID;
                            }

                        } //each visited island

                        //found cheapest ticket for this ferry
                        var newFairyRoute = new TravelRoute(remainingIsland.ID, currentRoute.TotalPaidSoFar + cheapestTicketCostForThisFerryFromAllVisitedIsland);
                        newFairyRoute.VisitedIslandsSoFar.AddRange(currentRoute.VisitedIslandsSoFar.ConvertAll(e => e));
                        newFairyRoute.VisitedIslandsSoFar.Add(remainingIsland);
                        newFairyRoute.VisitedIslandsSoFar[visitedIslandOfferingCheapestTicket].TicketsInHand |= Convert.ToUInt32(1 << availableFerry.ID);
                        newFairyRoute.VisitedIslandsSoFar[currentIsland.ID].TicketsInHand |= Convert.ToUInt32(1 << availableFerry.ID);
                        priorityQueue.Add(newFairyRoute);

                    } //each available ferry from this current island

                } //each remaining island for loop
            } //while queue

            return -1;
        }
    }
}
