using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming.Graph
{
    //Hopcroft_Karp Algorithm
    public class MaxBipartiteMatching_JobApplications
    {
        Dictionary<int, int> jobToApplicantMatches = new Dictionary<int, int>();
        private int[,] graph;

        //DFS graph based recursive function that returns true if a
        // matching job for applicant is found
        private bool _findMatchingJobForApplicant(int currentApplicant)
        {
            // Mark all jobs as not seen for this new applicant.
            var visitedJobs = new List<int>();

            // Try every job one by one
            for (int job = 0; job < graph.GetLength(1); job++)
            {
                // If applicant is interested in job and job is not visited
                if (!visitedJobs.Contains(job) && graph[currentApplicant, job] == 1)
                {
                    //Add this job into visited list so that recursive DFS call for another applicant does not consider this job
                    visitedJobs.Add(job);

                    //if job is not already taken by another applicant then return true as we found the match here
                    if (!jobToApplicantMatches.ContainsKey(job))
                        return true;
                    else
                    {
                        //else, see if that particular applicant can be assigned different job, other than this job
                        //so that we can assign this job to this current applicant in interest.
                        int anotherApplicantWithThisJob = jobToApplicantMatches[job];

                        if (_findMatchingJobForApplicant(anotherApplicantWithThisJob))
                        {
                            //then assign this job to this applicant
                            jobToApplicantMatches[job] = currentApplicant;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //The value bipartiteGraph[i][j] is 1 if i’th applicant is interested in j’th job, otherwise 0.
        public int HowMayApplicantsCanGetJobs(int[,] bipartiteGraph)
        {
            graph = bipartiteGraph;

            int totalApplicantsWhoCanGetJobs = 0;

            //for each applicant, find matching job
            for (int applicant = 0; applicant < bipartiteGraph.GetLength(0); applicant++)
            {
                if (_findMatchingJobForApplicant(applicant))
                    totalApplicantsWhoCanGetJobs++;
            }

            return totalApplicantsWhoCanGetJobs;
        }
    }
}
