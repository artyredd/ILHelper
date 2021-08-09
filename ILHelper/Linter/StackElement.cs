using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class StackElement : IStackElement
    {
        public StackVerificationType VerificationType { get; set; } = StackVerificationType.Object;

        public Type UnderlyingType { get; set; }

        public bool HasType => UnderlyingType is not null;
    }
}
