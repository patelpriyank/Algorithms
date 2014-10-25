using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicProgramming.Helpers;

namespace DynamicProgramming.Graph.MaxBipartite_Problems
{
    public class Graduation
    {
        public String MoreClasses(String classesTaken, String[] requirements)
        {
            char[] tmpClasses = classesTaken.ToCharArray();
            _classesTakenAlready.AddRange(tmpClasses);

            int idCounter = 0;
            foreach (var requirement in requirements)
            {
                char[] tmpReq = requirement.ToCharArray();
                int totalRequired = int.Parse(tmpReq[0].ToString());
                
                var tmpReqList = new List<char>(tmpReq);
                tmpReqList.RemoveAt(0);

                var reqObj = new Requirement(idCounter++, totalRequired, tmpReqList);
                _requirements.Add(reqObj);
            }

            string classesToTakeForThisReq = "";

            var tmpAllClassesToReqMatch = new SortedDictionary<char, int>();
            foreach (var requirement in _requirements)
            {
                var resultSet = _clasesToTakeForThisReq(new GraduationRoute(requirement, tmpAllClassesToReqMatch, 0, new StringBuilder(classesToTakeForThisReq)));
                if (resultSet == null)
                {
                    //student could not fullfill this req. so he cannot graduate.
                    return "0";
                }
                classesToTakeForThisReq = resultSet.Item1;
                tmpAllClassesToReqMatch = resultSet.Item2;
            }
            return classesToTakeForThisReq;
        }


        class Requirement
        {
            public int ID { get; set; }
            public int NumberOfClassesToGraduate { get; set; }
            public List<char> ClassesList { get; set; }

            public Requirement(int id, int numOfClasses, List<char> classList)
            {
                ID = id;
                NumberOfClassesToGraduate = numOfClasses;
                ClassesList = classList;
            }
        }

        class GraduationRoute
        {
            public Requirement CurrentRequirement;
            //public SortedDictionary<int, List<char>> AllRequirementsToClassesMatching = new SortedDictionary<int, List<char>>();
            public SortedDictionary<char, int> AllClassToReqMatching  = new SortedDictionary<char, int>();
            public StringBuilder KeyBuilder = new StringBuilder();
            public int TotalClassesFoundSoFar = 0;

            public GraduationRoute(Requirement forRequirement, SortedDictionary<char, int> allClassToReqMatching, int totalClassesFoundSoFar, StringBuilder keyBuilder)
            {
                CurrentRequirement = forRequirement;
                AllClassToReqMatching = allClassToReqMatching;
                TotalClassesFoundSoFar = totalClassesFoundSoFar;
                KeyBuilder = keyBuilder;
            }

            
        }

        class GraduationRouteComparer : IComparer<GraduationRoute>
        {
            public int Compare(GraduationRoute x, GraduationRoute y)
            {
                //y > x -1
                //x > y 1

                if (x == null)
                {
                    if (y == null)
                    {
                        // If x is null and y is null, they're 
                        // equal.  
                        return 0;
                    }
                    else
                    {
                        // If x is null and y is not null, y 
                        // is greater.  
                        return -1;
                    }
                }
                else
                {
                    // If x is not null... 
                    // 
                    if (y == null)
                        // ...and y is null, x is greater.
                    {
                        return 1;
                    }
                    else
                    {
                        // ...and y is not null, compare the  
                        // lengths of the two strings. 
                        // 
                        //NOTE: There is an issue with SortedSet<> equality. SortedSet will not add element if it returns 0.
                        if (x.AllClassToReqMatching.Count < y.AllClassToReqMatching.Count)
                            return -1;
                        else if (x.AllClassToReqMatching.Count > y.AllClassToReqMatching.Count)
                            return 1;
                        else
                        {
                            if (x.KeyBuilder.ToString().CompareTo(y.KeyBuilder.ToString()) < 0)
                            {
                                return -1;
                            }
                            else if (x.KeyBuilder.ToString().CompareTo(y.KeyBuilder.ToString()) > 0)
                            {
                                return 1;
                            }

                            return 0;
                        }

                    }

                    
                }
            }
        }

