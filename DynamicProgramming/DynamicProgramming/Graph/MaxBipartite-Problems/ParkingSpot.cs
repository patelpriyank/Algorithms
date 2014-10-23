using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming.Graph.MaxBipartite_Problems
{
    public class ParkingSpot
    {
        [Serializable]
        class Spot
        {
            public int X;
            public int Y;
            public int TimeCost;
            public int From { get; set; }

            public Spot(int row, int col, int time)
            {
                X = row;
                Y = col;
                TimeCost = time;
            }
        }

        [Serializable]
        class CarRoute
        {
            public int X;
            public int Y;
            public int TimeCost;
            public int From { get; set; }
            public Dictionary<int, int> ParkingSpotToCarMatching = new Dictionary<int, int>();

            public CarRoute(int row, int col, int time, int from)
            {
                X = row;
                Y = col;
                TimeCost = time;
                From = from;
            }
        }

        List<int> allParkingSpots = new List<int>();
        List<int> allCars = new List<int>();

        public int MinTimeToFindParkingSpot(string[] parkingMap)
        {
            char[,] parkingGrid = new char[parkingMap.Length, parkingMap[0].Length];

            //create parking grid
            for (int row = 0; row < parkingMap.Length; row++)
            {
                char[] parkingSpot = parkingMap[row].ToCharArray();

                for (int colSpot = 0; colSpot < parkingSpot.Length; colSpot++)
                {
                    parkingGrid[row, colSpot] = parkingSpot[colSpot];

                    if (parkingSpot[colSpot] == 'C')
                        allCars.Add(getKey(row, colSpot));

                    if (parkingSpot[colSpot] == 'P')
                        allParkingSpots.Add(getKey(row, colSpot));
                }
            }

            //use BFS to get shorted distance (cost) between each car <-> spot
            //[carKey:(x,y) , [parkingspot (x,y), timeCost ] ]
            var timeForCarToAllParkingSpots = new Dictionary<int, Dictionary<int, int>>();

            for (int row = 0; row < parkingGrid.GetLength(0); row++)
            {
                for (int col = 0; col < parkingGrid.GetLength(1); col++)
                {
                    //for each car, find shortest route (nearest parking spot) 
                    if (parkingGrid[row, col] == 'C')
                    {
                        var distanceToParkingForThisCar = bfs(row, col, parkingGrid);
                        timeForCarToAllParkingSpots.Add(getKey(row, col), distanceToParkingForThisCar);
                    }
                }
            }

            var parkingSpotToCarMatching = new Dictionary<int, int>();
            var minimumTimeToParkPerCar = new List<int>();
            int bestMinTime = -1;
            foreach (var carKey in allCars)
            {
                var sourceCar = new CarRoute(carKey/KEY_MULTIPLIER, carKey%KEY_MULTIPLIER, 0, super_source);
                sourceCar.From = super_source;
                sourceCar.ParkingSpotToCarMatching = parkingSpotToCarMatching;

                var resultSet = _calculateMinTimeForEachCar(sourceCar, timeForCarToAllParkingSpots[carKey]);

                bestMinTime = resultSet.Item1;
                parkingSpotToCarMatching = resultSet.Item2;

                minimumTimeToParkPerCar.Add(bestMinTime);
            }

            //If it is impossible for each car to drive to a parking place, return -1.
            if (minimumTimeToParkPerCar.Count != allCars.Count)
                return -1;

            //else return the final time which is the best minimum time to park all the cars
            return bestMinTime;
        }

        #region "Heap/Priority queue technique"

        //key = current node, value = from node
        //Dictionary<int, int> _fromSpots = new Dictionary<int, int>();
        //List<int> _parkingSpotAlreadyTaken = new List<int>();
        const int super_source = -1;
        const int super_sink = -99;
        private Tuple<int, Dictionary<int, int>> _calculateMinTimeForEachCar(CarRoute sourceCar, Dictionary<int, int> reachableParkingSpotsForThisCar)
        {
            var pqRoute = new BinaryHeap<CarRoute>(new CarRouteComparer());
            var visitedParkingSpots = new List<int>();

            pqRoute.Insert(sourceCar);

            //build FromSpots route
            while (pqRoute.Count > 0)
            {
                //pop the top element from priority queue - the spot with minium cost
                var currentCar = pqRoute.RemoveRoot();
                //pqRoute.Remove(currentCar);

                //reached super sink
                if (currentCar.X == super_sink)
                    return new Tuple<int, Dictionary<int, int>>(currentCar.TimeCost, currentCar.ParkingSpotToCarMatching);
/*
                //if already visited, then ignore it
                if(parkingSpotAlreadyTaken.Contains(getKey(currentCar.X, currentCar.Y))) continue;

                //otherwise add to visited spot
                parkingSpotAlreadyTaken.Add(getKey(currentCar.X, currentCar.Y));

                if (FromSpots.ContainsKey(getKey(currentCar.X, currentCar.Y)))
                    FromSpots[getKey(currentCar.X, currentCar.Y)] = sourceCar.From;
                else
                    FromSpots.Add(getKey(currentCar.X, currentCar.Y), sourceCar.From);
*/

                if(reachableParkingSpotsForThisCar == null) throw new ApplicationException("Invalid input");

                //each parking spot (from set B) connected/reachable (has an edge) by this car
                foreach (var parkingSpot in allParkingSpots)
                {
                    if (visitedParkingSpots.Contains(parkingSpot)) 
                        continue;
                    else
                        visitedParkingSpots.Add(parkingSpot);

                    /*int parkingSpotX = parkingSpot / KEY_MULTIPLIER;
                    int parkingSpotY = parkingSpot % KEY_MULTIPLIER;*/

                    //if the edge doesn't exist or this car is already matched with this parking spot, then ignore this parking spot
                    if(!reachableParkingSpotsForThisCar.ContainsKey(parkingSpot)
                        || (currentCar.ParkingSpotToCarMatching.ContainsKey(parkingSpot) && currentCar.ParkingSpotToCarMatching[parkingSpot] == getKey(currentCar.X, currentCar.Y)))
                        continue;

                    //otherwise there is an edge between current car and this parking spot, find out if it is already taken or not
                    if (currentCar.ParkingSpotToCarMatching.ContainsKey(parkingSpot))
                    {
                        //Go back to that parked car and find out if that car can be parked at some other spot. 
                        int carParkedAtThisParkingSpot = currentCar.ParkingSpotToCarMatching[parkingSpot];

                        //Add previously parked car to the queue and add max cost on this route to it
                        int costSoFarOnThisRoute = currentCar.TimeCost;
                        int costOfParkingThisCarInThisParkingSpot = reachableParkingSpotsForThisCar[parkingSpot];
                        int maxTimeCostOnThisRoute = Math.Max(costSoFarOnThisRoute, costOfParkingThisCarInThisParkingSpot);

                        var previousCarSpot = new CarRoute(carParkedAtThisParkingSpot/KEY_MULTIPLIER, carParkedAtThisParkingSpot%KEY_MULTIPLIER, maxTimeCostOnThisRoute, getKey(currentCar.X, currentCar.Y));
                        previousCarSpot.From = parkingSpot;
                        previousCarSpot.ParkingSpotToCarMatching = Helper.DeepClone(currentCar.ParkingSpotToCarMatching);
                        //Add car to this parking spot matching matrix
                        previousCarSpot.ParkingSpotToCarMatching[parkingSpot] = getKey(currentCar.X, currentCar.Y);
                        pqRoute.Insert(previousCarSpot);
                    }
                    else
                    {
                        //otherwise parking spot is available to park. So park this car here
                        int costSoFarOnThisRoute = currentCar.TimeCost;
                        int costOfParkingThisCarInThisParkingSpot = reachableParkingSpotsForThisCar[parkingSpot];
                        int maxTimeCostOnThisRoute = Math.Max(costSoFarOnThisRoute, costOfParkingThisCarInThisParkingSpot);
                        var next_superSink = new CarRoute(super_sink, super_sink, maxTimeCostOnThisRoute, getKey(currentCar.X, currentCar.Y));
                        next_superSink.ParkingSpotToCarMatching = Helper.DeepClone(currentCar.ParkingSpotToCarMatching);

                        next_superSink.ParkingSpotToCarMatching.Add(parkingSpot, getKey(currentCar.X, currentCar.Y));
                        pqRoute.Insert(next_superSink);
                    }

                }
            }

            return new Tuple<int, Dictionary<int, int>>(-1, null);
        }

        [Serializable]
        class CarRouteComparer : IComparer<CarRoute>
        {
            public int Compare(CarRoute x, CarRoute y)
            {
                //y > x -1
                //x > y 1

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
                        //NOTE: There is an issue with SortedSet<> equality. SortedSet will not add element if it returns 0.
                        if (x.TimeCost > y.TimeCost) return 1;
                        if (x.TimeCost < y.TimeCost) return -1;
                        else
                            return 0;
                    }
                }
            }

        }


        #endregion

        #region "Car to Parking spot cost matrix"

        const int KEY_MULTIPLIER = 100;

        private Dictionary<int, int> bfs(int sourceCarRow, int sourceCarCol, char[,] parkingGrid)
        {
            var timeToParkingSpotsForThisCar = new Dictionary<int, int>();
            var visitedSpots = new List<int>();

            //int[,] bfsDistanceMatrix = new int[parkingGrid.GetLength(0), parkingGrid.GetLength(1)];
            //var bfsCarToParkingDistanceMatric = new Dictionary<int, int>();

            var bfsRoute = new Queue<Spot>();
            //int visitedCar = KEY_MULTIPLIER * sourceCarRow + sourceCarCol;
            var sourceSpot = new Spot(sourceCarRow, sourceCarCol, 0);

            bfsRoute.Enqueue(sourceSpot);

            //traversing tree with breath-first search for each spot except wall ('X') and note down the distance from source car
            while (bfsRoute.Count > 0)
            {
                var currentSpot = bfsRoute.Dequeue();
                //(x, y) gives location of car in parkingGrid[,]
                int x = currentSpot.X; //currentSpot / KEY_MULTIPLIER;
                int y = currentSpot.Y; //currentSpot % KEY_MULTIPLIER;
                int distance = currentSpot.TimeCost;
                
                //ignore spots outside the grid as well as already visited spots on current route
                if(!isValidSpot(x,y,visitedSpots,parkingGrid)) continue;

                //add to visited spot
                visitedSpots.Add(getKey(x, y));

                if(parkingGrid[x,y] == 'X')
                    continue;

                if(parkingGrid[x,y] == 'P')
                    timeToParkingSpotsForThisCar.Add(getKey(x, y), distance);

                if (isValidSpot(x - 1, y, visitedSpots, parkingGrid))
                    bfsRoute.Enqueue(new Spot(x - 1, y, distance + 1)); // 1 unit of time = 1 spot

                if (isValidSpot(x + 1, y, visitedSpots, parkingGrid))
                    bfsRoute.Enqueue(new Spot(x + 1, y, distance + 1));

                if (isValidSpot(x, y - 1, visitedSpots, parkingGrid))
                    bfsRoute.Enqueue(new Spot(x, y - 1, distance + 1));

                if (isValidSpot(x, y + 1, visitedSpots, parkingGrid))
                    bfsRoute.Enqueue(new Spot(x, y + 1, distance + 1));

            }

            return timeToParkingSpotsForThisCar;
        }

        private bool isValidSpot(int x, int y, List<int> visitedSpots, char[,] parkingGrid)
        {
            //ignore spots outside the grid as well as already visited spots on current route
            if (x < 0 || x > parkingGrid.GetLength(0) - 1) return false;
            if (y < 0 || y > parkingGrid.GetLength(1) - 1) return false;
            if (visitedSpots.Contains(getKey(x, y))) return false;

            return true;
        }

        private int getKey(int x, int y)
        {
            return KEY_MULTIPLIER * x + y;
        }

        #endregion
    }
}
