using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquealerConsoleCSharp.Models
{
    public enum EGitCommand
    {
        [GitCommand("rev-parse --show-toplevel", false)]
        GetProject = 1,

        [GitCommand("rev-parse --abbrev-ref HEAD", false)]
        GetBranch,

        [GitCommand("status --porcelain", true)]
        GetUnCommitedFiles
    }
}
