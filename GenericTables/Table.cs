using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>       
    {
        public List<TRow> Rows { get; private set; }
        public List<TColumn> Columns { get; private set; }

        Dictionary<(TRow, TColumn), TValue> storage;

        public Table()
        {
            storage = new Dictionary<(TRow, TColumn), TValue>();
            Rows = new List<TRow>();
            Columns = new List<TColumn>();
            Open = new OpenIndexer<TRow, TColumn, TValue>(storage, this);
            Existed = new ExistedIndexer<TRow, TColumn, TValue>(storage, this);
        }

        public OpenIndexer<TRow, TColumn, TValue> Open { get; set; }

        public ExistedIndexer<TRow, TColumn, TValue> Existed { get; set; }

        public void AddRow(TRow row)
        {
            if(!Rows.Contains(row))
                Rows.Add(row);
        }

        public void AddColumn(TColumn column)
        {
            if (!Columns.Contains(column))
                Columns.Add(column);
        }
    }

    public class OpenIndexer<TRow, TColumn, TValue>
    {
        Table<TRow, TColumn, TValue> table;
        Dictionary<(TRow, TColumn), TValue> storage;

        public OpenIndexer(Dictionary<(TRow, TColumn), TValue> storage, Table<TRow, TColumn, TValue> table)
        {
            this.storage = storage;
            this.table = table;
        }

        public TValue this[TRow rowIndex, TColumn columnIndex]
        {
            get 
            {
                if (!table.Rows.Contains(rowIndex) && !table.Columns.Contains(columnIndex))
                    return default(TValue);
                try
                {
                    return storage[(rowIndex, columnIndex)];
                }
                catch(KeyNotFoundException)
                {
                    return default(TValue);
                }
            }

            set
            {
                table.AddRow(rowIndex);
                table.AddColumn(columnIndex);
                storage[(rowIndex, columnIndex)] = value;
            }
        }
    }

    public class ExistedIndexer<TRow, TColumn, TValue>
    {
        Table<TRow, TColumn, TValue> table;
        Dictionary<(TRow, TColumn), TValue> storage;

        public ExistedIndexer(Dictionary<(TRow, TColumn), TValue> storage, Table<TRow, TColumn, TValue> table)
        {
            this.storage = storage;
            this.table = table;
        }

        public TValue this[TRow rowIndex, TColumn columnIndex]
        {
            get 
            {
                if (!table.Rows.Contains(rowIndex) || !table.Columns.Contains(columnIndex))
                    throw new ArgumentException();
                try
                {
                    return storage[(rowIndex, columnIndex)];
                }
                catch (KeyNotFoundException)
                {
                    return default(TValue);
                }
            }
            set
            {                
                if (!table.Rows.Contains(rowIndex) || !table.Columns.Contains(columnIndex))
                    throw new ArgumentException();
                storage[(rowIndex, columnIndex)] = value;
            }
        }
    }
}
