using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace ILHelper.Linter
{
    public class StackState
    {
        public Stack<IStackElement> Stack { get; } = new(10);

        private readonly IStackElementFactory Factory = new StackElementFactory();

    }
}
