using System;

public class DynamicExample
{
    static void M(int a)
    {
        Console.WriteLine("I'm an int: {0}", a);
    }

    static void M(string a)
    {
        Console.WriteLine("I'm a string: {0}", a);
    }

    static void Main(string[] args)
    {
        dynamic x = 5;
        M(x);
        x = "Thanks for all the fish!";
        M(x);
    }
}
