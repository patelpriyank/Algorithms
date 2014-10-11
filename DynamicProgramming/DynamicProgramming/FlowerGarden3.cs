using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DynamicProgramming
{
    public class FlowerGarden3
    {
        int[] _height, _bloom, _wilt;

        public List<int> Arrangement(int[] height, int[] bloom, int[] wilt)
        {
            _height = height;
            _bloom = bloom;
            _wilt = wilt;

            if (_height.Length < 2) return new List<int>(_height);

            return sort(0, _height.Length - 1);
        }

        private List<int> sort(int start, int end)
        {
            var temp = new List<int>();

            if (start >= end)
                return new List<int>() { _height[end] };

            var sortedRights = sort(start + 1, end);

            temp.Add(_height[start]);
            temp.AddRange(sortedRights);

            var results = new LinkedList<int>(temp);
            //var results = new List<int>(temp);
            foreach (var x in temp)
            {
                var temp2 = new List<int>(results);
                foreach (var y in temp2)
                {
                    if(x == y) continue;

                    bool getInFront = false;
                    List<int> tempResults = results.ToList();
                    if (tempResults.IndexOf(y) > tempResults.IndexOf(x))
                        getInFront = bringYinFront(x, y);
                    else
                        getInFront = bringYinFront(y, x);
                    
                    if (getInFront)
                    {
                        results.Remove(y);
                        results.AddFirst(y);
                    }
                }
            }
            
            print(results.ToArray());

            return results.ToList();
        }

        private void print(int[] array)
        {
            var builder = new StringBuilder();

            foreach (var item in array)
            {
                builder.Append(item + ", ");
            }
            Console.WriteLine(builder.ToString());
        }
      
        //returns true if left is OK to keep on left
        private bool bringYinFront(int left, int right)
        {
            List<int> tmp = new List<int>(_height);

            int leftPos = tmp.IndexOf(left);
            int rightPos = tmp.IndexOf(right);

            var lb = _bloom[leftPos];
            var lw = _wilt[leftPos];

            var rb = _bloom[rightPos];
            var rw = _wilt[rightPos];

            if (left > right && ((lw >= rb && lb <= rw) || (lb <= rw && lw >= rb)))
                return true;

            if (left < right && ((lw < rb) || (lb > rw))) 
                return true;

            return false;
        }
    }
}
