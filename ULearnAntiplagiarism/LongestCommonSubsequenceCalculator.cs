using System;
using System.Collections.Generic;

namespace Antiplagiarism
{
    public static class LongestCommonSubsequenceCalculator
    {
        public static List<string> Calculate(List<string> first, List<string> second)
        {
            var opt = CreateOptimizationTable(first, second);
            return RestoreAnswer(opt, first, second);
        }

        private static int[,] CreateOptimizationTable(List<string> first, List<string> second)
        {
            var opt = new int[first.Count + 1, second.Count + 1];
            for (int i = first.Count - 1; i >= 0; i--)
                for (int j = second.Count - 1; j >= 0; j--)
                    if (TokenDistanceCalculator.GetTokenDistance(first[i], second[j]) == 0)
                        opt[i, j] = 1 + opt[i + 1, j + 1];
                    else
                        opt[i, j] = Math.Max(opt[i + 1, j], opt[i, j + 1]);
            return opt;
        }

        private static List<string> RestoreAnswer(int[,] opt, List<string> first, List<string> second)
        {
            var res = new List<string>();
            for (int i = 0, j = 0; opt[i, j] != 0 && i < first.Count && j < second.Count;)
                if (TokenDistanceCalculator.GetTokenDistance(first[i], second[j]) == 0)
                {
                    res.Add(first[i]);
                    i++;
                    j++;
                }
                else
                    if (opt[i, j] == opt[i + 1, j])
                    i++;
                else
                    j++;
            return res;
        }
    }
}