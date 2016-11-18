using System;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    public class Program: Node {}

    public class DeclarationList: Node {}

    public class Declaration: Node {}

    public class FArray: Node {}

    public class Parameter: Node {}

    public class Data: Node {}

    public class DataList: Node {}

    public class Common: Node {}

    public class CommonList: Node {}

    public class StatementList: Node {}

    public class ArgumentList: Node {}

    public class Stop: Node {}

    public class Return: Node {}

    public class End: Node {}

    public class Assignment: Node {}

    public class ParenthesisOpen: Node {}

    public class ParenthesisClose: Node {}

    public class If: Node {}

    public class ElseIf: Node {}

    public class Else: Node {}

    public class DoLoop: Node {}

    public class Continue: Node {}

    public class Read: Node {}

    public class Write: Node
    {
        public List<Type> ExpressionTypes { get; set; }
        public Write()
        {
            ExpressionTypes = new List<Type>();
        }
    }

    public class GoTo: Node {}

    /* *******************************************************************
     *                        Skip to Expressions.
     ********************************************************************/

    public class Identifier: Node {}

    public class IntLiteral: Node {}

    public class LogicLiteral: Node {}

    public class RealLiteral: Node {}

    public class StringLiteral: Node {}

    public class Call: Node {}

    public class Or: Node {}

    public class And: Node {}

    public class Equal: Node {}

    public class NotEqual: Node {}

    public class GreaterOrEqual: Node {}

    public class GreaterThan: Node {}

    public class LessOrEqual: Node {}

    public class LessThan: Node {}

    public class Addition: Node {}

    public class Substraction: Node {}

    public class Multiplication: Node {}

    public class Division: Node {}

    public class Power: Node {}

    public class Negation: Node {}

    public class Not: Node {}

    public class Real: Node {}

    public class Label: Node {}

    public class Function: Node {}

    public class FunctionType: Node {}

    public class Subroutine: Node {}
}
