using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public enum CallbackType
    {
        none,
        BeforeEvaluation,
        AfterEvaluation,
        OnFail,
        OnPass,
        OnThrow,
        OnSkip
    }
}
