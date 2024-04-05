using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public enum EType
    {
        StoredProcedure,
        ScalarFunction,
        InlineTableFunction,
        MultiStatementTableFunction,
        View,
        Invalid,
    }
}
