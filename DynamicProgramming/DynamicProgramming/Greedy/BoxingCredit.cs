using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.Greedy
{
    public class BoxingCredit
    {
        private const int MAX_BUTTON_PRESS_INTERVAL = 1000; //in miliseconds

        public int MaxCredit(int[] a, int[] b, int[] c, int[] d, int[] e)
        {
            var currentIndexPerJudge = new int[] {0,0,0,0,0};
            int buttonPressesWithinMAXInterval = 0;
            int timespanUpperLimit = 0;

            while (true)
            {
                var buttonPressTimePerJudge = new List<int>();
                if (currentIndexPerJudge[0] < a.Length)
                    buttonPressTimePerJudge.Add(a[currentIndexPerJudge[0]]);
                if (currentIndexPerJudge[1] < b.Length)
                    buttonPressTimePerJudge.Add(b[currentIndexPerJudge[1]]);
                if (currentIndexPerJudge[2] < c.Length)
                    buttonPressTimePerJudge.Add(c[currentIndexPerJudge[2]]);
                if (currentIndexPerJudge[3] < d.Length)
                    buttonPressTimePerJudge.Add(d[currentIndexPerJudge[3]]);
                if (currentIndexPerJudge[4] < e.Length)
                    buttonPressTimePerJudge.Add(e[currentIndexPerJudge[4]]);

                buttonPressTimePerJudge.Sort();

                if(buttonPressTimePerJudge.Count == 0) break;

                if (buttonPressTimePerJudge.Count >= 3 &&
                    (buttonPressTimePerJudge[2] - buttonPressTimePerJudge[0]) <= MAX_BUTTON_PRESS_INTERVAL)
                {
                    buttonPressesWithinMAXInterval++;
                    timespanUpperLimit = buttonPressTimePerJudge[2] + 1;
                }

                else if (buttonPressTimePerJudge.Count >= 4 &&
                    (buttonPressTimePerJudge[3] - buttonPressTimePerJudge[1]) <= MAX_BUTTON_PRESS_INTERVAL)
                {
                    buttonPressesWithinMAXInterval++;
                    timespanUpperLimit = buttonPressTimePerJudge[3] + 1;
                }

                else if (buttonPressTimePerJudge.Count >= 5 &&
                    (buttonPressTimePerJudge[4] - buttonPressTimePerJudge[2]) <= MAX_BUTTON_PRESS_INTERVAL)
                {
                    buttonPressesWithinMAXInterval++;
                    timespanUpperLimit = buttonPressTimePerJudge[4] + 1;
                }
                else
                {
                    timespanUpperLimit++;
                }

                while (currentIndexPerJudge[0] < a.Length && a[currentIndexPerJudge[0]] < timespanUpperLimit)
                    currentIndexPerJudge[0]++;

                while (currentIndexPerJudge[1] < b.Length && b[currentIndexPerJudge[1]] < timespanUpperLimit)
                    currentIndexPerJudge[1]++;

                while (currentIndexPerJudge[2] < c.Length && c[currentIndexPerJudge[2]] < timespanUpperLimit)
                    currentIndexPerJudge[2]++;

                while (currentIndexPerJudge[3] < d.Length && d[currentIndexPerJudge[3]] < timespanUpperLimit)
                    currentIndexPerJudge[3]++;

                while (currentIndexPerJudge[4] < e.Length && e[currentIndexPerJudge[4]] < timespanUpperLimit)
                    currentIndexPerJudge[4]++;

            }
            return buttonPressesWithinMAXInterval;
        }
    }
}
