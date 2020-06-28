using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class StatisticsTask
	{       
        public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
        {
            if (visits.Count > 0)
            {
                var group = visits                   
                    .OrderBy(x => x.DateTime)
                    .GroupBy(x => x.UserId)
                    .SelectMany(x => x.Select(v => new {v.SlideType, v.DateTime }).Bigrams())               
                    .Where(x => x.Item1.SlideType == slideType)
                    .Select(x => (x.Item2.DateTime - x.Item1.DateTime).TotalMinutes)
                    .Where(x => x >= 1 && x <= 120);               

                if (group.Count() > 0)
                    return group.Median();
            }
            return 0;
        }
    }
}