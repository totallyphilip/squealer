using System;


namespace SquealerConsoleCSharp.Models
{
    public class SquealerObjectType
    {
        public enum eType
        {
            StoredProcedure,
            ScalarFunction,
            InlineTableFunction,
            MultiStatementTableFunction,
            View,
            Invalid
        }

        public enum eShortType
        {
            p, // stored procedure
            fn, // scalar function
            @if, // inline table function
            tf, // multistatement table function
            v, // view
            err
        }

        public eType LongType { get; set; }

        public eShortType ShortType => ToShortType(LongType);

        public string GeneralType => LongType switch
        {
            eType.StoredProcedure => "PROCEDURE",
            eType.ScalarFunction or eType.InlineTableFunction or eType.MultiStatementTableFunction => "FUNCTION",
            eType.View => "VIEW",
            _ => "ERROR",
        };

        public bool Selected { get; set; }

        public SquealerObjectType() : this(eType.Invalid, false) { }

        public SquealerObjectType(eType t) : this(t, false) { }

        public SquealerObjectType(eType t, bool selected)
        {
            LongType = t;
            Selected = selected;
        }

        public string FriendlyType => LongType switch
        {
            eType.StoredProcedure => "Stored Procedure",
            eType.ScalarFunction => "Scalar Function",
            eType.InlineTableFunction => "Inline Table-Valued Function",
            eType.MultiStatementTableFunction => "Multi-Statement Table-Valued Function",
            eType.View => "View",
            _ => eType.Invalid.ToString(),
        };

        public static string EvalSimpleType(eType t) => t switch
        {
            eType.StoredProcedure => "procedure",
            eType.ScalarFunction or eType.InlineTableFunction or eType.MultiStatementTableFunction => "function",
            eType.View => "view",
            _ => eType.Invalid.ToString(),
        };

        public static eShortType ToShortType(eType t) => t switch
        {
            eType.StoredProcedure => eShortType.p,
            eType.ScalarFunction => eShortType.fn,
            eType.InlineTableFunction => eShortType.@if,
            eType.MultiStatementTableFunction => eShortType.tf,
            eType.View => eShortType.v,
            _ => eShortType.err,
        };

        public void SetType(string s)
        {
            LongType = Eval(s);
        }

        public static bool Validated(string s)
        {
            try
            {
                _ = (eType)Enum.Parse(typeof(eType), s, true);
                return true;
            }
            catch
            {
                try
                {
                    _ = (eShortType)Enum.Parse(typeof(eShortType), s, true);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static eType Eval(string s)
        {
            try
            {
                return (eType)Enum.Parse(typeof(eType), s, true);
            }
            catch
            {
                var st = eShortType.err;
                try
                {
                    st = (eShortType)Enum.Parse(typeof(eShortType), s, true);
                }
                catch
                {
                    // In case of exception, st remains eShortType.err
                }

                return st switch
                {
                    eShortType.p => eType.StoredProcedure,
                    eShortType.fn => eType.ScalarFunction,
                    eShortType.@if => eType.InlineTableFunction,
                    eShortType.tf => eType.MultiStatementTableFunction,
                    eShortType.v => eType.View,
                    _ => eType.Invalid,
                };
            }
        }
    }
}
