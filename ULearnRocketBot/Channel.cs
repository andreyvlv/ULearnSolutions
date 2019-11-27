using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        List<T> list = new List<T>();

        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (list)
                {                    
                    if (index >= list.Count)
                        return null;
                    return list[index];                   
                }
            }
            set
            {
                lock (list)
                {
                    if (list.Count == index)
                    {
                        list.Add(value);
                    }
                    else if (index >= 0 && index <= list.Count)
                    {
                        list[index] = value;
                        list.RemoveRange(index + 1, list.Count - index - 1);
                    }

                }

            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (list)
                if (list.Count > 0)
                    return list[list.Count - 1];
                else
                    return null;
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (list)
                if (this.Count == 0 || this.LastItem() == knownLastItem)
                    list.Add(item);
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (list)
                    return list.Count;
            }
        }
    }
}