//---------------------------------------------------------
// Student Name: Ivan David Diaz Sanchez
// Student ID: A01371166
//---------------------------------------------------------

using System;
using System.IO;
using System.Text.RegularExpressions;

namespace Exam1 {
    class Question1 {
        public static void Main(String[] args) {
            // Your code goes here.
            var regex_tokens = new Regex(@"(\w+)|(\n)");
            var regex_chars = new Regex(@"(.)|(\s)");
            var input = File.ReadAllText(args[0]);

            uint words = 0;
            uint newlines = 0;
            uint characters = 0;

            foreach(Match m in regex_tokens.Matches(input))
            {
                if (m.Groups[1].Length > 0) words++;
                else if (m.Groups[2].Length > 0) newlines++;
            }

            foreach(Match m in regex_chars.Matches(input))
            {
                if (m.Groups[0].Length > 0) characters++;
            }

            Console.WriteLine("{0} {1} {2}", newlines, words, characters);
        }
    }
}