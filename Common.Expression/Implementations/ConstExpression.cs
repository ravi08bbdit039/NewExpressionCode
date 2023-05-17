using Common.Expression.Abstractions;
using Common.Expression.Helpers;
using Common.Expression.Visitors.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Runtime.Serialization;

namespace Common.Expression.Implementations
{
    public class ConstExpression : Expression, IConstantExpression
    {
        public ConstExpression(ExpressionType type, object value)
                : base(type)
        {
            Value = ObjectHelper.ChangeType(value, type);
        }

        public ConstExpression(object value)
                : this(FindMatchingType(value), value) { }

        protected ConstExpression(SerializationInfo info, StreamingContext context)
            :base(info,context)
        {
            var wrapper = (JValue)info.GetValue(nameof(Value), typeof(object));
            Value = ObjectHelper.ChangeType(wrapper.Value, ResultType);            
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(Value), Value, typeof(object));
            base.GetObjectData(info, context);
        }

        public object Value { get; }
        
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, this))
            {
                return true;
            }

            return obj is ConstExpression other
                   && base.Equals(obj)
                   && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() * ObjectHelper.HashCodeSeed + (Value?.GetHashCode() ?? 0);
            }
        }

        public override string ToString()
        {
            return ObjectHelper.FormatValue(Value);
        }

        public static ExpressionType FindMatchingType(object value)
        {
            if (value == null)
            {
                return ExpressionType.Any;
            }

            if (value is bool)
            {
                return ExpressionType.Bool;
            }

            if (value is DateTime)
            {
                return ExpressionType.DateTime;
            }

            if (TypeUtility.IsNumericOrNumericNullableType(value.GetType()))
            {
                return ExpressionType.Numeric;
            }
            return ExpressionType.Any;
        }
    }
}
