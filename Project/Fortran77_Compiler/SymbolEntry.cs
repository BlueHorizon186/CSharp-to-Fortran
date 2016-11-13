using System;
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
            Params = null;
        }

        public override string ToString()
        {
            return String.Format("Type: {0}, IsConst: {1}",
                                 SymbolType,
                                 IsConstant);
        }
    }
}
