using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApplication
{
    public class LimitedSizeStack<T>
    {
        LinkedList<T> ll;

        int limit;

        public LimitedSizeStack(int limit)
        {
            ll = new LinkedList<T>();
            this.limit = limit;
        }

        public void Push(T item)
        {
            if (ll.Count < limit)
                ll.AddFirst(item);
            else
            {
                ll.AddFirst(item);
                ll.RemoveLast();
            }           
        }

        public T Pop()
        {
            var result = ll.First();
            ll.RemoveFirst();
            return result;
        }

        public int Count
        {
            get
            {
                return ll.Count;
            }
        }
    }
}