        List<Requirement> _requirements = new List<Requirement>();
        List<char> _classesTakenAlready = new List<char>();


        private Tuple<string, SortedDictionary<char, int>> _clasesToTakeForThisReq(GraduationRoute forThisRoute)
        {
            var visitedClasses = new List<char>();

            var pqGraduationRoute = new BinaryHeap<GraduationRoute>(new GraduationRouteComparer());
            pqGraduationRoute.Insert(forThisRoute);

            while (pqGraduationRoute.Count > 0)
            {
                var currentReqRoute = pqGraduationRoute.RemoveRoot();
                //pqGraduationRoute.Remove(currentReqRoute);

                //if we found total classed required then return
                if (currentReqRoute.TotalClassesFoundSoFar == currentReqRoute.CurrentRequirement.NumberOfClassesToGraduate)
                    return new Tuple<string, SortedDictionary<char, int>>(currentReqRoute.KeyBuilder.ToString(), currentReqRoute.AllClassToReqMatching);

                currentReqRoute.CurrentRequirement.ClassesList.Sort();
                foreach (var availableClass in currentReqRoute.CurrentRequirement.ClassesList)
                {
                    //if this class is already assigned to this req then continue
                    if(currentReqRoute.AllClassToReqMatching.ContainsKey(availableClass) && currentReqRoute.AllClassToReqMatching[availableClass] == currentReqRoute.CurrentRequirement.ID)
                        continue;
                    

                    if (_classesTakenAlready.Contains(availableClass))
                    {
                        currentReqRoute.TotalClassesFoundSoFar++;
                        continue; //ignore this class and continue to next
                    }

                    //if we found total classed required then return
                    if (currentReqRoute.TotalClassesFoundSoFar >= currentReqRoute.CurrentRequirement.NumberOfClassesToGraduate)
                        return new Tuple<string, SortedDictionary<char, int>>(currentReqRoute.KeyBuilder.ToString(), currentReqRoute.AllClassToReqMatching);

                    if (visitedClasses.Contains(availableClass)) continue;
                    
                    var newClassToReqMatching = Helper.DeepClone(currentReqRoute.AllClassToReqMatching);
                    int newTotalClassesFoundSoFar = currentReqRoute.TotalClassesFoundSoFar;
                    //otherwise, check if this class is still available to take - means no other requirements already took this class

                    //if not available, then assign this class to this requirement and see that requirement can be assigned to some other class
                    if (!currentReqRoute.AllClassToReqMatching.ContainsKey(availableClass))
                    {
                        newClassToReqMatching.Add(availableClass, currentReqRoute.CurrentRequirement.ID);
                        newTotalClassesFoundSoFar++;
                        var newKeyBuilder = new StringBuilder();
                        newKeyBuilder.Append(currentReqRoute.KeyBuilder);
                        newKeyBuilder.Append(availableClass);

                        //we are adding the same reference to currentReqRoute.CurrentRequirement without deepcopy since we are not making any change to this object
                        pqGraduationRoute.Insert(new GraduationRoute(currentReqRoute.CurrentRequirement, newClassToReqMatching, newTotalClassesFoundSoFar, newKeyBuilder));
                    }
                    else if (currentReqRoute.AllClassToReqMatching.ContainsKey(availableClass))
                    {
                        int preReqID = newClassToReqMatching[availableClass];
                        var previousReq = _requirements[preReqID];
                        
                        //assign this class to this req
                        newClassToReqMatching[availableClass] = currentReqRoute.CurrentRequirement.ID;
                        newTotalClassesFoundSoFar++;
                        var newKeyBuilder = new StringBuilder();
                        newKeyBuilder.Append(currentReqRoute.KeyBuilder);
                        newKeyBuilder.Append(availableClass);

                        //and add previous req to queue to find another class for it
                        pqGraduationRoute.Insert(new GraduationRoute(previousReq, newClassToReqMatching, newTotalClassesFoundSoFar, newKeyBuilder));
                    }

                    //if available, then assign this class and go to next if totalClasses requirement hasn't met yet.
                }
            }

            return null;
        }
    }
}
