using System;
using System.Text;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class SymbolTable: IEnumerable<KeyValuePair<string, SymbolEntry>>
    {
        IDictionary<string, SymbolEntry> data = new SortedDictionary<string, SymbolEntry>();

        //-----------------------------------------------------------
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Symbol Table\n");
            sb.Append("====================\n");

            foreach (var entry in data)
            {
                sb.Append(String.Format("{0} ---> {1}\n",
                                        entry.Key,
                                        entry.Value.ToString()));
            }

            sb.Append("====================\n");
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public SymbolEntry this[string key]
        {
            get { return data[key]; }
            set { data[key] = value; }
        }

        //-----------------------------------------------------------
        public bool Contains(string key)
        {
            return data.ContainsKey(key);
        }

        //-----------------------------------------------------------
        public IEnumerator<KeyValuePair<string, SymbolEntry>> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        //-----------------------------------------------------------
        System.Collections.IEnumerator
                System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
