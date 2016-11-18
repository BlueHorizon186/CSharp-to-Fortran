using System;
using System.Text;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class SemanticAnalyzer
    {
        //-----------------------------------------------------------
        static readonly IDictionary<TokenCategory, Type> typeMapper =
            new Dictionary<TokenCategory, Type>()
            {
                { TokenCategory.CHARACTER, Type.CHARACTER },
                { TokenCategory.FUNCTION, Type.FUNCTION },
                { TokenCategory.INTEGER, Type.INTEGER },
                { TokenCategory.LOGICAL, Type.LOGICAL },
                { TokenCategory.PROGRAM, Type.PROGRAM },
                { TokenCategory.REAL, Type.REAL },
                { TokenCategory.SUBROUTINE, Type.SUBROUTINE }
            };

        //-----------------------------------------------------------
        public IDictionary<string, SymbolTable> GlobalTable { get; private set; }

        //-----------------------------------------------------------
        private Stack<string> labelStorage;

        //-----------------------------------------------------------
        private string currentProgUnit;

        //-----------------------------------------------------------
        public SemanticAnalyzer()
        {
            GlobalTable = new Dictionary<string, SymbolTable>();
            labelStorage = new Stack<string>();
            currentProgUnit = "";
        }

        //-----------------------------------------------------------
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var table in GlobalTable)
            {
                sb.Append(table.Key + "\n");
                sb.Append(table.Value.ToString());
                sb.Append("\n");
            }
            return sb.ToString();
        }

        /**********************************************************************
         *                        Analyser Main Core
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type RunAnalyser(Program node)
        {
            FirstRun((Program) node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type FirstRun(Program node)
        {
            VisitFirstProgram((Identifier) node[0]);
            if (node.NodeChildrenCount() > 5)
            {
                for (int i = 5; i < node.NodeChildrenCount(); i++)
                {
                    VisitFirst((dynamic) node[i]);
                }
            }
            return Type.VOID;
        }

        /**********************************************************************
         *         First Run: Populate symbol tables with essential data
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type VisitFirstProgram(Identifier node)
        {
            var programName = node.AnchorToken.Lexeme;
            GlobalTable.Add(programName, new SymbolTable());
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type VisitFirst(Parameter node)
        {
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type VisitFirst(Function node)
        {
            var funcType = node[0];
            var funcName = node[1].AnchorToken.Lexeme;
            GlobalTable.Add(funcName, new SymbolTable());

            currentProgUnit = funcName;
            var funcTable = GlobalTable[currentProgUnit];

            foreach (var arg in node[2])
            {
                var argName = arg.AnchorToken.Lexeme;
                funcTable[argName] = new FunctionSymbolEntry(Type.NONE, true);
            }

            VisitFirst((DeclarationList) node[3]);
            return typeMapper[funcType.AnchorToken.Category];
        }

        //-----------------------------------------------------------
        public Type VisitFirst(Subroutine node)
        {
            var subrName = node[0].AnchorToken.Lexeme;
            GlobalTable.Add(subrName, new SymbolTable());

            currentProgUnit = subrName;
            var subrTable = GlobalTable[currentProgUnit];

            foreach (var arg in node[1])
            {
                var argName = arg.AnchorToken.Lexeme;
                subrTable[argName] = new FunctionSymbolEntry(Type.NONE, true);
            }
            VisitFirst((DeclarationList) node[2]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type VisitFirst(DeclarationList node)
        {
            foreach (var decl in node)
            {
                VisitFirst((dynamic) decl);
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type VisitFirst(Declaration node)
        {
            var funcTable = GlobalTable[currentProgUnit];

            foreach (var decl in node)
            {
                var variableName = decl.AnchorToken.Lexeme;
                if (!funcTable.Contains(variableName))
                {
                    funcTable[variableName] =
                        new FunctionSymbolEntry(
                        typeMapper[node.AnchorToken.Category]);
                }
                else
                {
                    funcTable[variableName].SymbolType =
                        typeMapper[node.AnchorToken.Category];
                }

                if (decl.HasChildren())
                {
                    var declArray = decl[0];
                    if (declArray.NodeChildrenCount() > 2)
                    {
                        throw new SemanticError(
                            "An array can have 2 dimensions at most: "
                            + variableName,
                            decl.AnchorToken);
                    }

                    for (int i = 0; i < declArray.NodeChildrenCount(); i++)
                    {
                        funcTable[variableName].Params
                            .Add(declArray[i].AnchorToken.Lexeme);
                    }
                }
            }
            return Type.VOID;
        }
    }
}
