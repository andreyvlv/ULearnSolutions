using System;

namespace Names
{
	internal static class HeatmapTask
	{
		public static HeatmapData GetBirthsPerDateHeatmap(NameData[] names)
		{           
            var days = CreateDatePartArray(30, 2);
            var months = CreateDatePartArray(12, 1);
            var birthsCounts = new double[30, 12];
            foreach (var name in names)
            {
                if(name.BirthDate.Day != 1)
                {
                    birthsCounts[name.BirthDate.Day - 2, name.BirthDate.Month - 1]++;
                }
            }
			return new HeatmapData("Пример карты интенсивностей", birthsCounts, days, months);
		}

        static string[] CreateDatePartArray(int length, int startValue)
        {
            string[] result = new string[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = (i + startValue).ToString();
            }
            return result;
        }
	}
}