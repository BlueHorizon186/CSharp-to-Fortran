using System;
using System.Text;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class SymbolEntry
    {
        public Type SymbolType { get; set; }
        public bool IsConstant { get; set; }
        public List<string> Params { get; set; }

        public SymbolEntry(Type type)
        {
            SymbolType = type;
            IsConstant = false;
            Params = new List<string>();
        }

        public override string ToString()
        {
            var paramsSb = new StringBuilder();
            paramsSb.Append("[");
            foreach (var param in Params)
            {
                paramsSb.Append(param);
                paramsSb.Append(",");
            }
            paramsSb.Append("]");

            return String.Format("Type: {0}, IsConst: {1}, Params: {2}",
                                 SymbolType,
                                 IsConstant,
                                 paramsSb.ToString());
        }
    }
}
