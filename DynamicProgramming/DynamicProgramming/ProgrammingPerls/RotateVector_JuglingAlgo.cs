using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming.ProgrammingPerls
{
    public class RotateVector_JuglingAlgo
    {
        public string[] Rotate(string[] array, int noOfLocations)
        {
            int numOfIterations = Helper.GCD(array.Length, noOfLocations);
            string tmp;
            int currentLocation, k;
            for (int i = 0; i < numOfIterations; i++)
            {
                //store i-th element of arry into temp
                tmp = array[i];
                currentLocation = i;
                while (true)
                {
                    k = currentLocation + noOfLocations;
                    if (k >= array.Length)
                        k = k - array.Length;
                    if(k == i)
                        break;

                    array[currentLocation] = array[k];
                    currentLocation = k;
                }

                array[currentLocation] = tmp;
            }

            return array;
        }
    }
}
