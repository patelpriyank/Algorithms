/*
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
        public List<Island> VisitedIslandsSoFar { get; set; } 
        public ulong VisitedIslands { get; set; } //bit location = island number, Max 63 islands supported
        public int IslandCurrentlyOn { get; set; }
        public int TotalPaidSoFar { get; set; }
        //public int LastFerryTakenToArriveIslandCurrentlyOn { get; set; } //bit location = ferry number, Max 32 ferries supported
        public uint TicketsInHand { get; set; } //bit location = that ferry number's ticket

        public TravelRoute(ulong visitedIslands, int islandCurrentlyOn, int totalPaidSoFar, uint ticketsInHand)
        {
            VisitedIslands = visitedIslands;
            VisitedIslandsSoFar = new List<Island>();
            IslandCurrentlyOn = islandCurrentlyOn;
            TotalPaidSoFar = totalPaidSoFar;
            //LastFerryTakenToArriveIslandCurrentlyOn = lastFerryTakenToArriveIslandCurrentlyOn;
            TicketsInHand = ticketsInHand;
        }
    }

    class Island
    {
        //commute unique key with formula for each ferry->island combination
        private Dictionary<uint, int> _costToOtherIslands;

        public int ID { get; set; }

        public List<Ferry> AvailableFerries { get; set; }
        public int ArrivalFerryTaken { get; set; }
        public int DepartureFerryTaken { get; set; }

        public Island()
        {
            _costToOtherIslands = new Dictionary<uint, int>();
            AvailableFerries = new List<Ferry>();
        }

        public void AddToAvailableFerries(int destIsland, Ferry ferry, int cost)
        {
            var key = (uint)(65536 * destIsland + ferry.ID);

            if (!_costToOtherIslands.ContainsKey(key))
            {
                _costToOtherIslands.Add(key, cost);

                if(AvailableFerries.All(e => e.ID != ferry.ID))
                    AvailableFerries.Add(ferry);

            }
        }

        public int CalculateCost(int destIsland, int ferry)
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
        private const int MAX_TICKETS_ALLOWED = 3;
        private uint MAX_TICKETS_HASH;

        List<Island> allIslands = new List<Island>();
        Dictionary<int, Ferry> allFerries = new Dictionary<int, Ferry>();

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
                var tmpIsland = new Island();
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
         #1#
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
            var start = new TravelRoute(1, 0, 0, 0);
            start.VisitedIslandsSoFar.Add(startIsland);

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
                var currentIsland = allIslands.First(e => e.ID == currentRoute.IslandCurrentlyOn);

                //find the cheapest route to all other remaining islands from current island
                //for (int remainingIsland = 0; remainingIsland < allIslands.Count; remainingIsland++)
                foreach (var remainingIsland in allIslands)
                {
                    //if this island is visited on this route then don't do anything
                    if (((currentRoute.VisitedIslands >> remainingIsland.ID) & 1) == 1)
                        continue;

                    int cheapestFerryToThisIsland = -1;
                    int cheapestCostToThisIsland = int.MaxValue;

                    //compare ticket price of each ferry that leaves from this island to this remainingIsland between price of all visited islands ticket price for the same ferry
                    foreach (var availableFerry in currentIsland.AvailableFerries)
                    {
                        //loop through already visited islands to check the price for this ferry
                        foreach (var visitedIsland in allIslands)
                        {
                            //if this ferry does not have a route between these two islands then continue to next ferry
                            if (availableFerry.FromToLeg[visitedIsland.ID, remainingIsland.ID] == 0)
                                continue;

                            if (visitedIsland.ID == remainingIsland.ID) continue;

                            //consider this island only it was visited on this route
                            if (((currentRoute.VisitedIslands >> visitedIsland.ID) & 1) == 1)
                            {
                                //what are the scenarios when i would have been able to purchase ticket for this ferry from this island?
                                //1. I did not travel through this ferry between this visitedIsland to currentIsland, hence allowing me to carry that ticket all the way here.
                                //2. i did not have 3 tickets in hand already
                                //Hint. Store the route taken to individual Island object. 



                                //you can buy ticket for this ferry, only if you currently do not hold a ticket for this ferry AND you don't already carry 3 tickets
                                //"Anti-competitive regulations prohibit you from carrying more than one ticket for the same ferry service, and from carrying more than three tickets total."
                                if (((currentRoute.TicketsInHand >> availableFerry.ID) & 1) != 1 &&
                                    ((currentRoute.TicketsInHand & MAX_TICKETS_HASH) == MAX_TICKETS_HASH))
                                {
                                    if (visitedIsland.ID == currentIsland.ID)
                                    {
                                        //you can buy ticket for this ferry, only if you currently do not hold a ticket for this ferry.
                                        //"Anti-competitive regulations prohibit you from carrying more than one ticket for the same ferry service, and from carrying more than three tickets total."
                                        if (((currentRoute.TicketsInHand >> availableFerry.ID) & 1) != 1)
                                        {
                                            int ferryCostToAvailabeIsland =
                                                currentIsland.CalculateCost(remainingIsland.ID, availableFerry.ID);

                                            if (ferryCostToAvailabeIsland != -1 &&
                                                cheapestCostToThisIsland > ferryCostToAvailabeIsland)
                                            {
                                                cheapestCostToThisIsland = ferryCostToAvailabeIsland;
                                                cheapestFerryToThisIsland = availableFerry.ID;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //you can buy ticket for this ferry, only if you currently do not hold a ticket for this ferry AND you don't already carry 3 tickets
                                    //"Anti-competitive regulations prohibit you from carrying more than one ticket for the same ferry service, and from carrying more than three tickets total."
                                    if (((currentRoute.TicketsInHand >> availableFerry.ID) & 1) != 1 &&
                                        ((currentRoute.TicketsInHand & MAX_TICKETS_HASH) == MAX_TICKETS_HASH))
                                    {
                                        int ferryCostToAvailabeIsland = visitedIsland.CalculateCost(remainingIsland.ID,
                                            availableFerry.ID);

                                        if (ferryCostToAvailabeIsland != -1 &&
                                            cheapestCostToThisIsland > ferryCostToAvailabeIsland)
                                        {
                                            cheapestCostToThisIsland = ferryCostToAvailabeIsland;
                                            cheapestFerryToThisIsland = availableFerry.ID;
                                        }
                                    }

                                }
                            }
                        } //if visited island
                    } //each available ferry from this island

                    ulong visitedIslands = (currentRoute.VisitedIslands | Convert.ToUInt64(1 << remainingIsland.ID));
                    int totalPaidSoFar = currentRoute.TotalPaidSoFar + cheapestCostToThisIsland;
                    uint ticketsOnHand = (currentRoute.TicketsInHand | Convert.ToUInt32(1 << cheapestFerryToThisIsland));

                    priorityQueue.Add(new TravelRoute(visitedIslands,remainingIsland.ID, totalPaidSoFar,cheapestFerryToThisIsland, ticketsOnHand));

                } //each remaining island for loop
            }

            return -1;
        }
    }
}
*/
