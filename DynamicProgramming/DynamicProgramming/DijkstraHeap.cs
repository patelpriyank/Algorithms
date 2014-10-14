using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming
{
    public class DijkstraHeap
    {
        class node : IComparer<node>
        {
            public int weapons;
            public int shots;

            public node(int w, int s)
            {
                weapons = w;
                shots = s;
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

            heap.Insert(new node(0,0));

            while (heap.Count > 0)
            {
                node top = heap.RemoveRoot();

                // Make sure we don't visit the same configuration twice
                if(visited[top.weapons]) continue;

                visited[top.weapons] = true;

                // A quick trick to check if we have all the weapons, meaning we defeated all the bosses.
                // We use the fact that (2^numWeapons - 1) will have all the numWeapons bits set to 1.
                int numWeapons = damageChart.Length;
                if (top.weapons == ((1 << numWeapons) - 1))
                    return top.weapons;

                //for each boss
                for (int i = 0; i < damageChart.Length; i++)
                {
                    // Check if we've already visited this boss, then don't bother trying him again
                    if((top.weapons >> i) == 1) continue;

                    // Now figure out what the best amount of time that we can destroy this boss is, given the weapons we have.
                    // We initialize this value to the boss's health, as that is our default (with our KiloBuster).
                    int bestForThisBoss = bossHealth[i];

                    //for all other boss's ith weapon: damageChart[j][i] -> ith weapon of jth boss
                    for (int j = 0; j < damageChart.Length; j++)
                    {
                        //ignore self weapon for self position
                        if(i == j) continue;

                        //if this boss was already dead and acquired, ignore him too
                        if ((top.weapons >> j) == 1) continue;

                        //otherwise, find out total number of shots it will take to kill this boss with ith weapon of this jth boss
                        char[] bossWeapons = damageChart[j].ToCharArray();
                        int perShotDamage = (int)Char.GetNumericValue(bossWeapons[i]);
                        
                        if(perShotDamage == 0) continue;
                        
                        int shotsNeeded = bossHealth[i]/perShotDamage;
                        if (bossHealth[i]%perShotDamage > 0) shotsNeeded++;
                        bestForThisBoss = Math.Min(bestForThisBoss, shotsNeeded);
                    }

                    heap.Insert(new node((top.weapons | (1 << i)), top.shots + bestForThisBoss));
                }
            }

            return 0;
        }
    }
}
