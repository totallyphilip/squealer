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
        [ObjectType("StoredProcedure", "P", "", "p", "Stored Procedure", "p", "execute")]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "FN", "()", "fn", "Scalar Function", "fn", "execute")]
        ScalarFunction,

        [ObjectType("InlineTableFunction", "iTVF", "()", "_if", "Inline Table-Valued Function", "fn", "select")]
        InlineTableFunction,

        [ObjectType("MultiStatementTableFunction", "mTVF", "()", "tf", "Multi-Statement Table-Valued Function", "fn", "select")]
        MultiStatementTableFunction,

        [ObjectType("View", "V", "*", "v", "View", "v", "select")]
        View

    }
}
