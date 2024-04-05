using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public enum EType
    {
        [Description("Stored Procedure")]
        StoredProcedure = 1,

        [Description("Scalar Function")]
        ScalarFunction,

        [Description("Inline Table-Valued Function")]
        InlineTableFunction,

        [Description("Multi-Statement Table-Valued Function")]
        MultiStatementTableFunction,

        [Description("View")]
        View


    }
}
