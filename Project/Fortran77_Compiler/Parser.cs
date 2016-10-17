using System;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    class Parser
    {
        /*******************************************************************
         * IMPORTANT NOTICE: This is just a template based on Buttercup's
         * 2nd Stage Parser. Adding and/or removing Sets and Methods will
         * most likely be required for the Fortran77 Version.
         *******************************************************************/
         /*
        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                // Here will go the declaration keywords.
            };

        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                // Here will go the statement keywords.
            };
            
        static readonly ISet<TokenCategory> firstOfOperator =
            new HashSet<TokenCategory>() {
                // Here will go the operator keywords.
            };
            */
        static readonly ISet<TokenCategory> equalitySymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.EQUAL,
                TokenCategory.NOT_EQUAL
            };
        
        static readonly ISet<TokenCategory> comparingSymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.GREATER_OR_EQUAL,
                TokenCategory.GREATER_THAN,
                TokenCategory.LESS_OR_EQUAL,
                TokenCategory.LESS_THAN
            };
        
        static readonly ISet<TokenCategory> additionSymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.ADD,
                TokenCategory.NEG
            };
        
        static readonly ISet<TokenCategory> multiplicationSymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.MUL,
                TokenCategory.DIV
            }
        
        static readonly ISet<TokenCategory> negationSymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.NOT,
                TokenCategory.NEG
            }

        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.CALL,
                TokenCategory.IDENTIFIER,
                TokenCategory.INT_LITERAL,
                TokenCategory.LOGIC_LITERAL,
                TokenCategory.REAL_LITERAL,
                TokenCategory.STRING_LITERAL,
                TokenCategory.PARENTHESIS_OPEN
            };

        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        /****************************************************************
         ***************************************************************/

        public Token Expect(TokenCategory category)
        {
            if (CurrentToken == category)
            {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            }
            
            else
            {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public void Program()
        {
            while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
            }

            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }

            Expect(TokenCategory.EOF);
        }
        /*
        public void Declaration() {
            
        }

        public void Statement() {

        }

        public void Type() {
            
        }

        public void Assignment() {
            
        }

        public void Print() {
            
        }

        public void If() {
            
        }
        */

        /*****************************************************************
         *                      Expression Methods
         ****************************************************************/

        public void Expression()
        {
            SimpleExpression();
            while (firstOfSimpleExpression.Contains(CurrentToken))
            {
                OrExpression();
            }
        }

        public void OrExpression()
        {
            AndExpression();
            if (CurrentToken == TokenCategory.OR)
            {
                Expect(TokenCategory.OR);
                AndExpression();
            }
        }

        public void AndExpression()
        {
            EqualityExpression();
            if (CurrentToken == TokenCategory.AND)
            {
                Expect(TokenCategory.AND);
                EqualityExpression();
            }
        }

        public void EqualityExpression()
        {
            CompExpression();
            if (equalitySymbols.Contains(CurrentToken))
            {
                EqualityOperator();
                CompExpression();
            }
        }

        public void CompExpression()
        {
            AddExpression();
            if (comparingSymbols.Contains(CurrentToken))
            {
                ComparingOperator();
                AddExpression();
            }
        }

        public void AddExpression()
        {
            MultExpression();
            while (additionSymbols.Contains(CurrentToken))
            {
                AdditionOperator();
                MultExpression();
            }
        }

        public void MultExpression()
        {
            PowExpression();
            while (multiplicationSymbols.Contains(CurrentToken))
            {
                MultiplicationOperator();
                PowExpression();
            }
        }

        public void PowExpression()
        {
            NegExpression();
            if (CurrentToken == TokenCategory.EXPONENT)
            {
                Expect(TokenCategory.EXPONENT);
                PowExpression();
            }
        }

        public void NegExpression()
        {
            if (negationSymbols.Contains(CurrentToken))
                NegationOperator();
            SimpleExpression();
        }

        public void SimpleExpression()
        {
            switch (CurrentToken)
            {
                case TokenCategory.CALL:
                    Expect(TokenCategory.CALL);
                    IdentifierFound();
                    break;

                case TokenCategory.IDENTIFIER:
                    IdentifierFound();
                    break;
                
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                
                case TokenCategory.LOGIC_LITERAL:
                    Expect(TokenCategory.LOGIC_LITERAL);
                    break;
                
                case TokenCategory.REAL_LITERAL:
                    Expect(TokenCategory.REAL_LITERAL);
                    break;

                case TokenCategory.STRING_LITERAL:
                    Expect(TokenCategory.STRING_LITERAL);
                    break;

                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    Expression();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    break;

                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                          tokenStream.Current);
            }
        }

        /*****************************************************************
         *                  Operator Auxiliary Methods
         ****************************************************************/

        public void EqualityOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.EQUAL:
                    Expect(TokenCategory.EQUAL);
                    break;
                
                case TokenCategory.NOT_EQUAL:
                    Expect(TokenCategory.NOT_EQUAL);
                    break;
                
                default:
                    throw new SyntaxError(equalitySymbols,
                                          tokenStream.Current);
            }
        }

        public void ComparingOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.GREATER_OR_EQUAL:
                    Expect(TokenCategory.GREATER_OR_EQUAL);
                    break;
                
                case TokenCategory.GREATER_THAN:
                    Expect(TokenCategory.GREATER_THAN);
                    break;
                
                case TokenCategory.LESS_OR_EQUAL:
                    Expect(TokenCategory.LESS_OR_EQUAL);
                    break;
                
                case TokenCategory.LESS_THAN:
                    Expect(TokenCategory.LESS_THAN);
                    break;
                
                default:
                    throw new SyntaxError(comparingSymbols,
                                          tokenStream.Current);
            }
        }

        public void AdditionOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.ADD:
                    Expect(TokenCategory.ADD);
                    break;
                
                case TokenCategory.NEG:
                    Expect(TokenCategory.NEG);
                    break;
                
                default:
                    throw new SyntaxError(additionSymbols,
                                          tokenStream.Current);
            }
        }

        public void MultiplicationOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.MUL:
                    Expect(TokenCategory.MUL);
                    break;
                
                case TokenCategory.DIV:
                    Expect(TokenCategory.DIV);
                    break;
                
                default:
                    throw new SyntaxError(multiplicationSymbols,
                                          tokenStream.Current);
            }
        }

        public void NegationOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    break;
                
                case TokenCategory.NEG:
                    Expect(TokenCategory.NEG);
                    break;
                
                default:
                    throw new SyntaxError(negationSymbols,
                                          tokenStream.Current);
            }
        }

        private void IdentifierFound()
        {
            Expect(TokenCategory.IDENTIFIER);
            if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
            {
                Expect(TokenCategory.PARENTHESIS_OPEN);
                if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
                {
                    Expression();
                    while (CurrentToken == TokenCategory.COMMA)
                    {
                        Expect(TokenCategory.COMMA);
                        Expression();
                    }
                }
                Expect(TokenCategory.PARENTHESIS_CLOSE);
            }
        }
    }
}
