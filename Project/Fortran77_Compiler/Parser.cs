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
         
        static readonly ISet<TokenCategory> firstOfDeclaration =
            new HashSet<TokenCategory>() {
                // Here will go the declaration keywords.
                TokenCategory.INTEGER,
                TokenCategory.REAL,
                TokenCategory.LOGICAL,
                TokenCategory.CHARACTER,
                TokenCategory.PARAMETER,
                TokenCategory.DATA,
                TokenCategory.COMMON
            };

        static readonly ISet<TokenCategory> firstOfMultipleDeclarations =
            new HashSet<TokenCategory>() {
                TokenCategory.COMMA,
                TokenCategory.PARENTHESIS_OPEN
            };
        
        static readonly ISet<TokenCategory> literals =
            new HashSet<TokenCategory>() {
                TokenCategory.INT_LITERAL,
                TokenCategory.LOGIC_LITERAL,
                TokenCategory.REAL_LITERAL,
                TokenCategory.STRING_LITERAL
            };
        
        static readonly ISet<TokenCategory> firstOfStatement =
            new HashSet<TokenCategory>() {
                // Here will go the statement keywords.
                TokenCategory.CALL,
                TokenCategory.CONTINUE,
                TokenCategory.DO,
                TokenCategory.GOTO,
                TokenCategory.IDENTIFIER,
                TokenCategory.IF,
                TokenCategory.READ,
                TokenCategory.WRITE
            };

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
            };
        
        static readonly ISet<TokenCategory> negationSymbols =
            new HashSet<TokenCategory>() {
                TokenCategory.NOT,
                TokenCategory.NEG
            };

        // Missing some keywords. Add them in Scanner.cs
        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                TokenCategory.CALL,
                TokenCategory.IDENTIFIER,
                TokenCategory.INT_LITERAL,
                TokenCategory.LOGIC_LITERAL,
                TokenCategory.REAL,
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

        public TokenCategory CurrentToken 
        {
            get { return tokenStream.Current.Category; }
        }

        /****************************************************************
         *                  Expecting Tokens Methods
         ***************************************************************/

        public Token Expect(TokenCategory category)
        {
            if (CurrentToken == category)
            {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                CheckForLabel();
                CheckForLineContinuation();
                return current;
            }
            
            else
            {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public void ExpectLiteral()
        {
            switch (CurrentToken)
            {
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
                
                default:
                    throw new SyntaxError(literals, tokenStream.Current);
            }
        }

        /****************************************************************
         *                          Begin!!!
         ***************************************************************/
        
        /****************************************************************
         *                          Program Node
         ***************************************************************/

        public void Program()
        {
            Expect(TokenCategory.PROGRAM);
            Expect(TokenCategory.IDENTIFIER);

            EvaluateDeclarations();
            EvaluateStatements();

            Expect(TokenCategory.STOP);
            Expect(TokenCategory.END);

            while (CurrentToken != TokenCategory.EOF)
            {
                if (CurrentToken == TokenCategory.SUBROUTINE)
                    Subroutine();
                else
                    Function();
            }
            
            Expect(TokenCategory.EOF);
        }

        /****************************************************************
         *                    Declarations List Node
         ***************************************************************/
        
        private void EvaluateDeclarations()
        {
            while (firstOfDeclaration.Contains(CurrentToken))
            {
                if (CurrentToken == TokenCategory.PARAMETER) Parameter();
                else if (CurrentToken == TokenCategory.COMMON) Common();
                else if (CurrentToken == TokenCategory.DATA) Data();
                else Declaration();
                // TODO: Missing "Common" implementation. Done!!!
            }
        }

        /****************************************************************
         *                    Statements List Node
         ***************************************************************/
        
        private Node EvaluateStatements()
        {
            var stmts = new StatementList();
            while (firstOfStatement.Contains(CurrentToken))
            {
                stmts.Add(Statement());
            }
            return stmts;
        }

        /****************************************************************
         *                       Declaration Node
         ***************************************************************/
        
        public void Declaration()
        {
            Type();
            Expect(TokenCategory.IDENTIFIER);

            while (firstOfMultipleDeclarations.Contains(CurrentToken))
            {
                if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                    ArrayDeclaration();
                else if (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    Expect(TokenCategory.IDENTIFIER);
                }
            }
        }

        /****************************************************************
         *                         Arrays Node
         ***************************************************************/

        public void ArrayDeclaration()
        {
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expression();
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Expression();
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
        }

        /****************************************************************
         *                          Type Node
         ***************************************************************/

        public void Type()
        {
            switch (CurrentToken)
            {
                case TokenCategory.INTEGER:
                    Expect(TokenCategory.INTEGER);
                    break;

                case TokenCategory.REAL:
                    Expect(TokenCategory.REAL);
                    break;

                case TokenCategory.LOGICAL:
                    Expect(TokenCategory.LOGICAL);
                    break;

                case TokenCategory.CHARACTER:
                    Expect(TokenCategory.CHARACTER);
                    break;

                default:
                    throw new SyntaxError(firstOfStatement,
                                        tokenStream.Current);
            }
        }

        /****************************************************************
         *                        Parameter Node
         ***************************************************************/

        public void Parameter()
        {
            Expect(TokenCategory.PARAMETER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Assignment();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
        }

        /****************************************************************
         *                           Data Node
         ***************************************************************/

        public void Data()
        {
            Expect(TokenCategory.DATA);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.DIV);
            ExpectLiteral();

            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                ExpectLiteral();
            }
            Expect(TokenCategory.DIV);
        }

        /****************************************************************
         *                         Common Node
         ***************************************************************/

        public void Common()
        {
            Expect(TokenCategory.COMMON);
            Expect(TokenCategory.DIV);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.DIV);

            if (CurrentToken == TokenCategory.IDENTIFIER)
                Expect(TokenCategory.IDENTIFIER);
            
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Expect(TokenCategory.IDENTIFIER);
            }
        }

        /****************************************************************
         *                        Statement Node
         ***************************************************************/

        public Node Statement()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    Assignment();
                    break;
                
                case TokenCategory.IF:
                    IfCondition();
                    break;
                
                case TokenCategory.DO:
                    return DoLoop();

                case TokenCategory.READ:
                    Read();
                    break;

                case TokenCategory.WRITE:
                    Write();
                    break;

                case TokenCategory.GOTO:
                    Goto();
                    break;
                
                case TokenCategory.CONTINUE:
                    Continue();
                    break;
                
                case TokenCategory.CALL:
                    Expression();
                    break;

                default:
                    throw new SyntaxError(firstOfStatement, 
                                        tokenStream.Current);
            }
        }

        /****************************************************************
         *                      If Condition Node
         ***************************************************************/
        
        public Node IfCondition()
        {
            var ifToken = Expect(TokenCategory.IF);
            var expr1 = Expression();
            CheckForThen();
            var stmtList1 = EvaluateStatements();

            var result = new If() { expr1, stmtList };
            result.AnchorToken = ifToken;
            
            while (CurrentToken == TokenCategory.ELSEIF)
            {
                var elseIfToken = Expect(TokenCategory.ELSEIF);
                var expr2 = Expression();
                CheckForThen();
                var stmtList2 = EvaluateStatements();

                var elseIfResult = new ElseIf() { expr2, stmtList2 };
                elseIfResult.AnchorToken = elseIfToken;
                result.Add(elseIfResult);
            }
            
            if (CurrentToken == TokenCategory.ELSE)
            {
                var elseToken = Expect(TokenCategory.ELSE);
                var stmtList3 = EvaluateStatements();
                
                var elseResult = new Else() {
                    AnchorToken = elseToken
                };
                elseResult.Add(stmtList3);
                result.Add(elseResult);
            }
            
            if (CurrentToken == TokenCategory.ENDIF)
                Expect(TokenCategory.ENDIF);
            
            return result;
        }

        /****************************************************************
         *                         Do Loop Node
         ***************************************************************/
        
        // Assignment is missing!
        public Node DoLoop()
        {
            var doToken = Expect(TokenCategory.DO);
            var loopLabel = Expect(TokenCategory.INT_LITERAL);
            Assignment();
            
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Expression();
            }

            EvaluateStatements();
        }

        /****************************************************************
         *                        Assignment Node
         ***************************************************************/

        // Array Management is missing!
        public void Assignment()
        {
            var idToken = Expect(TokenCategory.IDENTIFIER);
            if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                ArrayDeclaration();

            var assgnToken = Expect(TokenCategory.ASSIGN);
            var expr = Expression();
            return null;
        }

        /****************************************************************
         *                         Write Node
         ***************************************************************/

        public void Write()
        {
            Expect(TokenCategory.WRITE);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expect(TokenCategory.MUL);
            Expect(TokenCategory.COMMA);
            Expect(TokenCategory.MUL);
            Expect(TokenCategory.PARENTHESIS_CLOSE);

            if (firstOfSimpleExpression.Contains(CurrentToken))
                Expression();
            
            while (CurrentToken == TokenCategory.COMMA)
            {
                Expect(TokenCategory.COMMA);
                Expression();
            }
        }

        /****************************************************************
         *                          Read Node
         ***************************************************************/

        public void Read()
        {
            Expect(TokenCategory.READ);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expect(TokenCategory.MUL);
            Expect(TokenCategory.COMMA);
            Expect(TokenCategory.MUL);
            Expect(TokenCategory.PARENTHESIS_CLOSE);

            Expect(TokenCategory.IDENTIFIER);
            while (firstOfMultipleDeclarations.Contains(CurrentToken))
            {
                if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                    ArrayDeclaration();
                else if (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    Expect(TokenCategory.IDENTIFIER);
                }
            }
        }

        /****************************************************************
         *                         Goto Node
         ***************************************************************/

        public void Goto()
        {
            Expect(TokenCategory.GOTO);
            Expect(TokenCategory.INT_LITERAL);
        }

        /****************************************************************
         *                         Continue Node
         ***************************************************************/
        
        public void Continue()
        {
            Expect(TokenCategory.CONTINUE);
        }

        /****************************************************************
         *                         Function Node
         ***************************************************************/
        
        public void Function()
        {
            Type();
            Expect(TokenCategory.FUNCTION);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            
            if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
            {
                Expect(TokenCategory.IDENTIFIER);
                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    Expect(TokenCategory.IDENTIFIER);
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            
            EvaluateDeclarations();
            EvaluateStatements();
            
            Expect(TokenCategory.RETURN);
            Expect(TokenCategory.END);
        }

        /****************************************************************
         *                       Subroutine Node
         ***************************************************************/
        
        public void Subroutine()
        {
            Expect(TokenCategory.SUBROUTINE);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);

            if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
            {
                Expect(TokenCategory.IDENTIFIER);
                while (CurrentToken == TokenCategory.COMMA)
                {
                    Expect(TokenCategory.COMMA);
                    Expect(TokenCategory.IDENTIFIER);
                }
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            
            EvaluateDeclarations();
            EvaluateStatements();
            
            Expect(TokenCategory.RETURN);
            Expect(TokenCategory.END);
        }

        /*****************************************************************
         *                      Expression Methods
         ****************************************************************/

         /****************************************************************
         *                        Expression Node
         ***************************************************************/

        public Node Expression() { return OrExpression(); }

        /****************************************************************
         *                        Or Expression
         ***************************************************************/

        public Node OrExpression()
        {
            var expr1 = AndExpression();
            if (CurrentToken == TokenCategory.OR)
            {
                var expr2 = new Or() {
                    AnchorToken = Expect(TokenCategory.OR)
                };
                expr2.Add(expr1);
                expr2.Add(AndExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                       And Expression
         ***************************************************************/

        public Node AndExpression()
        {
            var expr1 = EqualityExpression();
            if (CurrentToken == TokenCategory.AND)
            {
                var expr2 = new And() {
                    AnchorToken = Expect(TokenCategory.AND)
                };
                expr2.Add(expr1);
                expr2.Add(EqualityExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                      Equality Expression
         ***************************************************************/

        public Node EqualityExpression()
        {
            var expr1 = CompExpression();
            if (equalitySymbols.Contains(CurrentToken))
            {
                var expr2 = EqualityOperator();
                expr2.Add(expr1);
                expr2.Add(CompExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                    Comparison Expression
         ***************************************************************/

        public Node CompExpression()
        {
            var expr1 = AddExpression();
            if (comparingSymbols.Contains(CurrentToken))
            {
                var expr2 = ComparingOperator();
                expr2.Add(expr1);
                expr2.Add(AddExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                     Addition Expression
         ***************************************************************/

        public Node AddExpression()
        {
            var expr1 = MultExpression();
            while (additionSymbols.Contains(CurrentToken))
            {
                var expr2 = AdditionOperator();
                expr2.Add(expr1);
                expr2.Add(MultExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                  Multiplication Expression
         ***************************************************************/

        public Node MultExpression()
        {
            var expr1 = PowExpression();
            while (multiplicationSymbols.Contains(CurrentToken))
            {
                var expr2 = MultiplicationOperator();
                expr2.Add(expr1);
                expr2.Add(PowExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                       Power Expression
         ***************************************************************/

        public Node PowExpression()
        {
            var expr1 = NegExpression();
            if (CurrentToken == TokenCategory.EXPONENT)
            {
                var expr2 = new Power() {
                    AnchorToken = Expect(TokenCategory.EXPONENT)
                };
                expr2.Add(expr1);
                expr2.Add(PowExpression());
                expr1 = expr2;
            }
            return expr1;
        }

        /****************************************************************
         *                     Negation Expression
         ***************************************************************/

        public Node NegExpression()
        {
            var expr = SimpleExpression();
            if (negationSymbols.Contains(CurrentToken))
            {
                var negate = NegationOperator();
                negate.Add(expr);
                expr = negate;
            }
            return expr;
        }

        /****************************************************************
         *                      Simple Expression
         ***************************************************************/

        public Node SimpleExpression()
        {
            switch (CurrentToken)
            {
                case TokenCategory.CALL:
                    var callSub = new Call();
                    callSub.AnchorToken = Expect(TokenCategory.CALL);
                    callSub.Add(IdentifierFound());
                    return callSub;

                case TokenCategory.IDENTIFIER:
                    return IdentifierFound();
                
                case TokenCategory.INT_LITERAL:
                    return new IntLiteral() {
                        AnchorToken = Expect(TokenCategory.INT_LITERAL)
                    };
                
                case TokenCategory.LOGIC_LITERAL:
                    return new LogicLiteral() {
                        AnchorToken = Expect(TokenCategory.LOGIC_LITERAL)
                    };
                
                case TokenCategory.REAL:
                    var realFunc = new Real();
                    realFunc.AnchorToken = Expect(TokenCategory.REAL);
                    realFunc.Add(Expression());
                    return realFunc;
                
                case TokenCategory.REAL_LITERAL:
                    return new RealLiteral() {
                        AnchorToken = Expect(TokenCategory.REAL_LITERAL)
                    };

                case TokenCategory.STRING_LITERAL:
                    return new StringLiteral() {
                        AnchorToken = Expect(TokenCategory.STRING_LITERAL)
                    };

                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    var result = Expression();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    return result;

                default:
                    throw new SyntaxError(firstOfSimpleExpression,
                                          tokenStream.Current);
            }
        }

        /*****************************************************************
         *                  Operator Auxiliary Methods
         ****************************************************************/

        public Node EqualityOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.EQUAL:
                    return new Equal() {
                        AnchorToken = Expect(TokenCategory.EQUAL)
                    };
                
                case TokenCategory.NOT_EQUAL:
                    return new NotEqual() {
                        AnchorToken = Expect(TokenCategory.NOT_EQUAL)
                    };
                
                default:
                    throw new SyntaxError(equalitySymbols,
                                          tokenStream.Current);
            }
        }

        public Node ComparingOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.GREATER_OR_EQUAL:
                    return new GreaterOrEqual() {
                        AnchorToken = Expect(TokenCategory.GREATER_OR_EQUAL)
                    };
                
                case TokenCategory.GREATER_THAN:
                    return new GreaterThan() {
                        AnchorToken = Expect(TokenCategory.GREATER_THAN)
                    };
                
                case TokenCategory.LESS_OR_EQUAL:
                    return new LessOrEqual() {
                        AnchorToken = Expect(TokenCategory.LESS_OR_EQUAL)
                    };
                
                case TokenCategory.LESS_THAN:
                    return new LessThan() {
                        AnchorToken = Expect(TokenCategory.LESS_THAN)
                    };
                
                default:
                    throw new SyntaxError(comparingSymbols,
                                          tokenStream.Current);
            }
        }

        public Node AdditionOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.ADD:
                    return new Addition() {
                        AnchorToken = Expect(TokenCategory.ADD)
                    };
                
                case TokenCategory.NEG:
                    return new Substraction() {
                        AnchorToken = Expect(TokenCategory.NEG)
                    };
                
                default:
                    throw new SyntaxError(additionSymbols,
                                          tokenStream.Current);
            }
        }

        public Node MultiplicationOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.MUL:
                    return new Multiplication() {
                        AnchorToken = Expect(TokenCategory.MUL)
                    };
                
                case TokenCategory.DIV:
                    return new Division() {
                        AnchorToken = Expect(TokenCategory.DIV)
                    };
                
                default:
                    throw new SyntaxError(multiplicationSymbols,
                                          tokenStream.Current);
            }
        }

        public Node NegationOperator()
        {
            switch (CurrentToken)
            {
                case TokenCategory.NOT:
                    return new Not() {
                        AnchorToken = Expect(TokenCategory.NOT)
                    };

                case TokenCategory.NEG:
                    return new Negation() {
                        AnchorToken = Expect(TokenCategory.NEG)
                    };
                
                default:
                    throw new SyntaxError(negationSymbols,
                                          tokenStream.Current);
            }
        }

        /****************************************************************
         *                 Identifier Auxiliary Method
         ***************************************************************/

        private Node IdentifierFound()
        {
            var identifier = new Identifier() {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            };

            if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
            {
                identifier.Add(new ParenthesisOpen() {
                    AnchorToken = Expect(TokenCategory.PARENTHESIS_OPEN)
                });

                if (CurrentToken != TokenCategory.PARENTHESIS_CLOSE)
                {
                    var expr = Expression();
                    identifier.Add(expr);

                    while (CurrentToken == TokenCategory.COMMA)
                    {
                        Expect(TokenCategory.COMMA);
                        expr = Expression();
                        identifier.Add(expr);
                    }
                }
                
                identifier.Add(new ParenthesisClose() {
                    AnchorToken = Expect(TokenCategory.PARENTHESIS_CLOSE)
                });
            }
            return identifier;
        }

        /****************************************************************
         *                   Optional Checks Methods
         ***************************************************************/
        
        private void CheckForLabel()
        {
            if (tokenStream.Current.Category == TokenCategory.INT_LITERAL
                && tokenStream.Current.Column < 6)
                Expect(TokenCategory.INT_LITERAL);
        }
        
        private void CheckForLineContinuation()
        {
            var nextTokCol = tokenStream.Current.Column;
            var nextTokCat = tokenStream.Current.Category;
            
            if (nextTokCat == TokenCategory.ADD
                && nextTokCol == 6)
            {
                Expect(TokenCategory.ADD);
            }
            
            else if (nextTokCat == TokenCategory.AMPERSAND
                    && nextTokCol == 6)
            {
                Expect(TokenCategory.AMPERSAND);
            }
        }
        
        private void CheckForThen()
        {
            if (CurrentToken == TokenCategory.THEN)
                Expect(TokenCategory.THEN);
        }

        /*****************************************************************
         *                          END
         ****************************************************************/
    }
}
