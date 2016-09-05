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

namespace Fortran77_Compiler
{
	class Scanner
	{
		// Stores the source code file read from the console.
		readonly string input;
        
        static readonly Regex regex = new Regex(
           @"
                (?<Assign>      [=]      )
              | (?<Declaration> []       )
              | (?<Constant>    []       )
              | (?<Exponent>    [**]     )
              | (?<Mul>         [*]      )
              | (?<Div>         [/]      )
              | (?<Add>         [+]      )
              | (?<Neg>         [-]      )
              | (?<Less_t>      [.lt.]   )
              | (?<Less_or_e>   [.le.]   )
              | (?<Greater_t>   [.gt.]   )
              | (?<Greater_o_e> [.ge.]   )              
              | (?<Equal>       [.eq.]   )
              | (?<N_equal>     [.ne.]   )
              | (?<WhiteSpace>  [\s]     )
              | (?<Other>       .        ) 
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
                {"while", TokenCategory.WHILE}
            };
            
            static readonly IDictionary<string, TokenCategory> nonKeywords =
            new Dictionary<string, TokenCategory>() {
                              
            };

		// Constructor
		public Scanner(string input)
		{
			this.input = input;
		}

		// Enumerable in charge of parsing the input into tokens.
		public IEnumerable<Token> Start()
		{
			yield return new Token("", TokenCategory.TEST, 1, 1);
		}
	}
}
