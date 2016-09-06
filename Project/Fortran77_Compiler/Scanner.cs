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
                (?<Assign>         [=]       )
              | (?<And>            [.](and)[.]   )
	          | (?<Or>             [.](or)[.]   )
	          | (?<Not>            [.](not)[.]   )
              | (?<Int_constant>   [0-9]+    )
              | (?<Real_constant>  [\d+[.]\d+])
              | (?<Logic_constant> [.]([true]|[false])+[.] )
              
              | (?<Exponent>       [**]      )
              | (?<Mul>            [*]       )
              | (?<Div>            [/]       )
              | (?<Add>            [+]       )
              | (?<Neg>            [-]       )
              | (?<Less_t>         [.](lt)[.]    )
              | (?<Less_or_e>      [.](le)[.]   )
              | (?<Greater_t>      [.](gt)[.]    )
              | (?<Greater_o_e>    [.](ge)[.]   )              
              | (?<Equal>          [.](eq)[.]    )
              | (?<N_equal>        [.](ne)[.]    )
              | (?<Identifier>     [a-zA-Z]+ )
              | (?<WhiteSpace>     [\s]      )
              | (?<Comment>        !.+       )
              | (?<Other>          .         ) 
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );
            
            static readonly IDictionary<string, TokenCategory> keywords =
            new Dictionary<string, TokenCategory>() {
                {"if", TokenCategory.IF},
                {"then", TokenCategory.THEN},
                {"else", TokenCategory.ELSE},
                {"do", TokenCategory.DO},
                {"stop", TokenCategory.STOP},
                {"end", TokenCategory.END},
                {"while", TokenCategory.WHILE},
                {"continue", TokenCategory.CONTINUE},
                {"write", TokenCategory.WRITE},
                {"read", TokenCategory.READ},
                {"goto", TokenCategory.GOTO},
                {"endif", TokenCategory.ENDIF},
                {"program", TokenCategory.PROGRAM},
                {"return", TokenCategory.RETURN},
                {"call", TokenCategory.CALL},
                {"common", TokenCategory.COMMON},
                {"data", TokenCategory.DATA}
            };
            
            static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                {"Assign", TokenCategory.ASSIGN},
                {"And", TokenCategory.AND},
		        {"Or", TokenCategory.OR},
	        	{"Not", TokenCategory.NOT},
                {"Int_constant", TokenCategory.INT_CONSTANT},
                {"Real_constant", TokenCategory.REAL_CONSTANT},
                {"Logic_constant", TokenCategory.LOGIC_CONSTANT},
                //{"Char_constant", TokenCategory.CHAR_CONSTANT},
                {"Exponent", TokenCategory.EXPONENT},
                {"Mul", TokenCategory.MUL},
                {"Div", TokenCategory.DIV},
                {"Add", TokenCategory.ADD},
                {"Neg", TokenCategory.NEG},
                {"less_t", TokenCategory.LESS_THAN},
                {"Less_or_e", TokenCategory.LESS_OR_EQUAL},
                {"Greater_t", TokenCategory.GREATER_THAN},
                {"Greater_o_e", TokenCategory.GREATER_OR_EQUAL},
                {"Equal", TokenCategory.EQUAL},
                {"N_equal", TokenCategory.NOT_EQUAL}
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

                        // Matched string is a keyword.
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
