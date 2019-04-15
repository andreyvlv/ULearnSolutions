using System;
using System.Collections.Generic;

namespace TodoApplication
{
    public class StackNode<TItem>
    {
        public int Index { get; set; }
        public TItem Value { get; set; }
        public Command Action { get; set; }

        public StackNode(TItem value, int index, Command action)
        {
            Value = value;
            Index = index;
            Action = action;
        }
    }

    public enum Command
    {
        Add, Remove
    }

    public class ListModel<TItem>
    {
        public List<TItem> Items { get; private set; }
        public int Limit;

        public LimitedSizeStack<StackNode<TItem>> stack;

        public ListModel(int limit)
        {
            Items = new List<TItem>();
            Limit = limit;
            stack = new LimitedSizeStack<StackNode<TItem>>(limit);
        }

        public void AddItem(TItem item)
        {
            stack.Push(new StackNode<TItem>(item, Items.Count, Command.Add));
            Items.Add(item);           
        }

        public void RemoveItem(int index)
        {
            stack.Push(new StackNode<TItem>(Items[index], index, Command.Remove));
            Items.RemoveAt(index);           
        }       

        public bool CanUndo()
        {
            return stack.Count > 0;
        }

        public void Undo()
        {
            var prevAction = stack.Pop();
            switch(prevAction.Action)
            {
                case Command.Remove:
                    Items.Insert(prevAction.Index, prevAction.Value);
                    break;
                case Command.Add:
                    Items.RemoveAt(prevAction.Index);
                    break;
            }                        
        }
    }
}
