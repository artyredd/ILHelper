using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum OperandType
    {
        BinaryNumericAdd,
        BinaryNumericSub,
        BinaryNumericDiv,
        Unary,
        /// <summary>
        /// beq, beq.s, bge, bge.s, bge.un, bge.un.s, bgt, bgt.s, bgt.un, bgt.un.s, ble, ble.s, ble.un, ble.un.s, blt, blt.s, blt.un, blt.un.s, bne.un, bne.un.s, ceq, cgt, cgt.un, clt, clt.un
        /// </summary>
        BinaryComparison,
        /// <summary>
        /// shl, shr
        /// </summary>
        Integer
    }
}
