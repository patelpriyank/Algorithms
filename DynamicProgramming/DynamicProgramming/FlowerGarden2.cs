using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DynamicProgramming
{
    public class FlowerGarden2
    {
        int[] _height, _bloom, _wilt;

        public int[] Arrangement(int[] height, int[] bloom, int[] wilt)
        {
            _height = height;
            _bloom = bloom;
            _wilt = wilt;

            if (_height.Length < 2) return _height;

            return sort(0, _height.Length - 1);
        }

        private int[] sort(int start, int end)
        {
            var results = new List<int>();

            if (start >= end) return new int[] { _height[end] };

            int[] sortedRights = sort(start + 1, end);

            int left = _height[start];
            int endPosition = 0, counter=0;

            eAction action = eAction.AddToEnd;
            foreach (var right in sortedRights)
            {
                counter++;
                action = compare(left, right);
                if (action == eAction.AddToEnd)
                    endPosition = counter;
                else if (action == eAction.Swap)
                    endPosition = counter - 1;
            }
            if (action == eAction.Swap)
            {
                results.Add(left);
                foreach (var sortedRight in sortedRights)
                {
                    results.Add(sortedRight);
                }
                results[0] = results[0] ^ results[endPosition + 1];
                results[endPosition + 1] = results[0] ^ results[endPosition + 1];
                results[0] = results[0] ^ results[endPosition + 1];
            }
            else
            {
                for (int i = 0; i < (end - start) + 1; i++)
                {
                    if (i == endPosition)
                    {
                        results.Add(left);
                    }

                    if (i < sortedRights.Length)
                        results.Add(sortedRights[i]);
                }
            }

            return results.ToArray();
        }

        enum eAction
        {
            KeepLeft,
            AddToEnd,
            Swap
        }

        //returns true if left is OK to keep on left
        private eAction compare(int left, int right)
        {
            List<int> tmp = new List<int>(_height);

            int leftPos = tmp.IndexOf(left);
            int rightPos = tmp.IndexOf(right);

            var lb = _bloom[leftPos];
            var lw = _wilt[leftPos];

            var rb = _bloom[rightPos];
            var rw = _wilt[rightPos];

            if (left > right && ((lw >= rb && lb <= rw) || (lb <= rw && lw >= rb)))
                return eAction.Swap;
            
            if (left < right && ((lw < rb) || (lb > rw))) 
                return eAction.AddToEnd;

            return eAction.KeepLeft;
        }
    }
}
