using System;
using System.Text;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class FunctionSymbolEntry: SymbolEntry
    {
        public bool IsArgument { get; set; }

        public FunctionSymbolEntry(Type type = Type.NONE, bool isArg = false) : base(type)
        {
            IsArgument = isArg;
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

            return String.Format("Type: {0}, IsConst: {1}, IsArgument: {2}, Params: {3}",
                                 SymbolType,
                                 IsConstant,
                                 IsArgument,
                                 paramsSb.ToString());
        }
    }
}

