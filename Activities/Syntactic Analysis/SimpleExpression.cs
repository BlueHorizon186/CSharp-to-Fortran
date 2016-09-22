using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum TokenCategory
{
    PLUS, TIMES, POW, PAR_OPEN, PAR_CLOSE, INT, EOF, ILLEGAL
}

public class Token
{
    public TokenCategory Category { get; }
    public String Lexeme { get; }

    public Token(TokenCategory category, String lexeme)
    {
        Category = category;
        Lexeme = lexeme;
    }

    public override String ToString()
    {
        return String.Format("[{0}, \"{1}\"]", Category, Lexeme);
    }
}

public class Scanner
{
    readonly String input;
    static readonly Regex regex = new Regex(@"([+])|([*])|([(])|([)])|(\d+)|(\s)|([\^])|(.)");

    public Scanner(String input)
    {
        this.input = input;
    }

    public IEnumerable<Token> Start()
    {
        foreach (Match m in regex.Matches(input))
        {
            if (m.Groups[1].Length > 0) {
                yield return new Token(TokenCategory.PLUS, m.Value);
            } else if (m.Groups[2].Length > 0) {
                yield return new Token(TokenCategory.TIMES, m.Value);
            } else if (m.Groups[3].Length > 0) {
                yield return new Token(TokenCategory.PAR_OPEN, m.Value);
            } else if (m.Groups[4].Length > 0) {
                yield return new Token(TokenCategory.PAR_CLOSE, m.Value);
            } else if (m.Groups[5].Length > 0) {
                yield return new Token(TokenCategory.INT, m.Value);
            } else if (m.Groups[6].Length > 0) {
                continue;
            } else if (m.Groups[7].Length > 0) {
                yield return new Token(TokenCategory.POW, m.Value);
            } else if (m.Groups[8].Length > 0) {
                yield return new Token(TokenCategory.ILLEGAL, m.Value);
            }
        }
        yield return new Token(TokenCategory.EOF, "");
    }
}

class SyntaxError: Exception { }

public class Parser
{
    IEnumerator<Token> tokenStream;

    public Parser(IEnumerator<Token> tokenStream)
    {
        this.tokenStream = tokenStream;
        this.tokenStream.MoveNext();
    }

    public TokenCategory Current
    {
        get { return tokenStream.Current.Category; }
    }

    public Token Expect(TokenCategory category)
    {
        if (Current == category) {
            Token current = tokenStream.Current;
            tokenStream.MoveNext();
            return current;
        } else {
            throw new SyntaxError();
        }
    }

    public int Prog()
    {
        var x = Exp();
        Expect(TokenCategory.EOF);
        return x;
    }

    public int Exp()
    {
        var x = Term();
        while (Current == TokenCategory.PLUS)
        {
            Expect(TokenCategory.PLUS);
            x += Term();
        }
        return x;
    }

    public int Term()
    {
        var x = Other();
        while (Current == TokenCategory.TIMES)
        {
            Expect(TokenCategory.TIMES);
            x *= Other();
        }
        return x;
    }

    public int Other()
    {
        var x = Factor();
        if (Current == TokenCategory.POW)
        {
            Expect(TokenCategory.POW);
            x = pow(x, Other());
        }
        return x;
    }

    public int Factor()
    {
        if (Current == TokenCategory.INT)
        {
            var t = Expect(TokenCategory.INT);
            return Convert.ToInt32(t.Lexeme);
        }
        else
        {
            Expect(TokenCategory.PAR_OPEN);
            var x = Exp();
            Expect(TokenCategory.PAR_CLOSE);
            return x;
        }
    }

    public static int pow(int x, int y)
    {
        var result = 1;
        for (var i = 1; i <= y; i++) result *= x;
        return result;
    }
}

public class SimpleExpression
{
    public static void Main()
    {
        var line = Console.ReadLine();
        var parser = new Parser(new Scanner(line).Start().GetEnumerator());

        try
        {
            var y = parser.Prog();
            Console.WriteLine("Evaluated Result: {0}", y);
        }
        catch (SyntaxError e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
