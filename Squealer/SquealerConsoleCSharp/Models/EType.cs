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
        [ObjectType("StoredProcedure", "p", "", "p")]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "fn", "()", "fn")]
        ScalarFunction,

        [ObjectType("Inline Table-Valued Function", "fn", "()", "_if")]
        InlineTableFunction,

        [ObjectType("Multi-Statement Table-Valued Function", "fn", "()", "tf")]
        MultiStatementTableFunction,

        [ObjectType("View", "v", "*", "v")]
        View

    }
}
