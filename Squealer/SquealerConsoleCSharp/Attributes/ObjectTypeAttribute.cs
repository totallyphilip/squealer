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

        public string Acronym { get; }
        public string NumericSymbol { get; }

        public string VariableName { get; }

        public string FriendlyName { get; set; }

        public ObjectTypeAttribute(string name, string acronym, string numericSymbol, string variableName, string friendlyName)
        {
            Name = name;
            Acronym = acronym;
            NumericSymbol = numericSymbol;
            VariableName = variableName;
            FriendlyName = friendlyName;
        }
    }
}
