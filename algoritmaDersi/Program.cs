using System;
using System.Collections.Generic;
using System.Linq;

namespace algoritmaDersi
{
    internal class Program
    {
        static void Main(string[] args)
        {

            /*
             * Firt of all, we need to get the cluster, subset elements and their cost from user. After that it will calculate for total cost and selecting cluster's optimal result. 
             * This code also dynamic code that's why it works every cluster datas under the restricts. This problem's goal, select the minimum cluster amount and combine it until achive the general cluster.
             * --- 91240000338 | Damla Naz Gençer ---
             */


            // we need to define restircts. 
            const int maxGeneralClusterSize = 40;
            const int maxSubsetCount = 40;

            // 1. Step : We need to get general cluster elements from user. (used to comma for console design also)
            Console.WriteLine($"Please, enter the general cluster elements (separated by commas, max {maxGeneralClusterSize} elements):");
            var generalClusterInput = Console.ReadLine().Split(',').Select(int.Parse).ToList();



            //We need to check restrict
            if (generalClusterInput.Count > maxGeneralClusterSize)
            {
                Console.WriteLine($"Error: General cluster size cannot exceed {maxGeneralClusterSize} elements!");
                return;
            }



            var generalCluster = new HashSet<int>(generalClusterInput);

            //2 Step: We need to get subset amount from user and it has to be less than the restirct. 
            Console.WriteLine($"How many subsets will you enter? (Max {maxSubsetCount}):");
            int amountOfSubset = int.Parse(Console.ReadLine());

            if (amountOfSubset > maxSubsetCount)
            {
                Console.WriteLine($"Error: Number of subsets cannot exceed {maxSubsetCount}!");
                return;
            }

            var subsets = new List<(HashSet<int> subset, int cost)>();



            // 3.Step: We need to get subset elements and this subset's costs from user with use to for-loop.
            for (int i = 0; i < amountOfSubset; i++)
            {
                Console.WriteLine($"Please, enter the {i + 1}st/nd subset elements (separated by commas):");
                var subsetInput = Console.ReadLine().Split(',').Select(int.Parse).ToList();

                if (subsetInput.Count > maxGeneralClusterSize)
                {
                    Console.WriteLine($"Error: Subset size cannot exceed {maxGeneralClusterSize} elements!");
                    return;
                }

                var subset = new HashSet<int>(subsetInput);

                Console.WriteLine($"Please, enter the {i + 1}st/nd subset's cost:");
                int cost = int.Parse(Console.ReadLine());

                subsets.Add((subset, cost));
            }


            var selectedSubset = new List<(HashSet<int>, int)>();
            var elementsCovered = new HashSet<int>();


            // 4. Step: Result with using Greedly Algorithm
            while (!generalCluster.SetEquals(elementsCovered))
            {
                var bestSubset = subsets
                    .Where(k => k.subset.Except(elementsCovered).Any())
                    .OrderByDescending(k => k.subset.Except(elementsCovered).Count() / (double)k.cost)
                    .FirstOrDefault();

                if (bestSubset.subset == null)
                {
                    Console.WriteLine("General cluster does not fully covered!");
                    return;
                }

                selectedSubset.Add(bestSubset);
                elementsCovered.UnionWith(bestSubset.subset);
            }

            // 5. Step: Print the result of Greedly. 
            Console.WriteLine("Greedy solution - Selected subsets and their costs:");
            int totalGreedyCost = 0;

            foreach (var (subset, cost) in selectedSubset)
            {
                Console.WriteLine($"Subset: {{ {string.Join(", ", subset)} }}, Cost: {cost}");
                totalGreedyCost += cost;
            }

            Console.WriteLine($"\nTotal Cost (Greedy): {totalGreedyCost}");




            // 6. Step: Find the optimal solution for this datas. 
            Console.WriteLine("\nCalculating optimal solution...");
            var allCombinations = GetAllCombinations(subsets);
            int minCost = int.MaxValue;

            foreach (var combination in allCombinations)
            {
                var combinedElements = new HashSet<int>();
                int combinationCost = 0;

                foreach (var (subset, cost) in combination)
                {
                    combinedElements.UnionWith(subset);
                    combinationCost += cost;
                }

                if (combinedElements.SetEquals(generalCluster) && combinationCost < minCost)
                {
                    minCost = combinationCost;
                }
            }

            Console.WriteLine($"Minimum Cost (Optimal Solution): {minCost}");
        }



        // 7. Step: Return the all subset's combinations. 
        static IEnumerable<List<(HashSet<int>, int)>> GetAllCombinations(List<(HashSet<int>, int)> subsets)
        {
            int subsetCount = subsets.Count;
            var allCombinations = new List<List<(HashSet<int>, int)>>();

            for (int i = 1; i < (1 << subsetCount); i++)
            {
                var combination = new List<(HashSet<int>, int)>();
                for (int j = 0; j < subsetCount; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        combination.Add(subsets[j]);
                    }
                }

                allCombinations.Add(combination);
            }

            return allCombinations;
        }
    }
}