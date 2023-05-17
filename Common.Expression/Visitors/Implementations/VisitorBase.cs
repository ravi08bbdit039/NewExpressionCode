using Common.Expression.Abstractions;
using Common.Expression.Helpers;
using Common.Expression.Implementations;
using Common.Expression.Visitors.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MsExpressions = System.Linq.Expressions;

namespace Common.Expression.Visitors.Implementations
{

    public abstract class VisitorBase : IVisitor
    {
        private static readonly MethodInfo ChangeTypeMethodInfo = typeof(Convert).GetMethod(nameof(Convert.ChangeType), new Type[] { typeof(object), typeof(Type) });
        private static readonly MethodInfo ConvertToNumericMethodInfo = typeof(ObjectHelper).GetMethod(nameof(ObjectHelper.TryConvertToNumeric), new Type[] { typeof(object) });

        private static readonly MethodInfo CustomAbsMethodInfo = typeof(ObjectHelper).GetMethod(nameof(ObjectHelper.Abs), new Type[] { typeof(object) });

        private static readonly Dictionary<Type, MethodInfo> AbsMethodInfos;

        private static readonly MethodInfo StringIndexInfoMethodInfo = typeof(string).GetMethod(nameof(string.IndexOf), new Type[] { typeof(string), typeof(StringComparison) });

        public MsExpressions.Expression Current;

        public abstract MsExpressions.ParameterExpression Parameter { get; }

        static VisitorBase()
        {
            AbsMethodInfos = new Dictionary<Type, MethodInfo>(TypeUtility.NumericTypes.Count);
            foreach (Type numericType in TypeUtility.NumericTypes)
            {
                AbsMethodInfos[numericType] = typeof(Math).GetMethod(nameof(Math.Abs), new Type[] { numericType });
            }
        }

        public void Visit(MemberExpression expression)
        {
            Current = VisitMemberExpression(expression);
        }

        public void Visit(ConstExpression expression)
        {
            Current = MsExpressions.Expression.Constant(expression.Value);
        }

        public void Visit(AreEqualExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Equal(leftExpression, rightExpression);
        }
        public void Visit(AreNotEqualExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.NotEqual(leftExpression, rightExpression);
        }

        public void Visit(LessThanExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.LessThan(leftExpression, rightExpression);
        }

        public void Visit(LessThanOrEqualExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.LessThanOrEqual(leftExpression, rightExpression);
        }

        public void Visit(GreaterThanExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.GreaterThan(leftExpression, rightExpression);
        }

        public void Visit(GreaterThanOrEqualExpression expression)
        {
            PrepareComparisonOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.GreaterThanOrEqual(leftExpression, rightExpression);
        }

        public void Visit(OrExpression expression)
        {
            MsExpressions.Expression current = null;
            foreach (var operand in expression.Operands)
            {
                operand.Accept(this);
                current = current == null ? Current : MsExpressions.Expression.Or(current, Current);
            }
            Current = current;
        }


        public void Visit(AndExpression expression)
        {
            MsExpressions.Expression current = null;
            foreach (var operand in expression.Operands)
            {
                operand.Accept(this);
                current = current == null ? Current : MsExpressions.Expression.And(current, Current);
            }
            Current = current;
        }

        public void Visit(NotExpression expression)
        {
            expression.Operand.Accept(this);
            Current = MsExpressions.Expression.Not(Current);
        }

        public void Visit(AbsExpression expression)
        {
            expression.Operand.Accept(this);
            var result = Current;
            Type resultType = Current.Type;
            if (TypeUtility.IsNumericNullableType(resultType) || TypeUtility.IsNumericType(resultType))
            {
                if (TypeUtility.IsNumericNullableType(resultType))
                {
                    resultType = resultType.GenericTypeArguments.First();
                    result = MsExpressions.Expression.Convert(result, resultType);
                }
                result = MsExpressions.Expression.Call(AbsMethodInfos[resultType], result);
            }
            else
            {
                result = MsExpressions.Expression.Convert(result, typeof(object));
                result = MsExpressions.Expression.Call(ConvertToNumericMethodInfo, result);
                result = MsExpressions.Expression.Call(CustomAbsMethodInfo, result);
            }
            Current = result;
        }

        public void Visit(SubtractExpression expression)
        {
            PrepareArithmeticOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Subtract(leftExpression, rightExpression);
        }

        public void Visit(AddExpression expression)
        {
            PrepareArithmeticOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Add(leftExpression, rightExpression);
        }

        public void Visit(DivideExpression expression)
        {
            PrepareArithmeticOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Divide(leftExpression, rightExpression);
        }

        public void Visit(MultiplyExpression expression)
        {
            PrepareArithmeticOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Add(leftExpression, rightExpression);
        }

        public void Visit(ModuloExpression expression)
        {
            PrepareArithmeticOperation(expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression);
            Current = MsExpressions.Expression.Modulo(leftExpression, rightExpression);
        }

        public void Visit(PositiveSignExpression expression)
        {
            expression.Operand.Accept(this);
        }

        public void Visit(NegativeSignExpression expression)
        {
            if (expression.Operand is IConstantExpression constantExpression)
            {
                var fixedExpression = Expression.Const(ExpressionType.Numeric, ObjectHelper.ChangeSign(constantExpression.Value));
                fixedExpression.Accept(this);
            }
            else
            {
                expression.Operand.Accept(this);
                Current = MsExpressions.Expression.Negate(Current);
            }
        }

