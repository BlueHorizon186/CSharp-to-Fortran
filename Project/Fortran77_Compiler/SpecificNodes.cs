using System;
using System.Collections.Generic;

namespace Fortran77_Compiler
{
    class Program: Node {}

    class DeclarationList: Node {}

    class Declaration: Node {}

    class FArray: Node {}

    class Parameter: Node {}

    class Data: Node {}

    class DataList: Node {}

    class Common: Node {}

    class CommonList: Node {}

    class StatementList: Node {}

    class ArgumentList: Node {}

    class Stop: Node {}

    class Return: Node {}

    class End: Node {}

    class Assignment: Node {}

    class ParenthesisOpen: Node {}

    class ParenthesisClose: Node {}

    class If: Node {}

    class ElseIf: Node {}

    class Else: Node {}

    class DoLoop: Node {}

    class Continue: Node {}

    class Read: Node {}

    class Write: Node
    {
        public List<Type> ExpressionTypes { get; set; }
        public Write()
        {
            ExpressionTypes = new List<Type>();
        }
    }

    class GoTo: Node {}

    /* *******************************************************************
     *                        Skip to Expressions.
     ********************************************************************/

    class Identifier: Node {}

    class IntLiteral: Node {}

    class LogicLiteral: Node {}

    class RealLiteral: Node {}

    class StringLiteral: Node {}

    class Call: Node {}

    class Or: Node {}

    class And: Node {}

    class Equal: Node {}

    class NotEqual: Node {}

    class GreaterOrEqual: Node {}

    class GreaterThan: Node {}

    class LessOrEqual: Node {}

    class LessThan: Node {}

    class Addition: Node {}

    class Substraction: Node {}

    class Multiplication: Node {}

    class Division: Node {}

    class Power: Node {}

    class Negation: Node {}

    class Not: Node {}

    class Real: Node {}

    class Label: Node {}

    class Function: Node {}

    class FunctionType: Node {}

    class Subroutine: Node {}
}
