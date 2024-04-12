using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    internal class ObjectTypeAttribute : Attribute
    {
        public string Name { get; }

        public string Acronym { get; }
        public string NumericSymbol { get; }

        public string VariableName { get; }

        public string FriendlyName { get; set; }

        public string ObjectTypeCode { get; set; }

        public string Permission {  get; set; }

        public ObjectTypeAttribute(string name, string acronym, string numericSymbol, string variableName, string friendlyName, string objectTypeCode, string permission)
        {
            Name = name;
            Acronym = acronym;
            NumericSymbol = numericSymbol;
            VariableName = variableName;
            FriendlyName = friendlyName;
            ObjectTypeCode = objectTypeCode;
            Permission = permission;
        }
    }
}
