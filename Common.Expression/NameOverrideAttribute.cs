using System;

namespace Common.Expression
{
    public class NameOverrideAttribute : Attribute
    {
        public  NameOverrideAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"Incorrect attribute's value: {value ?? "null"}");
            }
            this.Value = value;
        }

        public string Value { get; set; }
    }
}
