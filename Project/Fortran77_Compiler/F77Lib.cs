
namespace Fortran77_Compiler
{
    using System;

    public class Utils
    {
        public static void Print(string s)
        {
            Console.WriteLine(s);
        }

        public static void Print(int i)
        {
            Console.WriteLine(i.ToString());
        }

        public static void Print(float d)
        {
            Console.WriteLine(d.ToString());
        }

        public static void Print(bool b)
        {
            Console.WriteLine(b ? "true" : "false");
        }
    }
}
