using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum StackVerificationType
    {
        None = 0,
        Int32 = 1,
        Int64 = 1 << 1,
        NativeInt = 1 << 2,
        Float64 = 1 << 3,
        Value = 1 << 4,
        Object = 1 << 5,
        Pointer = 1 << 6,
        Address = 1 << 7,
        All = 0b_11111111_11111111_11111111_1111111, // 32 bit mask covering all cases, 1 bit missing since it's int not uint, max Enum count should not exceed 31 members
    }
}
