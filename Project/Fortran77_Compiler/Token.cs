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

namespace Fortran77_Compiler
{
	class Token
	{
		// Stores the actual text of this token.
		readonly string lexeme;

		// Stores which category this token belongs to.
		readonly TokenCategory category;

		// Stores which line or row this token was found in.
		readonly int row;

		// Stores which column this token was found in.
		readonly int column;

		// Properties to access the Token's fields.
		public string Lexeme { get { return lexeme; }}
		public TokenCategory Category { get { return category; }}
		public int Row { get { return row; }}
		public int Column { get { return column; }}

		// Token Constructor
		public Token(
			string lexeme,
			TokenCategory category,
			int row,
			int column)
		{
			this.lexeme = lexeme;
			this.category = category;
			this.row = row;
			this.column = column;
		}

		// ToString method used to display the tokens accordingly.
		public override string ToString()
		{
			return string.Format("{{{0}, \"{1}\", @({2}, {3})}}",
				category, lexeme, row, column);
		}
	}
}

