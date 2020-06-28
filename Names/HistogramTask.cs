using System;
using System.Linq;

namespace Names
{
	internal static class HistogramTask
	{
        public static HistogramData GetBirthsPerDayHistogram(NameData[] names, string name)
        {
            var minDay = 1;
            var maxDay = 1;
            foreach (var item in names)
            {
                if (item.Name == name)
                {
                    minDay = Math.Min(minDay, item.BirthDate.Day);
                    maxDay = Math.Max(maxDay, item.BirthDate.Day);
                }
            }
            var days = new string[31];
            for (int i = 0; i < days.Length; i++)
            {
                days[i] = (i + minDay).ToString();
            }

            var birthsCounts = new double[31];
            foreach (var item in names)
                if (item.Name == name && item.BirthDate.Day != 1)
                    birthsCounts[item.BirthDate.Day - minDay]++;

            return new HistogramData(string.Format("Рождаемость людей с именем '{0}'", name), days, birthsCounts);
        }
    }
}