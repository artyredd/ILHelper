using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum StackElementType
    {
        None,
        Pointer,
        Address,
        Value,
        BoxedValue,
        BoxedReference,
        BoxedAny,
        Label
    }
}
