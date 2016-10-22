//---------------------------------------------------------
// Student Name: Ivan David Diaz Sanchez
// Student ID: A01371166
//---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public enum TokenCategory {
    PREP, ADJ, ARTICLE, NAME, NOUN, VERB, PRONOUN, EOF, ILLEGAL
}

public class Token {
    public TokenCategory Category { get; }
    public String Lexeme { get; }
    public Token(TokenCategory category, String lexeme) {
        Category = category;
        Lexeme = lexeme;
    }
    public override String ToString() {
        return String.Format("[{0}, \"{1}\"]", Category, Lexeme);
    }
}

public class Scanner {
    readonly String input;
    static readonly IDictionary<String, TokenCategory> words =
        new Dictionary<String, TokenCategory>() {
            {"to", TokenCategory.PREP},
            {"in", TokenCategory.PREP},
            {"by", TokenCategory.PREP},
            {"with", TokenCategory.PREP},
            {"on", TokenCategory.PREP},
            {"big", TokenCategory.ADJ},
            {"little", TokenCategory.ADJ},
            {"blue", TokenCategory.ADJ},
            {"green", TokenCategory.ADJ},
            {"the", TokenCategory.ARTICLE},
            {"a", TokenCategory.ARTICLE},
            {"dick", TokenCategory.NAME},
            {"jane", TokenCategory.NAME},
            {"janet", TokenCategory.NAME},
            {"mark", TokenCategory.NAME},
            {"man", TokenCategory.NOUN},
            {"ball", TokenCategory.NOUN},
            {"woman", TokenCategory.NOUN},
            {"table", TokenCategory.NOUN},
            {"hit", TokenCategory.VERB},
            {"took", TokenCategory.VERB},
            {"saw", TokenCategory.VERB},
            {"liked", TokenCategory.VERB},
            {"he", TokenCategory.PRONOUN},
            {"she", TokenCategory.PRONOUN},
            {"it", TokenCategory.PRONOUN},
            {"these", TokenCategory.PRONOUN},
            {"those", TokenCategory.PRONOUN},
            {"that", TokenCategory.PRONOUN}
        };
    static readonly Regex regex =
        new Regex(
            @"(\w+)|(\s)|(.)",
            RegexOptions.Compiled | RegexOptions.Multiline
        );
    public Scanner(String input) {
        this.input = input;
    }
    public IEnumerable<Token> Start() {
        foreach (Match m in regex.Matches(input)) {
            if (m.Groups[1].Length > 0) {
                if (words.ContainsKey(m.Value)) {
                    yield return new Token(words[m.Value], m.Value);
                } else {
                    yield return new Token(TokenCategory.ILLEGAL, m.Value);
                }
            } else if (m.Groups[2].Length > 0) {
                continue;
            } else if (m.Groups[3].Length > 0) {
                yield return new Token(TokenCategory.ILLEGAL, m.Value);
            }
        }
        yield return new Token(TokenCategory.EOF, "");
    }
}

class SyntaxError: Exception {
}

public class Parser {
    IEnumerator<Token> tokenStream;
    public Parser(IEnumerator<Token> tokenStream) {
        this.tokenStream = tokenStream;
        this.tokenStream.MoveNext();
    }
    public TokenCategory Current {
        get { return tokenStream.Current.Category; }
    }
    public Token Expect(TokenCategory category) {
        if (Current == category) {
            Token current = tokenStream.Current;
            tokenStream.MoveNext();
            return current;
        } else {
            throw new SyntaxError();
        }
    }
    
    /*********************************************************************
     *                     Main Sentence Structure
     ********************************************************************/

    public void Sentence() {
        NounPhrase();
        VerbPhrase();
        Expect(TokenCategory.EOF);
    }

    private void NounPhrase() {
        switch (Current) {
            case TokenCategory.ARTICLE:
                Expect(TokenCategory.ARTICLE);
                AdjList();
                Expect(TokenCategory.NOUN);
                PPList();
                break;
            
            case TokenCategory.NAME:
                Expect(TokenCategory.NAME);
                break;
            
            case TokenCategory.PRONOUN:
                Expect(TokenCategory.PRONOUN);
                break;

            default:
                throw new SyntaxError();
        }
    }

    private void VerbPhrase() {
        Expect(TokenCategory.VERB);
        NounPhrase();
        PPList();
    }

    /*********************************************************************
     *                        Sentence Sections
     ********************************************************************/

     private void AdjList() {
         while (Current == TokenCategory.ADJ)
            Expect(TokenCategory.ADJ);
     }

     private void PPList() {
         while (Current == TokenCategory.PREP)
            PP();
     }

     private void PP() {
         Expect(TokenCategory.PREP);
         NounPhrase();
     }
}

public class English {
    public static void Main(String[] args) {
        Console.Write("Input sentence: ");
        var line = Console.ReadLine();
        var parser = new Parser(new Scanner(line).Start().GetEnumerator());
        try {
            parser.Sentence();
            Console.WriteLine("Syntax fine.");
        } catch (SyntaxError) {
            Console.WriteLine("Bad syntax.");
        }
    }
}