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
        [ObjectType("StoredProcedure", "p", "")]
        StoredProcedure = 1,

        [ObjectType("ScalarFunction", "fn", "()")]
        ScalarFunction,

        [ObjectType("Inline Table-Valued Function", "fn", "()")]
        InlineTableFunction,

        [ObjectType("Multi-Statement Table-Valued Function", "fn", "()")]
        MultiStatementTableFunction,

        [ObjectType("View", "v", "*")]
        View

    }
}
