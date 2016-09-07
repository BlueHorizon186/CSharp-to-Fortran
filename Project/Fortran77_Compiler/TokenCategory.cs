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

namespace Fortran77_Compiler
{
	// Defines the possible categories a Token may belong to.
	// IMPORTANT: Enum elements sorting pending... 
	enum TokenCategory
	{
		ADD,
		AND,
		ASSIGN, 
        OR,
        NOT,
        INT_LITERAL,
        REAL_LITERAL,
        LOGIC_LITERAL,
        STRING_LITERAL,
        ILLEGAL_CHAR,
        EXPONENT,
        MUL,
        EOF,
        DIV,
        SUBS,
        NEG,
        LESS_THAN,
        LESS_OR_EQUAL,
        GREATER_THAN,
        GREATER_OR_EQUAL,
        EQUAL,
        NOT_EQUAL,
        IF,
        THEN,
        ELSE,
        DO,
        WHILE,
        STOP,
        IDENTIFIER,
        END,
        CONTINUE,
        WRITE,
        READ,
        GOTO,
        ENDIF,
        PROGRAM,
        RETURN,
        CALL,
        COMMON,
        DATA
	}
}

