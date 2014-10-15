using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming
{
    //C++ implementation
    //Ref: https://github.com/ssarangi/algoquestions/blob/master/topcoder/GraphTheory/KiloManX.cpp

    public class DijkstraHeap2
    {
        class node : IComparer<node>
        {
            public int weapons;
            public int shots;
            public int bestIncomingBossToHere;
            public string route;

            public node(int w, int s, int incoming, string routeSoFar)
            {
                weapons = w;
                shots = s;
                bestIncomingBossToHere = incoming;
                route = routeSoFar;

                //Console.WriteLine("Adding to heap:- Weapons = " + w + ", Shots = " + s);
            }

            public node()
            {
                // TODO: Complete member initialization
            }

            public int TotalWeaponsAcquired()
            {
                int count = 0;
                while (weapons >> 1 == 1)
                {
                    count++;
                }

                return count;
            }
            
            // Define a comparator that puts nodes with less shots on top appropriate to your language
            public int Compare(node x, node y)
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
                        if (x.shots > y.shots) return 1;
                        if (x.shots < y.shots) return -1;
                        else return 0;
                    }
                }
            }
        };

        public int LeastShots(string[] damageChart, int[] bossHealth)
        {
            var visited = new bool[(int)Math.Pow(2, damageChart.Length)];

            var heap = new BinaryHeap<node>(new node());

            heap.Insert(new node(0,0,-99, "Start"));

            while (heap.Count > 0)
            {
                node top = heap.RemoveRoot();
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("Popping:- " + top.route + ", Weapons = " + top.weapons + ", Shots = " + top.shots);
               
                // Make sure we don't visit the same configuration twice
                if(visited[top.weapons]) continue;

                visited[top.weapons] = true;
                //Console.WriteLine("visited[" + top.weapons + "] = True");

                // A quick trick to check if we have all the weapons, meaning we defeated all the bosses.
                // We use the fact that (2^numWeapons - 1) will have all the numWeapons bits set to 1.
                int numWeapons = damageChart.Length;
                if (top.weapons == ((1 << numWeapons) - 1))
                {
                    Console.WriteLine("End with boss = " + top.bestIncomingBossToHere + ", Total shots = " + top.shots);
                    return top.shots;
                }

                //for each remaining boss on this route, find the lowest shots with which it can be killed using already dead bosses's weapons
                for (int i = 0; i < damageChart.Length; i++)
                {
                    // Check if we've already visited this boss, then don't bother trying him again
                    if ((top.weapons >> i) == 1)
                    {
                        continue;
                    }

                    // Now figure out what the best amount of time that we can destroy this boss is, given the weapons we have.
                    // We initialize this value to the boss's health, as that is our default (with our KiloBuster).
                    int bestShotsSoFar = bossHealth[i] / 1;
                    int deadBossWithLowestShotsToThisBoss = -1;

                    //get the lowest shots route to this remaining boss i with the help of previously acquired weapons of dead bosses on this route
                    for (int j = 0; j < damageChart.Length; j++)
                    {
                        //ignore self -> self route
                        if(i ==j) continue;
                        
                        //consider only those bosses whom we killed on this route and whose weapons we own
                        if (((top.weapons >> j) & 1) == 1)
                        {
                            //get all weapons for this boss
                            char[] allWeaponsForThisBoss = damageChart[j].ToCharArray();
                            int perShotDamage = (int) Char.GetNumericValue(allWeaponsForThisBoss[i]);
                            if (perShotDamage != 0)
                            {
                                int shotsNeeded = bossHealth[i]/perShotDamage;
                                if (bossHealth[i]%perShotDamage > 0) shotsNeeded++;

                                if (shotsNeeded < bestShotsSoFar)
                                {
                                    bestShotsSoFar = shotsNeeded;
                                    deadBossWithLowestShotsToThisBoss = j;
                                }
                            }
                        }
                    }//end of traversing all other bosses for boss i

                  /*  //if this boss i cannot be killed/reached with any of existing weapons of dead boss, then that means he cannot be reached by this route. don't add it to the heap.
                    if(deadBossWithLowestShotsToThisBoss == -1 && top.bestIncomingBossToHere != -99)
                        continue;*/

                    //insert the lowest shots route to this remaining boss i on this route which can be killed with already acquired weapons of previously dead bosses
                    string route = "\n Kill this remaining boss " + i + " from dead boss " + deadBossWithLowestShotsToThisBoss + " with his weapon shots " + bestShotsSoFar;
                    route = " > [" + deadBossWithLowestShotsToThisBoss + " > " + i + "]";
                    Console.WriteLine("Adding: " + top.route + route);
                    heap.Insert(new node((top.weapons | (1 << i)), top.shots + bestShotsSoFar, deadBossWithLowestShotsToThisBoss, top.route + route));
                }
            }

            return 0;
        }
    }
}
