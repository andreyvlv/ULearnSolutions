using System.Collections.Generic;

namespace yield
{
	public static class ExpSmoothingTask
	{
        public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
        {
            bool first = true;
            DataPoint prevDP = null;
            foreach (var dp in data)
            {
                dp.ExpSmoothedY = first ? dp.OriginalY : alpha * dp.OriginalY + (1 - alpha) * prevDP.ExpSmoothedY;
                prevDP = dp;
                first = false;
                yield return dp;
            }
        }
    }
}