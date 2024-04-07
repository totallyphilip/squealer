using SquealerConsoleCSharp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Extensions
{
    public static class GitCommandExtensions
    {
        public static GitCommandAttribute GetGitCommandAttribute(this EGitCommand command)
        {
            // Get the type of the enum
            Type type = command.GetType();

            // Get the member information for the specified enum value
            MemberInfo[] memberInfo = type.GetMember(command.ToString());

            if (memberInfo.Length > 0)
            {
                // Get the GitCommandAttribute applied to this enum value, if any
                var attribute = memberInfo[0].GetCustomAttributes(typeof(GitCommandAttribute), false)
                                            .FirstOrDefault() as GitCommandAttribute;

                return attribute;
            }

            return null; // Or throw an exception if appropriate
        }
    }

}
