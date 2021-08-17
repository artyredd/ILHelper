using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class MethodLinter
    {
        public static readonly RuleDictionary<OpCode, StackState> OpRules = new();

        static MethodLinter()
        {
            InitializeOpRules();
        }

        //public bool TryLint(IMethod method, out LintResult result)
        //{
        //    result = new();

        //    // every method gets its own stack frame
        //    StackState stack = new();

        //    foreach (IOpInstruction item in method.Instructions)
        //    {
        //        // each instruction 

        //    }
        //}



        private static void InitializeOpRules()
        {

        }
    }
}
