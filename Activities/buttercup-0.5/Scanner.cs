/*
  Buttercup compiler - This class performs the lexical analysis, 
  (a.k.a. scanning).
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM
  
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
using System.Text;
using System.Text.RegularExpressions;

namespace Buttercup {

    class Scanner {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"                             
                (?<And>        [&]       )
              | (?<Assign>     [=]       )
              | (?<Comment>    ;.*       )
              | (?<False>      [#]f      )
              | (?<Identifier> [a-zA-Z]+ )
              | (?<IntLiteral> \d+       )
              | (?<Less>       [<]       )
              | (?<Mul>        [*]       )
              | (?<Neg>        [-]       )
              | (?<Newline>    \n        )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<Plus>       [+]       )              
              | (?<True>       [#]t      )
              | (?<WhiteSpace> \s        )     # Must go anywhere after Newline.
              | (?<Other>      .         )     # Must be last: match any other character.
            ", 
            RegexOptions.IgnorePatternWhitespace 
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"bool", TokenCategory.BOOL},
                {"end", TokenCategory.END},
                {"if", TokenCategory.IF},
                {"int", TokenCategory.INT},
                {"print", TokenCategory.PRINT},
                {"then", TokenCategory.THEN}
            };

        static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"And", TokenCategory.AND},
                {"Assign", TokenCategory.ASSIGN},
                {"False", TokenCategory.FALSE},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Less", TokenCategory.LESS},
                {"Mul", TokenCategory.MUL},
                {"Neg", TokenCategory.NEG},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"Plus", TokenCategory.PLUS},
                {"True", TokenCategory.TRUE}                
            };

        public Scanner(string input) {
            this.input = input;
        }

        public IEnumerable<Token> Start() {

            var row = 1;
            var columnStart = 0;

            Func<Match, TokenCategory, Token> newTok = (m, tc) =>
                new Token(m.Value, tc, row, m.Index - columnStart + 1);

            foreach (Match m in regex.Matches(input)) {

                if (m.Groups["Newline"].Length > 0) {

                    // Found a new line.
                    row++;
                    columnStart = m.Index + m.Length;

                } else if (m.Groups["WhiteSpace"].Length > 0 
                    || m.Groups["Comment"].Length > 0) {

                    // Skip white space and comments.

                } else if (m.Groups["Identifier"].Length > 0) {

                    if (keywords.ContainsKey(m.Value)) {

                        // Matched string is a Buttercup keyword.
                        yield return newTok(m, keywords[m.Value]);                                               

                    } else { 

                        // Otherwise it's just a plain identifier.
                        yield return newTok(m, TokenCategory.IDENTIFIER);
                    }

                } else if (m.Groups["Other"].Length > 0) {

                    // Found an illegal character.
                    yield return newTok(m, TokenCategory.ILLEGAL_CHAR);

                } else {

                    // Match must be one of the non keywords.
                    foreach (var name in nonKeywords.Keys) {
                        if (m.Groups[name].Length > 0) {
                            yield return newTok(m, nonKeywords[name]);
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
