using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public interface IMethod
    {
        public Type ReturnType { get; set; }

        public IList<Type> ParameterTypes { get; set; }

        public MethodAttributes Attributes { get; set; }

        public IList<IOpInstruction> Instructions { get; set; }
    }
}
