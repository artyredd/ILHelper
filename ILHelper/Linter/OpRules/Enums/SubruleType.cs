using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum SubruleType
    {
        none = 1 << 0,
        All = 0b_11111111_11111111_11111111_1111111, // 32 bit mask covering all cases, 1 bit missing since it's int not uint, max Enum count should not exceed 31 members
        LogicalAND = 1 << 1,
        LogicalOR = 1 << 2,
        LogicalXOR = 1 << 3,
        If = 1 << 4,
    }
}
