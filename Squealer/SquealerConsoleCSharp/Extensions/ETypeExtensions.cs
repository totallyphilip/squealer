using Microsoft.VisualBasic.FileIO;
using SquealerConsoleCSharp.Attributes;
using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Extensions
{

    public static class ETypeExtensions
    {
        public static EType GetFileTypeByName(this Type eType, string name)
        {
            if (!eType.IsEnum || eType != typeof(EType))
            {
                throw new ArgumentException("This method is only valid for FileType enum.", nameof(eType));
            }

            foreach (var value in Enum.GetValues(typeof(EType)))
            {
                FieldInfo field = typeof(EType).GetField(value.ToString());
                ObjectTypeAttribute attribute = field.GetCustomAttribute<ObjectTypeAttribute>();

                if (attribute != null && attribute.Name == name)
                {
                    return (EType)value;
                }
            }

            throw new InvalidOperationException();
        }

        public static ObjectTypeAttribute GetObjectTypeAttribute(this EType eType)
        {
            // Get the type of the enum
            Type type = eType.GetType();

            // Get the FieldInfo for the specific enum value
            FieldInfo fieldInfo = type.GetField(Enum.GetName(type, eType));

            // Retrieve and return the ObjectTypeAttribute for the enum value
            return fieldInfo.GetCustomAttribute<ObjectTypeAttribute>();
        }
    }
}
