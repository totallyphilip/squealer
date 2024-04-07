using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class GitCommandAttribute : Attribute
{
    public string Command { get; }
    public bool MultiLineOutput { get;}

    public GitCommandAttribute(string command, bool multiLineOutput = false)
    {
        Command = command;
        MultiLineOutput = multiLineOutput;
    }
}