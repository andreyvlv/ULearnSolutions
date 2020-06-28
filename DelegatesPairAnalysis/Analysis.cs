using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)                 
            => data
                .Pairs()
                .Select(x => (x.Item2 - x.Item1).TotalSeconds)                
                .MaxIndex();       

        public static double FindAverageRelativeDifference(params double[] data) 
            => data
                .Pairs()
                .Select(x => ((x.Item2 - x.Item1) / x.Item1))                
                .Average();        
    }

    public static class Extensions
    {       
        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> list)
        {
            var first = true;
            var prev = default(T);
            foreach (var val in list)            
                if(first)
                {
                    prev = val;
                    first = false;
                    continue;
                }                                 
                else
                {
                    yield return Tuple.Create(prev, val);
                    prev = val;
                }            
        }
        
        public static int MaxIndex<T>(this IEnumerable<T> list) where T : IComparable
        {            
            var max = typeof(T).MinValue<T>();
            var bestIndex = 0;
            var counter = 0;         
            var enumerator = list.GetEnumerator();
            while(enumerator.MoveNext())
            {
                if (enumerator.Current.CompareTo(max) > 0)
                {
                    max = enumerator.Current;
                    bestIndex = counter;
                }
                counter++;
            }
            if (counter == 0)
                throw new ArgumentException();
            return bestIndex;
        }

        static T MinValue<T>(this Type self)        
            => (T)self.GetField(nameof(MinValue)).GetRawConstantValue();        
    }
}
