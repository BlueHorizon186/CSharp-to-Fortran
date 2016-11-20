using System;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class CILGenerator
    {
        IDictionary<string, SymbolTable> GlobalTable { get; private set; }
        private int labelCounter;

        public CILGenerator(IDictionary<string, SymbolTable> table)
        {
            GlobalTable = table;
            labelCounter = 0;
        }

        //-----------------------------------------------------------
        private string GenerateLabel()
        {
            return String.Format("${0:000000}", labelCounter++);
        }

        //-----------------------------------------------------------
        static readonly IDictionary<Type, string> CILTypes =
            {
                { Type.LOGICAL, "bool" },
                { Type.INTEGER, "int32" }
            };

        //-----------------------------------------------------------
        public string Visit(Program node)
        {
            return "";
        }
    }
}

