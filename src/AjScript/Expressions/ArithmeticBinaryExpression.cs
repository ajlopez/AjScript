﻿namespace AjScript.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using AjScript.Language;

    using Microsoft.VisualBasic.CompilerServices;

    public class ArithmeticBinaryExpression : BinaryExpression
    {
        private Func<object, object, object> function;
        private ArithmeticOperator operation;

        public ArithmeticBinaryExpression(ArithmeticOperator operation, IExpression left, IExpression right)
            : base(left, right)
        {
            this.operation = operation;

            switch (operation)
            {
                case ArithmeticOperator.Add:
                    this.function = AddOrConcatenateObjects;
                    break;
                case ArithmeticOperator.Subtract:
                    this.function = Operators.SubtractObject;
                    break;
                case ArithmeticOperator.Multiply:
                    this.function = Operators.MultiplyObject;
                    break;
                case ArithmeticOperator.Divide:
                    this.function = Operators.DivideObject;
                    break;
                case ArithmeticOperator.IntegerDivide:
                    this.function = Operators.IntDivideObject;
                    break;
                case ArithmeticOperator.Modulo:
                    this.function = Operators.ModObject;
                    break;
                default:
                    throw new ArgumentException("Invalid operator");
            }
        }

        public ArithmeticOperator Operation { get { return this.operation; } }

        public override object Apply(object leftValue, object rightValue)
        {
            if (this.operation == ArithmeticOperator.Add)
            {
                if (leftValue == null)
                    if (ObjectUtilities.IsNumber(rightValue))
                        leftValue = 0;

                if (rightValue == null)
                    if (ObjectUtilities.IsNumber(leftValue))
                        rightValue = 0;
            }
            else
            {
                if (leftValue == null)
                    leftValue = 0;
                if (rightValue == null)
                    rightValue = 0;
            }

            var result = this.function(leftValue, rightValue);

            if (this.operation != ArithmeticOperator.Divide || !(result is double))
                return result;

            var r = (double)result;
            var floor = Math.Floor(r);

            if (floor == r)
                return (int)floor;

            return result;
        }

        private static object AddOrConcatenateObjects(object left, object right)
        {
            if (ObjectUtilities.IsNumber(left) && ObjectUtilities.IsNumber(right))
                return Operators.AddObject(left, right);

            return Operators.ConcatenateObject(StringUtilities.AsString(left), StringUtilities.AsString(right));
        }
    }
}
