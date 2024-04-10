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
        [ObjectType("StoredProcedure", "P", "", "p", "Stored Procedure")]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "FN", "()", "fn", "Scalar Function")]
        ScalarFunction,

        [ObjectType("InlineTableFunction", "iTVF", "()", "_if", "Inline Table-Valued Function")]
        InlineTableFunction,

        [ObjectType("MultiStatementTableFunction", "mTVF", "()", "tf", "Multi-Statement Table-Valued Function")]
        MultiStatementTableFunction,

        [ObjectType("View", "V", "*", "v", "View")]
        View

    }
}
