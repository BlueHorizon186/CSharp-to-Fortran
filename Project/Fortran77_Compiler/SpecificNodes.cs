using System;

namespace Fortran77_Compiler
{
    class Program: Node {}

    class DeclarationList: Node {}

    class Declaration: Node {}

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

    class Write: Node {}

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

    class Subroutine: Node {}
}
