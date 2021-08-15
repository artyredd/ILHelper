using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class LintResult
    {
        public bool Passed { get; set; }

        public int InstructionIndex { get; set; }

        public string Message { get; set; }
    }
}
