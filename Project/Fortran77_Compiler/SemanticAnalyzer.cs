using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

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
                { TokenCategory.FUNCTION, Type.FUNCTION },
                { TokenCategory.INTEGER, Type.INTEGER },
                { TokenCategory.LOGICAL, Type.LOGICAL },
                { TokenCategory.PROGRAM, Type.PROGRAM },
                { TokenCategory.REAL, Type.REAL },
                { TokenCategory.SUBROUTINE, Type.SUBROUTINE }
            };

        //-----------------------------------------------------------
        public IDictionary<string, List<SymbolTable>> Tables { get; private set; }

        //-----------------------------------------------------------
        private string progUnit;

        //-----------------------------------------------------------
        public SemanticAnalyzer()
        {
            Tables = new Dictionary<string, List<SymbolTable>>();
            progUnit = "";
        }

        //-----------------------------------------------------------
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (KeyValuePair<string, List<SymbolTable>> kvp in Tables)
            {
                sb.Append(kvp.Key + ":\n");
                foreach (var entry in kvp.Value)
                {
                    sb.Append(entry.ToString());
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public bool IdHasBeenFound(string id)
        {
            foreach (var entry in Tables)
            {
                foreach (var table in entry.Value)
                {
                    if (table.Contains(id))
                        return true;
                }
            }
            return false;
        }

        public Type GetIdType(string id)
        {
            foreach (var entry in Tables)
            {
                foreach (var table in entry.Value)
                {
                    if (table.Contains(id))
                        return table[id].SymbolType;
                }
            }
            return Type.NONE;
        }

        /**********************************************************************
         *                         Visiting Methods!
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type Visit(Program node)
        {
            string programKeyword = node.AnchorToken.Lexeme;
			Tables.Add(programKeyword, new List<SymbolTable>());
            Tables[programKeyword].Add(new SymbolTable());
            progUnit = programKeyword;

            // Add Program's name to Table.
            var programTable = Tables[progUnit].Last();
            string programName = node[0].AnchorToken.Lexeme;

            programTable[programName] =
                new SymbolEntry(typeMapper[node.AnchorToken.Category]);

            // Visit and check all declarations and statements in the program.
            Visit((dynamic) node[1]);
            Visit((dynamic) node[2]);

            if (node.NodeChildrenCount() > 5)
            {
                for (int i = 5; i < node.NodeChildrenCount(); i++)
                {
                    Visit((dynamic) node[i]);
                }
            }
            return Type.VOID;
        }

        /**********************************************************************
         *                       Visiting Declarations
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type Visit(DeclarationList node)
        {
            VisitChildren(node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Declaration node)
        {
            var currentTable = Tables[progUnit].Last();

            foreach (var decl in node)
            {
                var variableName = decl.AnchorToken.Lexeme;
                if (currentTable.Contains(variableName))
                {
                    throw new SemanticError(
                        "Duplicated variable: " + variableName,
                        decl.AnchorToken);
                }
                else
                {
                    currentTable[variableName] =
                        new SymbolEntry(typeMapper[node.AnchorToken.Category]);
                    
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
                            if (Visit((dynamic) declArray[i]) != Type.INTEGER)
                            {
                                throw new SemanticError(
                                    "Only integers can be used as Array dimensions.",
                                    declArray[i].AnchorToken);
                            }

                            if (declArray[i].AnchorToken.Category == TokenCategory.IDENTIFIER)
                            {
                                var dimName = declArray[i].AnchorToken.Lexeme;
                                if (!currentTable.Contains(dimName))
                                {
                                    throw new SemanticError(
                                        "Undeclared dimension: " + dimName,
                                        declArray[i].AnchorToken);
                                }

                                if (!currentTable[dimName].IsConstant)
                                {
                                    throw new SemanticError(
                                        "Cannot use non-constant variables as"
                                        + " array dimensions.",
                                        declArray[i].AnchorToken);
                                }
                            }

                            currentTable[variableName].Params
                                .Add(declArray[i].AnchorToken.Lexeme);
                        }
                    }
                }
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Parameter node)
        {
            var paramAssgn = node[0];
            var variableName = paramAssgn[0].AnchorToken.Lexeme;
            var currentTable = Tables[progUnit].Last();

            if (IdHasBeenFound(variableName))
            {
                if (!currentTable.Contains(variableName))
                {
                    currentTable[variableName] =
                        new SymbolEntry(GetIdType(variableName));
                }
                else
                {
                    if (currentTable[variableName].IsConstant)
                    {
                        throw new SemanticError(
                            "A variable can only be used in one parameter: "
                            + variableName,
                            paramAssgn[0].AnchorToken);
                    }
                }

                currentTable[variableName].IsConstant = true;
                var expectedType = currentTable[variableName].SymbolType;
                var foundType = Visit((dynamic) paramAssgn[1]);

                if (expectedType != foundType)
                {
                    throw new SemanticError(
                        "Expecting type " + expectedType
                        + " in assignment statement.",
                        paramAssgn[0].AnchorToken);
                }
            }
            else
            {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    paramAssgn[0].AnchorToken);
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Data node)
        {
            var currentTable = Tables[progUnit].Last();
            var variableName = node[0].AnchorToken.Lexeme;

            if (currentTable.Contains(variableName))
            {
                var dataEntry = currentTable[variableName];
                if (!dataEntry.Params.Any())
                {
                    throw new SemanticError(
                        "Cannot set data to a non-Array type: " + variableName,
                        node[0].AnchorToken);
                }

                var dataList = node[1];

                foreach (var n in dataList)
                {
                    if (Visit((dynamic) n) != dataEntry.SymbolType)
                    {
                        throw new SemanticError(
                            "Expecting type " + dataEntry.SymbolType
                            + " in Data Assignment.",
                            node[0].AnchorToken);
                    }
                }
            }
            else
            {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node[0].AnchorToken);
            }
            return Type.VOID;
        }

        /**********************************************************************
         *                       Visiting Statements
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type Visit(StatementList node)
        {
            VisitChildren(node);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Identifier node)
        {
            var currentTable = Tables[progUnit].Last();
            var variableName = node.AnchorToken.Lexeme;

            if (currentTable.Contains(variableName))
            {
                var id = currentTable[variableName];
                if (!id.Params.Any())
                    return currentTable[variableName].SymbolType;

                if (node[0] is FArray)
                {
                    if (node[0].NodeChildrenCount() != id.Params.Count)
                    {
                        throw new SemanticError(
                            "Wrong number of arguments when calling or accessing: "
                            + variableName,
                            node.AnchorToken);
                    }
                }
                return id.SymbolType;
            }

            throw new SemanticError(
                "Undeclared variable: " + variableName,
                node.AnchorToken);
        }

        //-----------------------------------------------------------
        public Type Visit(Assignment node)
        {
            var currentTable = Tables[progUnit].Last();
            var assgnBegin = 0;

            if (node[assgnBegin] is Label)
            {
                VisitLabel((Label) node[assgnBegin]);
                assgnBegin++;
            }

            var variableName = node[assgnBegin].AnchorToken.Lexeme;
            if (currentTable.Contains(variableName))
            {
                if (currentTable[variableName].IsConstant)
                {
                    throw new SemanticError(
                        "Variable was declared as constant: " + variableName,
                        node[assgnBegin].AnchorToken);
                }

                var expectedType = currentTable[variableName].SymbolType;
                var foundType = Visit((dynamic) node[assgnBegin + 1]);

                if (expectedType != foundType)
                {
                    throw new SemanticError(
                        "Expecting type " + expectedType
                        + " in assignment statement.",
                        node[assgnBegin].AnchorToken);
                }
            }
            else
            {
                throw new SemanticError(
                    "Undeclared variable: " + variableName,
                    node[assgnBegin].AnchorToken);
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(If node)
        {
            var assgnBegin = 0;

            if (node[assgnBegin] is Label)
            {
                VisitLabel((Label) node[assgnBegin]);
                assgnBegin++;
            }

            if (Visit((dynamic) node[assgnBegin]) != Type.LOGICAL)
            {
                throw new SemanticError(
                    "Expecting type " + Type.LOGICAL
                    + " in Conditional Statement.",
                    node.AnchorToken);
            }

            assgnBegin++;
            VisitChildren(node[assgnBegin]);
            assgnBegin++;

            if (node.NodeChildrenCount() > assgnBegin)
            {
                while (node[assgnBegin].AnchorToken.Category
                       == TokenCategory.ELSEIF)
                {
                    var elseIf = node[assgnBegin];
                    if (Visit((dynamic) elseIf[0]) != Type.LOGICAL)
                    {
                        throw new SemanticError(
                            "Expecting type " + Type.LOGICAL
                            + " in Conditional Statement.",
                            node.AnchorToken);
                    }

                    VisitChildren(elseIf[1]);
                    assgnBegin++;
                }

                if (node[assgnBegin] != null)
                {
                    VisitChildren(node[assgnBegin]);
                }
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(DoLoop node)
        {
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Write node)
        {
            foreach (var ch in node)
            {
                node.ExpressionTypes.Add(Visit((dynamic) ch));
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Read node)
        {
            foreach (var ch in node)
            {
                Visit((Identifier) ch);
            }
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(GoTo node)
        {
            var assgnBegin = 0;

            if (node.NodeChildrenCount() == 2)
            {
                VisitLabel((Label) node[assgnBegin]);
                assgnBegin++;
            }

            Visit((Label) node[assgnBegin]);
            return Type.VOID;
        }

        //-----------------------------------------------------------
        public Type Visit(Continue node)
        {
            if (node.HasChildren())
            {
                VisitLabel((Label) node[0]);
            }
            return Type.VOID;
        }

        /**********************************************************************
         *                       Visiting Literals
         * *******************************************************************/

        //-----------------------------------------------------------
        public Type Visit(StringLiteral node)
        {
            return Type.STRING;
        }

        //-----------------------------------------------------------
        public Type Visit(LogicLiteral node)
        {
            return Type.LOGICAL;
        }

        //-----------------------------------------------------------
        public Type Visit(IntLiteral node)
        {
            var intStr = node.AnchorToken.Lexeme;

            try
            {
                Convert.ToInt32(intStr);
            }
            catch (OverflowException)
            {
                throw new SemanticError(
                    "Integer literal too large to fit in memory: " + intStr,
                    node.AnchorToken);
            }
            return Type.INTEGER;
        }

        //-----------------------------------------------------------
        public Type Visit(RealLiteral node)
        {
            var realStr = node.AnchorToken.Lexeme;
            try
            {
                Convert.ToDouble(realStr);
            }
            catch (OverflowException)
            {
                throw new SemanticError(
                    "Real literal too large to fit in memory: " + realStr,
                    node.AnchorToken);
            }
            return Type.REAL;
        }

        //-----------------------------------------------------------
        public Type Visit(Label node)
        {
            var currentTable = Tables[progUnit].Last();
            var label = node.AnchorToken.Lexeme;

            if (!currentTable.Contains(label))
            {
                throw new SemanticError(
                    "Label has not been defined: " + label,
                    node.AnchorToken);
            }
            return Type.LABEL;
        }

        //-----------------------------------------------------------
        public Type VisitLabel(Label node)
        {
            var currentTable = Tables[progUnit].Last();
            var label = node.AnchorToken.Lexeme;

            if (currentTable.Contains(label))
            {
                throw new SemanticError(
                    "Label has already been used: " + label,
                    node.AnchorToken);
            }
            else
            {
                currentTable[label] =
                    new SymbolEntry(Type.LABEL);
            }
            return Type.LABEL;
        }

        /**********************************************************************
         *                       Auxiliary Functions
         * *******************************************************************/

        //-----------------------------------------------------------
        private void VisitChildren(Node node)
        {
            foreach (var ch in node)
            {
                Visit((dynamic) ch);
            }
        }
    }
}
