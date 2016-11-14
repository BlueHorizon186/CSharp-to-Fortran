using System;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    class SemanticAnalyzer
    {
        /*****************************************************************
         *  Missing Stuff to begin defining the Semantic Analyzer:
         *  :) Define the Type Enumeration.
         *  :) Define the TypeMapper according to the Type Enumeration.
         *  :) Create and define the Symbol Table class.
         * ***************************************************************
         *  - Instantiate the Symbol Table class as needed (Implementation)
         *  - Define all Visit methods.
         *  - Define all Visit methods for Operators.
         ****************************************************************/

         //-----------------------------------------------------------
         static readonly IDictionary<TokenCategory, Type> typeMapper =
            new Dictionary<TokenCategory, Type>()
            {
                { TokenCategory.CHARACTER, Type.CHARACTER },
                { TokenCategory.INTEGER, Type.INTEGER },
                { TokenCategory.LOGICAL, Type.LOGICAL },
                { TokenCategory.REAL, Type.REAL }
            };

        //-----------------------------------------------------------
        public List<SymbolTable> Tables { get; private set; }

        //-----------------------------------------------------------
        public SemanticAnalyzer()
        {
            Tables = new List<SymbolTable>();
        }

        //-----------------------------------------------------------
    }
}
