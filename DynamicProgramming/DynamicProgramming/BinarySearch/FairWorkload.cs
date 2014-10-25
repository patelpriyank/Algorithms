using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.BinarySearch
{
    public class FairWorkload
    {
        private int[] allFolders;
        private int totalWorkersAvailable;

        public int GetMostWork(int[] folders, int workers)
        {
            allFolders = folders;
            totalWorkersAvailable = workers;

            int low = folders.Min();
            int high = folders.Sum();
            int mid, totalPasses = 0;

            while (low < high)
            {
                totalPasses++;
                mid = low + (high - low) / 2;

                Console.WriteLine(mid + ", ");

                if (_canItBeDoneWithGivenWorkers(mid))
                {
                    //find the minium possible workload
                    high = mid; //include mid as 'mid' could be the lowest possible value
                }
                else
                {
                    low = mid + 1;
                }

            }
            Console.WriteLine("Total passes = " + totalPasses);
            return low;
        }

        private bool _canItBeDoneWithGivenWorkers(int maxFoldersPerWorker)
        {
            int workersRequired = 1;
            int workLoadSoFar = 0;
            //get total workers required
            for (int filingCabinetNum = 0; filingCabinetNum < allFolders.Length; filingCabinetNum++)
            {
                int foldersInThisCabinet = allFolders[filingCabinetNum];

                //if current worker can handle this workload (his workloadsofar is still less than highest limit = foldersPerWorker)
                if (workLoadSoFar + foldersInThisCabinet <= maxFoldersPerWorker)
                    workLoadSoFar = workLoadSoFar + foldersInThisCabinet;
                else
                {
                    //otherwise assign this cabinet to new worker
                    workersRequired++;
                    workLoadSoFar = foldersInThisCabinet;
                }
            }

            return (workersRequired <= totalWorkersAvailable);
        }
    }
}
