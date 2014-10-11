using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DynamicProgramming
{
    public class FlowerGarden
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
            if (start >= end) return new int[] { _height[end] };
            
            //if (end - start > 1)
            //{
                var pivot = (end - start)/2;
                var leftSorted = sort(start, pivot);
                var rightSorted = sort(pivot + 1, end);
                return merge(leftSorted, rightSorted);
            //}

        }

        private int[] merge(int[] left, int[] right)
        {
            List<int> result = new List<int>();
            foreach (var lh in left)
            {
                foreach (var rh in right)
                {
                    if (compare(lh, rh))
                        result.Add(lh);
                    else
                        result.Add(rh);
                }
            }

            return result.ToArray();
        }

        //returns true if left is greater than
        private bool compare(int left, int right)
        {
            var lb = _bloom.First(e => e == left);
            var lw = _wilt.First(e => e == left);

            var rb = _height.First(e => e == right);
            var rw = _wilt.First(e => e == right);

            if (left > right && (lw < rb || lw > rb)) return true;

            return false;
        }
    }
}
