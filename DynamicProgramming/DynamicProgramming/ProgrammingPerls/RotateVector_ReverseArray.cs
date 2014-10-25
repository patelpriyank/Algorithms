using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.ProgrammingPerls
{
    public class RotateVector_ReverseArray
    {
        private string[] reverse(string[] array, int start, int end)
        {
            string tmp;
            while (start < end)
            {
                tmp = array[start];
                array[start] = array[end];
                array[end] = tmp;
                
                start++;
                end--;
            }

            return array;
        }

        public string[] ReverseArray(string[] array, int d)
        {
            reverse(array, 0, d - 1);
            reverse(array, d, array.Length-1);
            reverse(array, 0, array.Length - 1);

            return array;
        }
    }
}
