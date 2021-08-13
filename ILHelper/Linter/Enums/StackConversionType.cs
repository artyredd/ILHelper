using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum StackConversionType
    {
        none,
        Truncate,
        TruncateToZero,
        Nop,
        SignExtend,
        ZeroExtend,
        StopGCTracking,
        ToFloat,
        ChangePrecision
    }
}
