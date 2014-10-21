using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    public class BadNeighbors
    {
        private Dictionary<int, int> answer = new Dictionary<int, int>();
        private int[] blah;

        private int getAnswer(int i1, int i2)
        {
            Console.WriteLine("getAnswer: i1 = " + i1 + ", i2 = " + i2);
            Console.WriteLine("getAnswer for " + print(i1, i2));

            int key = i1*500 + i2;
            if (answer.ContainsKey(key))
            {
                Console.WriteLine("1) return answer(" + i1 + ", " + i2 + ") = " + answer[key]);
                return answer[key];
            }
            if (i1 > i2)
            {
                Console.WriteLine("2) return answer(" + i1 + ", " + i2 + ") = 0");
                return 0;
            }
            var ans1 = getAnswer(i1 + 1, i2);
            var ans2 = getAnswer(i1 + 2, i2);
            Console.WriteLine("end of recurssion for (" + i1 + ", " + i2 + "): Ans1 = " + ans1 + ", Ans2 = " + ans2);
            answer[key] = MAX(ans1, ans2 + blah[i1]);
            Console.WriteLine("3) return answer(" + i1 + ", " + i2 + ") = " + answer[key]);
            return answer[key];
        }

        public int MaxDonations(int[] donations)
        {
            blah = donations;
            int n = donations.Length;
            return MAX(getAnswer(0, n - 2), getAnswer(1, n - 1));
        }

        private int MAX(int x, int y)
        {
            Console.WriteLine("4) MAX: x = " + x + ", y = " + y);
            return x > y ? x : y;
        }

        private string print(int start, int end)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = start; i <= end; i++)
                builder.Append(blah[i] + ", ");

            return builder.ToString();
        }
    }
}
