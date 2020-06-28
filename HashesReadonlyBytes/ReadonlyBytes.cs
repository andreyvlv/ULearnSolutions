using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes
{
	class ReadonlyBytes : IEnumerable<byte>
    {        
        const int FNV32Prime = unchecked(16777619);
        const int FNV32Offset = unchecked((int)0x811C9DC5);

        byte[] array;

        public ReadonlyBytes(params byte[] values)
        {
            array = values ?? throw new ArgumentNullException();
        }

        public ReadonlyBytes()
        {
            array = new byte[0];
        }       

        public int Length
        {
            get { return array.Length; }            
        }

        public byte this[int index]
        {
            get
            {
                if (index < 0 || index > array.Length) throw new IndexOutOfRangeException();
                return array[index];
            }
        }
       
        public IEnumerator<byte> GetEnumerator()
        {
            foreach (var value in array)            
                yield return value;            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override bool Equals(object obj)
        {
            var rb = obj as ReadonlyBytes;
            if (array.Length != rb.Length) return false;
            for (int i = 0; i < array.Length; i++)           
                if (array[i] != rb[i])
                    return false;
            return true;
        }        

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = FNV32Offset;
                for (int i = (this.Length >= 8 ? this.Length - 8 : 0); i < this.Length; i++)
                {
                    hash ^= array[i];
                    hash *= FNV32Prime;
                }
                return hash;
            }
        }

        public override string ToString()
        {
            if (array.Length == 0) return "[]";
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                for (int i = 0; i < array.Length - 1; i++)
                    sb.Append(array[i] + ", ");
                sb.Append(array[array.Length - 1] + "]");
                return sb.ToString();
            }
        }
    }
}