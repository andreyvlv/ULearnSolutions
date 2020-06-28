using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		/// <summary>
		/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
		/// Медиана списка из четного количества элементов — среднее арифметическое двух серединных элементов списка после сортировки.
		/// </summary>
		/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
		public static double Median(this IEnumerable<double> items)
		{
            var list = items.OrderBy(x => x).ToList();
            if (list.Count == 0)
                throw new InvalidOperationException();
            return list.Count % 2 == 0 ?
                (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2 : list[list.Count / 2];
		}

		/// <returns>
		/// Возвращает последовательность, состоящую из пар соседних элементов.
		/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
		/// </returns>
		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
            bool isFirstIteration = true;
            T previous = default(T);
            foreach (var item in items)
            {               
                if(isFirstIteration)
                {
                    isFirstIteration = false;
                    previous = item;
                }
                else
                {
                    yield return Tuple.Create(previous, item);
                    previous = item;
                }
            }
		}
	}
}