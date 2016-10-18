using System;
using System.Collections.Generic;

namespace Fortran77_Compiler {

    class Parser {

        /*******************************************************************
         * IMPORTANT NOTICE: This is just a template based on Buttercup's
         * 2nd Stage Parser. Adding and/or removing Sets and Methods will
         * most likely be required for the Fortran77 Version.
         *******************************************************************/

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

        static readonly ISet<TokenCategory> firstOfSimpleExpression =
            new HashSet<TokenCategory>() {
                // Here will go Simple Expression keywords.
            };

        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream) {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category) {
            if (CurrentToken == category) {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            } else {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public void Program() {

            while (firstOfDeclaration.Contains(CurrentToken)) {
                Declaration();
            }

            while (firstOfStatement.Contains(CurrentToken)) {
                Statement();
            }

            Expect(TokenCategory.EOF);
        }
    
        public void Declaration() {
            Type();
            Expect(TokenCategory.IDENTIFIER);
    	
    	    while (firstOfStatement.Contains(CurrentToken)){
    		    if(firstOfStatement.Contains(CurrentToken ){
    			    while (firstOfStatement.Contains(CurrentToken)) {
                        Expect(TokenCategory.COMMA);
                        Identifier();
                    }
    	
    		    }else{
    		  
    		        Expect(TokenCategory.PARENTHESIS_OPEN);
    	            Identifier();
    	            while (firstOfStatement.Contains(CurrentToken)) {
    			 	    Expect(TokenCategory.COMMA);
    				    Identifier();
    			
    			}
    		    Expect(TokenCategory.PARENTHESIS_CLOSE);
    		}
    	
    
    	    }
            
        }

        public void Statement() {
            switch (CurrentToken) {

            case TokenCategory.IDENTIFIER:
                Assignment();
                break;

            case TokenCategory.PARAMETER:
                Parameter();
                break;

            case TokenCategory.READ:
                Read();
                break;

 	        case TokenCategory.WRITE:
                Write();
                break;
            
  	        case TokenCategory.CONDITION:
                Condition();
                break;

  	         case TokenCategory.DO:
                Do();
                break;

       	    case TokenCategory.GOTO:
                Goto();
                break;

 	        case TokenCategory.SIMPLE:
                Simple();
                break;

            default:
                throw new SyntaxError(firstOfStatement, 
                                      tokenStream.Current);
            }

        }

        public void Type() {
            switch (CurrentToken) {

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
        
        public void Function() {
	        Type();  
 	        Expect(TokenCategory.FUNCTION);
	        Identifier();
            Expect(TokenCategory.PARENTHESIS_OPEN);
           
            if(firstOfStatement.Contains(CurrentToken)){
	            Expression();
                while (firstOfStatement.Contains(CurrentToken)) {
                    Expect(TokenCategory.COMMA);
                    Expression();
                }                
            }
            
            Expect(TokenCategory.PARENTHESIS_CLOSE);
	 
		    while (firstOfStatement.Contains(CurrentToken)) {
		        Declaration(); 
	        }
	        while (firstOfStatement.Contains(CurrentToken)) {
	 	        Statement();
	        }
		    Expect(TokenCategory.RETURN)
		    Expect(TokenCategory.END);
        }
        
        public void Line(){
            
        }


        public void Assignment() {
            
        }

        public void Print() {
            
        }

        public void If() {
            
        }

        public void Expression() {
            
        }

        public void SimpleExpression() {

        }

        public void Operator() {

        }
    }
}
