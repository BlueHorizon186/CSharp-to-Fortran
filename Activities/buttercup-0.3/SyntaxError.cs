/*
  Buttercup compiler - Syntax error exception class.
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

namespace Buttercup {

    class SyntaxError: Exception {

        public SyntaxError(TokenCategory expectedCategory,
                           Token token):
            base(String.Format(
                "Syntax Error: Expecting {0} \n" +
                "but found {1} (\"{2}\") at row {3}, column {4}.",
                expectedCategory,
                token.Category,
                token.Lexeme,
                token.Row,
                token.Column)) {
        }

        public SyntaxError(ISet<TokenCategory> expectedCategories,
                           Token token):
            base(String.Format(
                "Syntax Error: Expecting one of {0}\n" +
                "but found {1} (\"{2}\") at row {3}, column {4}.",
                Elements(expectedCategories),
                token.Category,
                token.Lexeme,
                token.Row,
                token.Column)) {
        }

        static string Elements(ISet<TokenCategory> expectedCategories) {
            var sb = new StringBuilder("{");
            var first = true;
            foreach (var elem in expectedCategories) {
                if (first) {
                    first = false;
                } else {
                    sb.Append(", ");
                }
                sb.Append(elem);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
