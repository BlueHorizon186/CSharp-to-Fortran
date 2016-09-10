//---------------------------------------------------------
// Student Name: Ivan David Diaz Sanchez
// Student ID: A01371166
//---------------------------------------------------------

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
    class Question2 {
        public static void Main(String[] args) {
            // Your code goes here.
            var regex = new Regex(@"0[Bb][01]+([_]*[01]+)*[Ll]?\b");
            var input = File.ReadAllText(args[0]);

            foreach(Match m in regex.Matches(input))
            {
                Console.WriteLine(m.Value);
            }
        }
    }
}