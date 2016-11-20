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
using System.IO;

namespace Fortran77_Compiler
{
    public class Driver
    {
        // Current compiler version.
        const string VERSION = "0.4";

        //-----------------------------------------------------------

        // Features the compiler includes.
        static readonly string[] ReleaseIncludes = {
            "Lexical Analysis",
            "Syntactic Analysis",
            "AST Construction",
            "Semantic Analysis"
        };

        //-----------------------------------------------------------

        // Information printed whenever the compiler is used.
        void PrintAppHeader() {
            Console.WriteLine("Fortran 77 compiler, version " + VERSION);
            Console.WriteLine("Copyright \u00A9 2016, ITESM CEM.");
            Console.WriteLine("This program is free software; you may "
                + "redistribute it under the terms of");
            Console.WriteLine("the GNU General Public License version 3 or "
                + "later.");
            Console.WriteLine("This program has absolutely no warranty.\n");
        }

        void PrintReleaseIncludes() {
            Console.WriteLine("Included in this release:");
            foreach (var phase in ReleaseIncludes) {
                Console.WriteLine("   * " + phase);
            }
            Console.Write("\n");
        }

        //-----------------------------------------------------------

        // Compiler's method to opening and interpreting the input file.
        void Run(string[] args)
        {
            // Print some general information.
            PrintAppHeader();
            Console.WriteLine();
            PrintReleaseIncludes();
            Console.WriteLine();

            // Check for input file and return an error if
            // it was not provided.
            if (args.Length != 2)
            {
                Console.Error.WriteLine(
                    "Please specify the name of the input and output files.");
                Environment.Exit(1);
            }

            // Read the input file and parse the tokens, or
            // return an error if the file was not found.
            try
            {
                // Read the source file.
                var inputPath = args[0];
                var inputCode = File.ReadAllText(inputPath);
                var parser = new Parser(new Scanner(inputCode).Start().GetEnumerator());
                var ast = parser.Program();
                //Console.WriteLine(program.ToStringTree());
                Console.WriteLine("Syntax OK!");

                var semantic = new SemanticAnalyzer();
                semantic.RunAnalyser((dynamic) ast);

                Console.WriteLine("Semantics OK.");
                Console.WriteLine();
                Console.WriteLine("Symbol Tables");
                Console.WriteLine("============\n");
                Console.WriteLine(semantic.ToString());

                var outputPath = args[1];
                var codeGen = new CILGenerator(semantic.GlobalTable);
                File.WriteAllText(outputPath,
                    codeGen.Visit((Program) ast));
                Console.WriteLine("Generated CIL code to '" + outputPath + "'.\n");
            }

            catch (Exception e)
            {
                if (e is FileNotFoundException
                    || e is SyntaxError
                    || e is SemanticError)
                {
                    Console.Error.WriteLine(e.Message);
                    Environment.Exit(1);
                }
                throw;
            }
        }

        public static void Main(string[] args)
        {
            // Begin the compiling process.
            new Driver().Run(args);
        }
    }
}