        public void Visit(LikeExpression expression)
        {
            expression.LeftOperand.Accept(this);
            MsExpressions.Expression leftExpression = Current;
            expression.RightOperand.Accept(this);
            MsExpressions.Expression rightExpression = Current;

            MsExpressions.Expression conditionExpression = MsExpressions.Expression.Equal(rightExpression, NullConstant);
            if (!leftExpression.Type.IsValueType)
            {
                conditionExpression = MsExpressions.Expression.Or
                (
                    MsExpressions.Expression.Equal(leftExpression, NullConstant),
                    conditionExpression
                );
            }
            if (!ReferenceEquals(leftExpression.Type, typeof(string)))
            {
                leftExpression = MsExpressions.Expression.Convert(leftExpression, typeof(object));
                leftExpression = MsExpressions.Expression.Call(ChangeTypeMethodInfo, leftExpression, leftExpression = MsExpressions.Expression.Constant(typeof(string)));
                leftExpression = MsExpressions.Expression.Convert(leftExpression, typeof(string));
            }

            var mainExpression = MsExpressions.Expression.NotEqual
            (
                MsExpressions.Expression.Call(leftExpression, StringIndexInfoMethodInfo, rightExpression, StringComparisonMode),
                IndexOfNotFound
            );

            Current = MsExpressions.Expression.Condition(conditionExpression, FalseConstant, mainExpression);
        }
        public void Reset()
        {
            Current = null;
        }

        protected internal abstract MsExpressions.Expression VisitMemberExpression(MemberExpression expression);

        private void PrepareArithmeticOperation(BinaryExpression expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression)
        {
            expression.LeftOperand.Accept(this);
            leftExpression = Current;

            expression.RightOperand.Accept(this);
            rightExpression = Current;

            var commonType = TypeUtility.FindNumericCommonType(leftExpression.Type, rightExpression.Type);
            var targetType = commonType;

            if (TypeUtility.IsNumericNullableType(leftExpression.Type) || TypeUtility.IsNumericNullableType(rightExpression.Type))
            {
                targetType = typeof(Nullable<>).MakeGenericType(commonType);
            }

            leftExpression = AddConversionIfNeeded(leftExpression, targetType, commonType);
            rightExpression = AddConversionIfNeeded(rightExpression, targetType, commonType);
        }

        private void PrepareComparisonOperation(BinaryExpression expression, out MsExpressions.Expression leftExpression, out MsExpressions.Expression rightExpression)
        {
            expression.LeftOperand.Accept(this);
            leftExpression = Current;

            expression.RightOperand.Accept(this);
            rightExpression = Current;

            var commonType = leftExpression.Type;
            if (ReferenceEquals(commonType, typeof(object)))
            {
                commonType = typeof(string);
            }

            commonType = TypeUtility.IsNonNumericType(commonType) ? commonType : TypeUtility.FindNumericCommonType(commonType, rightExpression.Type);
            var targetType = commonType;
            if (!TypeUtility.IsNonNumericType(commonType) && (TypeUtility.IsNumericNullableType(leftExpression.Type) || TypeUtility.IsNumericNullableType(rightExpression.Type)))
            {
                targetType = typeof(Nullable<>).MakeGenericType(commonType);
            }

            leftExpression = AddConversionIfNeeded(leftExpression, targetType, commonType);
            rightExpression = AddConversionIfNeeded(rightExpression, targetType, commonType);
        }

        private MsExpressions.Expression AddConversionIfNeeded(MsExpressions.Expression expression, Type targetType, Type mainType)
        {
            if (ReferenceEquals(expression.Type, typeof(string)))
            {
                return expression;
            }
            var result = expression;
            if (ReferenceEquals(expression.Type, typeof(string)))
            {
                result = MsExpressions.Expression.Call(ChangeTypeMethodInfo, result, MsExpressions.Expression.Constant(mainType));
                result = MsExpressions.Expression.Convert(result, mainType);
                if (!ReferenceEquals(targetType, mainType))
                {
                    result = MsExpressions.Expression.Convert(result, targetType);
                }
                return result;
            }
            if (ReferenceEquals(targetType, typeof(string)))
            {
                result = MsExpressions.Expression.Convert(result, typeof(object));
                result = MsExpressions.Expression.Call(ChangeTypeMethodInfo, result, MsExpressions.Expression.Constant(targetType));
                result = MsExpressions.Expression.Convert(result, targetType);
                return result;
            }
            if (!TypeUtility.IsNumericNullableType(expression.Type) && TypeUtility.IsNumericNullableType(targetType) && !ReferenceEquals(expression.Type, mainType))
            {
                result = MsExpressions.Expression.Convert(result, mainType);
            }
            result = MsExpressions.Expression.Convert(result, targetType);
            return result;
        }

        private static readonly MsExpressions.Expression IndexOfNotFound = MsExpressions.Expression.Constant(-1);
        private static readonly MsExpressions.Expression StringComparisonMode = MsExpressions.Expression.Constant(StringComparison.InvariantCultureIgnoreCase);
        private static readonly MsExpressions.Expression NullConstant = MsExpressions.Expression.Constant(null);
        private static readonly MsExpressions.Expression FalseConstant = MsExpressions.Expression.Constant(false);

    }
}
