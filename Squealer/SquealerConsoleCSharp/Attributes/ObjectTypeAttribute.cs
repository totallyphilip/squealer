using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ObjectTypeAttribute : Attribute
    {
        public string Name { get; }

        public string ShortName { get; }
        public string NumericSymbol { get; }

        public ObjectTypeAttribute(string name, string shortname, string symbol)
        {
            Name = name;
            ShortName = shortname;
            NumericSymbol = symbol;
        }
    }
}
