using SquealerConsoleCSharp.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp
{
    public static class EnumHelper
    {
        public static T GetEnumByObjectTypeCode<T>(string objectTypeCode) where T : Enum
        {
            var type = typeof(T);
            foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attribute = field.GetCustomAttribute<ObjectTypeAttribute>();
                if (attribute != null && attribute.ObjectTypeCode == objectTypeCode)
                {
                    return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException($"No enum value found with ObjectTypeCode '{objectTypeCode}'", nameof(objectTypeCode));
        }
    }
}
