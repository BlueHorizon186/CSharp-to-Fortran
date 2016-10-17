/*
  Fortran77 compiler - This class performs the lexical analysis,
  (a.k.a. scanning).
  Copyright (C) 2016, ITESM CEM

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Fortran77_Compiler
{
    class Scanner
    {
        // Stores the source code file read from the console.
        readonly string input;

        static readonly Regex regex = new Regex(
           @"
                (?<Add>            [+]                  )
              | (?<And>            [.](and)[.]          )
              | (?<Assign>         [=]                  )
              | (?<Comma>          [,]                  )
              | (?<Comment>        !.*                  )
              | (?<Div>            [/]                  )
              | (?<Equal>          [.](eq)[.]           )
              | (?<Exponent>       (\*\*)               )
              | (?<GreaterOrEqual> [.](ge)[.]           )
              | (?<GreaterThan>    [.](gt)[.]           )
              | (?<Identifier>     [a-zA-Z]+            )
              | (?<IntLiteral>     \d+                  )
              | (?<LessOrEqual>    [.](le)[.]           )
              | (?<LessThan>       [.](lt)[.]           )
              | (?<LogicLiteral>   [.](true|false)[.]   )
              | (?<Mul>            [*]                  )
              | (?<Neg>            [-]                  )
              | (?<Newline>        \n                   )
              | (?<Not>            [.](not)[.]          )
              | (?<NotEqual>       [.](ne)[.]           )
              | (?<Or>             [.](or)[.]           )
              | (?<ParLeft>        [(]                  )
              | (?<ParRight>       [)]                  )
              | (?<RealLiteral>	   (\d+[.]\d+)          )
              | (?<StringLiteral>  ['].*[']             )
              | (?<WhiteSpace>     [\s]                 )
              | (?<Other>          .*                   )
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"call", TokenCategory.CALL},
                {"character", TokenCategory.CHARACTER},
                {"common", TokenCategory.COMMON},
                {"continue", TokenCategory.CONTINUE},
                {"data", TokenCategory.DATA},
                {"do", TokenCategory.DO},
                {"else", TokenCategory.ELSE},
                {"elseif", TokenCategory.ELSEIF},
                {"end", TokenCategory.END},
                {"endif", TokenCategory.ENDIF},
                {"function", TokenCategory.FUNCTION},
                {"goto", TokenCategory.GOTO},
                {"if", TokenCategory.IF},
                {"integer", TokenCategory.INTEGER},
                {"logical", TokenCategory.LOGICAL},
                {"program", TokenCategory.PROGRAM},
                {"parameter", TokenCategory.PARAMETER},
                {"read", TokenCategory.READ},
                {"real", TokenCategory.REAL},
                {"return", TokenCategory.RETURN},
                {"stop", TokenCategory.STOP},
                {"subroutine", TokenCategory.SUBROUTINE},
                {"then", TokenCategory.THEN},
                {"while", TokenCategory.WHILE},
                {"write", TokenCategory.WRITE}
            };

        static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"Add", TokenCategory.ADD},
                {"And", TokenCategory.AND},
                {"Assign", TokenCategory.ASSIGN},
                {"Comma", TokenCategory.COMMA},
                {"Div", TokenCategory.DIV},
                {"Equal", TokenCategory.EQUAL},
                {"Exponent", TokenCategory.EXPONENT},
                {"GreaterOrEqual", TokenCategory.GREATER_OR_EQUAL},
                {"GreaterThan", TokenCategory.GREATER_THAN},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"LessOrEqual", TokenCategory.LESS_OR_EQUAL},
                {"LessThan", TokenCategory.LESS_THAN},
                {"LogicLiteral", TokenCategory.LOGIC_LITERAL},
                {"Mul", TokenCategory.MUL},
                {"Neg", TokenCategory.NEG},
                {"Not", TokenCategory.NOT},
                {"NotEqual", TokenCategory.NOT_EQUAL},
                {"Or", TokenCategory.OR},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"RealLiteral", TokenCategory.REAL_LITERAL},
                {"StringLiteral", TokenCategory.STRING_LITERAL}
            };

        // Constructor
        public Scanner(string input)
        {
            this.input = input;
        }

        // Enumerable in charge of parsing the input into tokens.
        public IEnumerable<Token> Start()
        {

            var row = 1;
            var columnStart = 0;

            Func<Match, TokenCategory, Token> generateToken = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);

            foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Length > 0) {

                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;
                    yield return generateToken(m, TokenCategory.EOL);

                } else if (m.Groups["WhiteSpace"].Length > 0
                    || m.Groups["Comment"].Length > 0) {

                    // Skip white space and comments.
                    continue;

                } else if (m.Groups["Identifier"].Length > 0) {

                    if (keywords.ContainsKey(m.Value)) {

                        // Matched string is a keyword.
                        yield return generateToken(m, keywords[m.Value]);

                    } else {

                        // Otherwise it's just a plain identifier.
                        yield return generateToken(m, TokenCategory.IDENTIFIER);
                    }

                } else if (m.Groups["Other"].Length > 0) {

                    // Found an illegal character.
                    yield return generateToken(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                    // Match must be one of the non keywords.
                    foreach (var name in nonKeywords.Keys) {
                        if (m.Groups[name].Length > 0) {
                            yield return generateToken(m, nonKeywords[name]);
                            break;
                        }
                    }
                }
            }

            yield return new Token(null,
                                   TokenCategory.EOF,
                                   row,
                                   input.Length - columnStart + 1);
        }
    }
}
