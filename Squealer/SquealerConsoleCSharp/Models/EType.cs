using Microsoft.VisualBasic.FileIO;
using SquealerConsoleCSharp.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public enum EType
    {
        [ObjectType("StoredProcedure", "p", "", "p", "Stored Procedure", "execute", 2)]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "fn", "()", "fn", "Scalar Function", "execute", 3)]
        ScalarFunction,

        [ObjectType("InlineTableFunction", "if", "()*", "_if", "Inline Table-Valued Function", "select", 4)]
        InlineTableFunction,

        [ObjectType("MultiStatementTableFunction", "tf", "</>", "tf", "Multi-Statement Table-Valued Function", "select", 5)]
        MultiStatementTableFunction,

        [ObjectType("View", "v", "*", "v", "View", "select", 1)]
        View

    }
}
