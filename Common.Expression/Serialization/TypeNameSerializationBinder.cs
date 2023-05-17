using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Common.Expression.Serialization
{
    public class TypeNameSerializationBinder : SerializationBinder, ISerializationBinder
    {
        private static readonly Dictionary<string, Type> NameToType;
        private static readonly Dictionary<Type, string> TypeToName;

        static TypeNameSerializationBinder()
        {
            var rootType = typeof(Expression);
            var types = rootType.Assembly.GetExportedTypes().Where(t => rootType.IsAssignableFrom(t));
            NameToType = types.ToDictionary(t => t.Name, t => t);
            TypeToName = NameToType.ToDictionary(t => t.Value, t => t.Key);
        }


        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            if (TypeToName.TryGetValue(serializedType, out typeName))
            {
                assemblyName = null;
                return;
            }

            base.BindToName(serializedType, out assemblyName, out typeName);
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            if (NameToType.TryGetValue(typeName, out var result))
            {
                return result;
            }

            return null;
        }
    }
}
