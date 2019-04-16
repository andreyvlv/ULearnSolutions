using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class MovingMaxTask
	{
        public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var queue = new MyQueue<double>();
            foreach (var value in data)
            {
                queue.Enqueue(value.OriginalY);
                if (queue.Count > windowWidth)
                {
                    queue.Dequeue();
                    value.MaxY = queue.Max;
                    yield return value;
                }
                else
                {
                    value.MaxY = queue.Max;
                    yield return value;
                }
            }
        }
    }

    struct QueueItem<T>
    {
        public T Item;
        public T Max;

        public QueueItem(T item, T max)
        {
            this.Item = item;
            this.Max = max;
        }
    }

    class MyQueue<T> where T : IComparable<T>
    {
        Stack<QueueItem<T>> s1 = new Stack<QueueItem<T>>();
        Stack<QueueItem<T>> s2 = new Stack<QueueItem<T>>();

        public int Count { get { return s1.Count + s2.Count; } }

        public T Max
        {
            get
            {
                if (s1.Count == 0 || s2.Count == 0)
                    return s1.Count == 0 ?
                        s2.Peek().Max :
                        s1.Peek().Max;
                else
                    return s1.Peek().Max.CompareTo(s2.Peek().Max) > 0 ?
                        s1.Peek().Max :
                        s2.Peek().Max;
            }
        }

        public void Enqueue(T value)
        {
            T max = s1.Count == 0 ?
                value :
                value.CompareTo(s1.Peek().Max) > 0 ?
                    value :
                    s1.Peek().Max;
            s1.Push(new QueueItem<T>(value, max));
        }

        public T Dequeue()
        {
            if (s2.Count == 0)
                while (!(s1.Count == 0))
                {
                    T element = s1.Peek().Item;
                    s1.Pop();
                    T max = s2.Count == 0 ?
                        element :
                        element.CompareTo(s2.Peek().Max) > 0 ?
                            element :
                            s2.Peek().Max;
                    s2.Push(new QueueItem<T>(element, max));
                }
            T result = s2.Peek().Item;
            s2.Pop();
            return result;
        }
    }
}