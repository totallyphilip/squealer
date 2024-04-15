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
        [ObjectType("StoredProcedure", "p", "", "p", "Stored Procedure", "execute")]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "fn", "()", "fn", "Scalar Function", "execute")]
        ScalarFunction,

        [ObjectType("InlineTableFunction", "if", "()*", "_if", "Inline Table-Valued Function", "select")]
        InlineTableFunction,

        [ObjectType("MultiStatementTableFunction", "tf", "</>", "tf", "Multi-Statement Table-Valued Function", "select")]
        MultiStatementTableFunction,

        [ObjectType("View", "v", "*", "v", "View", "select")]
        View

    }
}
