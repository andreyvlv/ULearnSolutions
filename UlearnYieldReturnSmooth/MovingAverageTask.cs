using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingAverageTask
	{       
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var queue = new Queue<DataPoint>();
            var sum = 0d;
            foreach (var value in data)
            {
                queue.Enqueue(value);
                sum += value.OriginalY;
                if (queue.Count > windowWidth)
                {
                    sum -= queue.Dequeue().OriginalY;
                    value.AvgSmoothedY = sum / queue.Count;
                    yield return value;
                }
                else
                {                  
                    value.AvgSmoothedY = sum / queue.Count;
                    yield return value;
                }
            }
        }
    }
}