using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class StackElementFactory : IStackElementFactory
    {

        public IStackElement Create(Type underlyingType, StackVerificationType verificationType)
        {
            return new StackElement()
            {
                UnderlyingType = underlyingType,
                VerificationType = verificationType
            };
        }
    }
}
