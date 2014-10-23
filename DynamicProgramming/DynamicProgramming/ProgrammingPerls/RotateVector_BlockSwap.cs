using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.ProgrammingPerls
{
    public class RotateVector_BlockSwap
    {
        //n = 7; d = 3
        public string[] BlockSwap(string[] array, int d)
        {
            if (d >= array.Length)
                return array;

            int sizeOfArrayA, sizeOfArrayB;

            sizeOfArrayA = d; //3 -> size of A
            sizeOfArrayB = array.Length - d; //7-3 = 4 -> size of B

            while (sizeOfArrayA != sizeOfArrayB)
            {
                if (sizeOfArrayA < sizeOfArrayB)
                {
                    swap(array, d - sizeOfArrayA, d - sizeOfArrayA + sizeOfArrayB, sizeOfArrayA);
                    sizeOfArrayB = sizeOfArrayB - sizeOfArrayA;
                }
                else
                {
                    swap(array, d - sizeOfArrayA, d, sizeOfArrayB);
                    sizeOfArrayA = sizeOfArrayA - sizeOfArrayB;
                }
            }

            swap(array, d - sizeOfArrayA, d, sizeOfArrayA);

            return array;
        }

        private string[] swap(string[] arr, int fi, int si, int d)
        {
            string tmp;
            for (int i = 0; i < d; i++)
            {
                tmp = arr[fi + i];
                arr[fi + i] = arr[si + i];
                arr[si + i] = tmp;
            }
            return arr;
        }
    }
}
