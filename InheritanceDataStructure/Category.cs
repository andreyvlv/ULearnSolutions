using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inheritance.DataStructure
{
    public class Category : IComparable
    {
        public string Product { get; private set; }

        public MessageType MessageType { get; private set; }

        public MessageTopic MessageTopic { get; private set; }

        public static bool operator ==(Category a, Category b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Category a, Category b)
        {
            return !a.Equals(b);
        }

        public static bool operator >(Category a, Category b)
        {
            return a.CompareTo(b) == 1;
        }

        public static bool operator <(Category a, Category b)
        {
            return a.CompareTo(b) == -1;
        }

        public static bool operator <=(Category a, Category b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Category a, Category b)
        {
            return a > b || a == b;
        }


        public Category(string product, MessageType messageType, MessageTopic messageTopic)
        {
            Product = product;
            MessageType = messageType;
            MessageTopic = messageTopic;
        }

        public override bool Equals(object obj)
        {
            var cat = obj as Category;          
            return cat is null ? false : this.Product == cat.Product 
                && this.MessageType == cat.MessageType 
                && this.MessageTopic == cat.MessageTopic;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Product, MessageType, MessageTopic).GetHashCode();
        }

        public override string ToString()
        {
            return $"{Product}.{MessageType}.{MessageTopic}";
        }

        public int CompareTo(object obj)
        {
            var cat = obj as Category;
            if (cat is null)
                return 0;
            int result;
            if (cat.Product is null || this.Product is null)
                result = 0;
            else
                result = this.Product.CompareTo(cat.Product);
            if (result == 0)
                result = this.MessageType.CompareTo(cat.MessageType);
            if (result == 0)
                result = this.MessageTopic.CompareTo(cat.MessageTopic);
            return result;
        }
    }
}
