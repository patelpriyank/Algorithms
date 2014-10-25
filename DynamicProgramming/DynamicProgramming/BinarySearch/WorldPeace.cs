using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.BinarySearch
{
    public class WorldPeace_GreedyAlgo
    {
        public long NumGroups(int k, int[] countries)
        {
            int uniqueContriesPerGroup = k;
            int totalCountries = countries.Length;
            int totalGroups = 0;
            int minNumOfCiviliansAvailableInLastKCountries;

            Array.Sort(countries);

            while (countries[countries.Length - uniqueContriesPerGroup] > 0)
            {
                minNumOfCiviliansAvailableInLastKCountries = countries[countries.Length - uniqueContriesPerGroup];
                totalGroups += minNumOfCiviliansAvailableInLastKCountries;

                for (int i = countries.Length - uniqueContriesPerGroup; i < countries.Length; i++)
                {
                    if (countries[i] <= minNumOfCiviliansAvailableInLastKCountries)
                        countries[i] = 0;
                    else
                        countries[i] -= minNumOfCiviliansAvailableInLastKCountries;
                }

                Array.Sort(countries);
            }

            return totalGroups;
        }
    }
}
