using System;
using System.Collections.Generic;

namespace Clones
{
    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    public class NodeStack<T>
    {
        Node<T> head;
        int count;

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public int Count
        {
            get { return count; }
        }

        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            node.Next = head;
            head = node;
            count++;
        }

        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            Node<T> temp = head;
            head = head.Next;
            count--;
            return temp.Data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            return head.Data;
        }

        public NodeStack<T> Copy()
        {
            NodeStack<T> nodeStack = new NodeStack<T>();
            List<T> list = new List<T>();
            Node<T> current = head;
            while (current != null)
            {
                list.Add(current.Data);
                current = current.Next;
            }
            list.Reverse();
            foreach (var item in list)
            {
                nodeStack.Push(item);
            }
            return nodeStack;
        }
    }

    public class Clone
    {
        public NodeStack<string> LearnedProgramms;
        public NodeStack<string> RollbackedProgramms;
        private bool clonned = false;

        public Clone()
        {
            LearnedProgramms = new NodeStack<string>();
        }

        public void Learn(string program)
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                clonned = false;
            }
            RollbackedProgramms = new NodeStack<string>();
            LearnedProgramms.Push(program);
        }

        public void Rollback()
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                clonned = false;
            }
            RollbackedProgramms.Push(LearnedProgramms.Pop());
        }

        public void Relearn()
        {
            if (clonned)
            {
                LearnedProgramms = LearnedProgramms.Copy();
                RollbackedProgramms = RollbackedProgramms.Copy();
                clonned = false;
            }
            LearnedProgramms.Push(RollbackedProgramms.Pop());
        }

        public string Check()
        {
            return LearnedProgramms.Count == 0
              ? "basic"
              : LearnedProgramms.Peek();
        }

        public Clone MakeCopy()
        {
            var cln = new Clone
            {
                LearnedProgramms = LearnedProgramms,
                RollbackedProgramms = RollbackedProgramms,
                clonned = true
            };
            this.clonned = true;
            return cln;
        }
    }

    public class CloneVersionSystem : ICloneVersionSystem
    {
        List<Clone> clones;

        public CloneVersionSystem()
        {
            clones = new List<Clone>();
            clones.Add(new Clone());
        }

        public string Execute(string query)
        {
            string result = string.Empty;
            var commands = query.Split();
            var cloneName = int.Parse(commands[1]);
            switch (commands[0])
            {
                case "learn":
                    clones[cloneName - 1].Learn(commands[2]);
                    return null;
                case "rollback":
                    clones[cloneName - 1].Rollback();
                    return null;
                case "relearn":
                    clones[cloneName - 1].Relearn();
                    return null;
                case "clone":
                    var clone = clones[cloneName - 1].MakeCopy();
                    clones.Add(clone);
                    break;
                case "check":
                    result = clones[cloneName - 1].Check();
                    break;
            }
            return result == string.Empty ? null : result;
        }
    }
}