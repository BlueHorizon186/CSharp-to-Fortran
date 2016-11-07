using System;
using System.Text;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class SymbolTable: IEnumerable<KeyValuePair<string, Type>>
    {
        IDictionary<string, Type> data = new SortedDictionary<string, Type>();

        //-----------------------------------------------------------
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Symbol Table\n");
            sb.Append("====================\n");
            foreach (var entry in data) {
                sb.Append(String.Format("{0}: {1}\n", 
                                        entry.Key, 
                                        entry.Value));
            }
            sb.Append("====================\n");
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public Type this[string key]
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
        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator()
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
