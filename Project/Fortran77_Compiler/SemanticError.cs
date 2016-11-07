using System;

namespace Fortran77_Compiler
{
    class SemanticError: Exception
    {
        public SemanticError(string message, Token token):
            base(String.Format(
                "Semantic Error: {0} \n" +
                "at row {1}, column {2}.",
                message,
                token.Row,
                token.Column)) {}
    }
}
