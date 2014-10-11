using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
           /* var badNeighbor = new BadNeighbors();
            var donations = new int[] { 10, 3, 2, 5, 7, 8 };
            int result = badNeighbor.MaxDonations(donations);
            Console.WriteLine("Max donation = " + result);*/

            //flowergarder
            var heights = new int[] { 239, 878, 182, 109, 510, 581, 643, 402, 470, 849, 196, 707, 810, 913, 758, 261, 451, 240, 624, 60, 671, 363, 300, 40, 22, 229, 755, 82, 8, 949, 906, 411, 17, 566 };
            var blooms = new int[] { 296, 136, 306, 94, 345, 37, 11, 133, 269, 276, 199, 348, 277, 324, 146, 252, 52, 138, 37, 5, 231, 314, 230, 138, 202, 340, 235, 58, 99, 123, 272, 67, 48, 277 }; //{ 1, 5, 10, 15, 20 };
            var wilts = new int[] { 301, 317, 339, 271, 361, 148, 117, 291, 357, 331, 282, 354, 342, 341, 178, 261, 158, 357, 177, 208, 323, 336, 256, 182, 228, 342, 326, 236, 358, 315, 325, 200, 358, 341 }; //{ 5, 10, 14, 20, 25 };

            var watch = Stopwatch.StartNew();
            var result = new FlowerGarden_SelectionSort().Arrangement(heights, blooms, wilts);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Total execution time in Milliseconds = " + elapsedMs);

            Console.ReadKey();
        }
    }
}
