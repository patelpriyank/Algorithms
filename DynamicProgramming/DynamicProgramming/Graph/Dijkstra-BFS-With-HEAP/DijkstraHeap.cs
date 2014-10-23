using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming
{
    //C++ implementation
    //Ref: https://github.com/ssarangi/algoquestions/blob/master/topcoder/GraphTheory/KiloManX.cpp

    public class DijkstraHeap
    {
        class node : IComparer<node>
        {
            public int weapons;
            public int shots;
            public int fromBoss, toBoss;

            public node(int w, int s, int fromB, int toB)
            {
                weapons = w;
                shots = s;
                fromBoss = fromB;
                toBoss = toB;

                //Console.WriteLine("Adding to heap:- Weapons = " + w + ", Shots = " + s);
            }

public    node()
    {
        // TODO: Complete member initialization
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

            heap.Insert(new node(0,0,-1, -1));

            while (heap.Count > 0)
            {
                node top = heap.RemoveRoot();
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("Popped from Heap:- From boss = " + top.fromBoss + ", to boos = " + top.toBoss + ", Weapons = " + top.weapons + ", Shots = " + top.shots);

                // Make sure we don't visit the same configuration twice
                if(visited[top.weapons]) continue;

                visited[top.weapons] = true;
                //Console.WriteLine("visited[" + top.weapons + "] = True");

                // A quick trick to check if we have all the weapons, meaning we defeated all the bosses.
                // We use the fact that (2^numWeapons - 1) will have all the numWeapons bits set to 1.
                int numWeapons = damageChart.Length;
                if (top.weapons == ((1 << numWeapons) - 1))
                {
                    Console.WriteLine("Total shots = " + top.shots);
                    return top.shots;
                }

                //for each boss
                for (int i = 0; i < damageChart.Length; i++)
                {
                    //Console.WriteLine("Starting boss = " + i);

                    // Check if we've already visited this boss, then don't bother trying him again
                    if ((top.weapons >> i) == 1)
                    {
                        Console.WriteLine("Already visited : " + i);
                        continue;
                    }

                    // Now figure out what the best amount of time that we can destroy this boss is, given the weapons we have.
                    // We initialize this value to the boss's health, as that is our default (with our KiloBuster).
                    int bestForThisBoss = bossHealth[i];

                    int nextBossToKill = -1;
                    string bestJ = "Cannot reach boss " + i + " from other boss";
                    //for all other boss's ith weapon: damageChart[j][i] -> ith weapon of jth boss
                    for (int j = 0; j < damageChart.Length; j++)
                    {
                        //ignore self weapon for self position
                        if (i == j)
                        {
                            //Console.WriteLine("i = j = " + j);
                            continue;
                        }

                        char[] bossWeapons = damageChart[j].ToCharArray();
                        int perShotDamage = (int) Char.GetNumericValue(bossWeapons[i]);
                        if ((top.weapons >> j) != 1 && perShotDamage != 0)
                        {
                            // We have this weapon, so try using it to defeat this boss
                            /*Console.WriteLine("This boss is already dead = " + j);
                            continue;
                        }*/

                            int shotsNeeded = bossHealth[i]/perShotDamage;
                            if (bossHealth[i]%perShotDamage > 0) shotsNeeded++;

                            if (bestForThisBoss > shotsNeeded)
                            {
                                bestForThisBoss = shotsNeeded;
                                bestJ = "From boss = " + j + ", to boos = " + i + " in shots = " + bestForThisBoss;

                                nextBossToKill = j;
                            }
                        }
                    }

                    //Console.WriteLine(bestJ);
                    heap.Insert(new node((top.weapons | (1 << i)), top.shots + bestForThisBoss, nextBossToKill, i));
                }
            }

            return 0;
        }
    }
}
