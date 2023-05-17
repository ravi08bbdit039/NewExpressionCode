using Common.Expression.Implementations;
using Newtonsoft.Json;
using System;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace Common.Expression.Serialization.Converters
{
    public class AreEqualExpressionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            var areEqualExpression = value as AreEqualExpression;
            if (areEqualExpression != null)
            {
                writer.WriteValue(value.ToString());
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? object_, Newtonsoft.Json.JsonSerializer serializer)
        {
            string s = (string)reader.Value;
            return new Version(s);
        }

        public override bool CanRead => true;

        public override bool CanConvert(Type objectType) => ReferenceEquals(objectType, typeof(AreEqualExpression));

    }
}
