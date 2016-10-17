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
    enum TokenCategory
    {
        ADD,
        AND,
        ASSIGN,
        CALL,
        CHARACTER,
        COMMA,
        COMMON,
        CONTINUE,
        DATA,
        DIV,
        DO,
        ELSE,
        ELSEIF,
        END,
        ENDIF,
        EOF,
        EOL,
        EQUAL,
        EXPONENT,
        FUNCTION,
        GOTO,
        GREATER_OR_EQUAL,
        GREATER_THAN,
        IDENTIFIER,
        IF,
        ILLEGAL_CHAR,
        INTEGER,
        INT_LITERAL,
        LESS_OR_EQUAL,
        LESS_THAN,
        LOGICAL,
        LOGIC_LITERAL,
        MUL,
        NEG,
        NOT,
        NOT_EQUAL,
        OR,
        PARAMETER,
        PARENTHESIS_CLOSE,
        PARENTHESIS_OPEN,
        PROGRAM,
        READ,
        REAL,
        REAL_LITERAL,
        RETURN,
        STOP,
        STRING_LITERAL,
        SUBROUTINE,
        SUBS,
        THEN,
        WHILE,
        WRITE
    }
}

