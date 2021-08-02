using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ILHelper
{
    public abstract class BuilderBase
    {
        protected SemaphoreSlim SyncronizationLock = new(1, 1);

        [DebuggerHidden]
        protected virtual T InvokeCritical<T>(Func<T> Expression)
        {
            SyncronizationLock?.Wait();
            try
            {
                return Expression.Invoke();
            }
            finally
            {
                SyncronizationLock?.Release();
            }
        }

        [DebuggerHidden]
        protected virtual void InvokeCritical(Action action)
        {
            SyncronizationLock?.Wait();
            try
            {
                action.Invoke();
            }
            finally
            {
                SyncronizationLock?.Release();
            }
        }
    }
}
