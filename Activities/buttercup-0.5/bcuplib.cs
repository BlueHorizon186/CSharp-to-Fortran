/*
  Buttercup runtime library.
  Copyright (C) 2013 Ariel Ortiz, ITESM CEM

  To compile this module as a DLL:

                mcs /t:library bcuplib.cs

  To link this DLL to a program written in C#:

                mcs /r:bcuplib.dll someprogram.cs

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

using System.Collections;

namespace Buttercup {
    
    using System;

    public class Utils {

        public static void Print(int i) {
            Console.WriteLine(i);
        }
        
        public static void Print(bool b) {
            Console.WriteLine(b ? "#t" : "#f");
        }
    }
}
