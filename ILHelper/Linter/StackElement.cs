using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class StackElement : IStackElement
    {
        public StackElementType ElementType { get; set; } = StackElementType.None;

        public object Value { get; set; }

        public bool HasValue => Value is not null;
    }
}
