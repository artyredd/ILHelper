using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum OperandType
    {
        /// <summary>
        /// add, div, mul, rem, or sub
        /// </summary>
        BinaryNumericAny,
        /// <summary>
        /// add
        /// </summary>
        BinaryNumericAdd,
        /// <summary>
        /// sub
        /// </summary>
        BinaryNumericSub,
        /// <summary>
        /// neg
        /// </summary>
        Unary,
        /// <summary>
        /// beq, beq.s, bge, bge.s, bge.un, bge.un.s, bgt, bgt.s, bgt.un, bgt.un.s, ble, ble.s, ble.un, ble.un.s, blt, blt.s, blt.un, blt.un.s, bne.un, bne.un.s, ceq, cgt, cgt.un, clt, clt.un
        /// </summary>
        BinaryComparison,
        /// <summary>
        /// beq[.s], bne.un[.s], ceq
        /// </summary>
        EqualityComparison,
        /// <summary>
        /// and, div.un, not, or, rem.un, xor
        /// </summary>
        Integer,
        /// <summary>
        /// shl, shr
        /// </summary>
        Shift,
        /// <summary>
        /// add.ovf, add.ovf.un, mul.ovf, mul.ovf.un, sub.ovf, sub.ovf.un
        /// </summary>
        OverflowArithmetic,
        /// <summary>
        /// add.ovf.un
        /// </summary>
        OverflowAddUnsigned,
        /// <summary>
        /// sub.ovf.un
        /// </summary>
        OverflowSubUnsigned,
        /// <summary>
        /// conv.<to type>, conv.ovf.<to type>, and conv.ovf.<to type>.un
        /// </summary>
        Convert,
    }
}
