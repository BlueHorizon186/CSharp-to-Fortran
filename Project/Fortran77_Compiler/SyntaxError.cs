using System;
using System.Collections.Generic;
using System.Text;

namespace Fortran77_Compiler
{
    class SyntaxError: Exception
    {
        public SyntaxError(TokenCategory expectedCategory, Token token):
            base(String.Format(
                "Syntax Error: Expecting {0} \n" +
                "but found {1} (\"{2}\") at row {3}, column {4}.",
                expectedCategory,
                token.Category,
                token.Lexeme,
                token.Row,
                token.Column)) {}
        
        public SyntaxError(ISet<TokenCategory> expectedCategories,
                           Token token):
            base(String.Format(
                "Syntax Error: Expecting one of {0}\n" +
                "but found {1} (\"{2}\") at row {3}, column {4}.",
                Elements(expectedCategories),
                token.Category,
                token.Lexeme,
                token.Row,
                token.Column)) {}
        
        static string Elements(ISet<TokenCategory> expectedCategories)
        {
            var sb = new StringBuilder("{");
            var first = true;
            foreach (var elem in expectedCategories)
            {
                if (first) first = false;
                else sb.Append(", ");
                sb.Append(elem);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
