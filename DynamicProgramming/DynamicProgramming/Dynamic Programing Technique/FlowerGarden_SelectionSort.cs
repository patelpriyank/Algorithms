using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace DynamicProgramming
{
    public class FlowerGarden_SelectionSort
    {
        int[] _height, _bloom, _wilt;
        List<int> _sortedHeights;

        public List<int> Arrangement(int[] height, int[] bloom, int[] wilt)
        {
            _height = height;
            _bloom = bloom;
            _wilt = wilt;
           
            _sortedHeights = new List<int>(_height);
            _sortedHeights.Sort();

            if (_height.Length < 2) return new List<int>(_height);

            return sort(_height.ToList());
        }

        private List<int> sort(List<int> candidates)
        {
            if (candidates != null && candidates.Count == 1) return candidates;

            var results = new List<int>();

            var temp = new List<int>(_height);
            for (int i = 0; i < _height.Length; i++) //current flower row
            {
                int theBestFlowerForThisRow = theFrontOneFrom(temp.ToList());
                results.Add(theBestFlowerForThisRow);
                temp.Remove(theBestFlowerForThisRow);
            }
            
            print(results.ToArray());

            return results.ToList();
        }

        private int theFrontOneFrom(List<int> candidates)
        {
            if (candidates != null && candidates.Count == 1) return candidates[0];

            var eligibleCandidates = new List<int>();
            foreach (var candidate in candidates)
            {
                int isGoodCounter = 0;
                foreach (var otherFlower in candidates)
                {
                    if (otherFlower == candidate) continue;

                    if (IsEligibleForFront(candidate, otherFlower))
                    {
                        isGoodCounter++;
                    }
                }

                if (isGoodCounter == candidates.Count - 1)
                {
                    if (candidate == _sortedHeights.Last())
                    {
                        /*
                         * List<T> is backed by a simple array, plus a size field that indicates which portion of the array is actually in use. (to allow for future growth). The array isn't resized unless you add too many elements or call TrimExcess.
                           Remove is O(n), since it needs to shift the rest of the list down by one.
                           Instead, you can use a LinkedList<T> (unless you use random access), or write your own list which tracks an empty portion in front.
                         */
                        _sortedHeights.RemoveAt(_sortedHeights.Count-1);
                        return candidate;
                    }
                    eligibleCandidates.Add(candidate);
                }
            }

            return eligibleCandidates.Max();
        }

        private bool IsEligibleForFront(int candidateHeight, int otherFlower)
        {
            var tmp = new List<int>(_height);
            int leftPos = tmp.IndexOf(candidateHeight);
            int rightPos = tmp.IndexOf(otherFlower);

            var lb = _bloom[leftPos];
            var lw = _wilt[leftPos];

            var rb = _bloom[rightPos];
            var rw = _wilt[rightPos];

            bool canStayInFront = true;
            if ((lw < rb) || (lb > rw))
            {
                canStayInFront = true;
            }
            else //overlaps days
            {
                if (candidateHeight > otherFlower)
                    canStayInFront = false;
            }
            return canStayInFront;
        }


        //returns true if left is OK to keep on left
        private bool IsBetterThenTheBest(int theBestSoFar, int candidate)
        {
            int bestValue = _height[theBestSoFar];
            int cadidateValue = _height[candidate];

            List<int> tmp = new List<int>(_height);
            int leftPos = tmp.IndexOf(bestValue);
            int rightPos = tmp.IndexOf(cadidateValue);

            var lb = _bloom[leftPos];
            var lw = _wilt[leftPos];

            var rb = _bloom[rightPos];
            var rw = _wilt[rightPos];

            if (bestValue > cadidateValue && ((lw >= rb && lb <= rw) || (lb <= rw && lw >= rb)))
                return true;

            if (bestValue < cadidateValue && ((lw < rb) || (lb > rw))) 
                return true;

            return false;
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
      

    }
}
