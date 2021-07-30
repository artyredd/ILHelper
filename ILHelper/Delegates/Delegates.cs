using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Delegates
{
    public class Delegates
    {
        //public delegate TResult Func<TResult>();
        //public delegate TResult Func<T, TResult>(T arg_0);
        //public delegate TResult Func<T1, T2, TResult>(T1 arg_0, T2 arg_1);
        //public delegate TResult Func<T1, T2, T3, TResult>(T1 arg_0, T2 arg_1, T3 arg_2);
        //public delegate TResult Func<T1, T2, T3, T4, TResult>(T1 arg_0, T2 arg_1, T3 arg_2, T4 arg_3);

        public delegate ref TResult RefFunc<TResult>();
        public delegate ref TResult RefFunc<T, TResult>(T arg_0);
        public delegate ref TResult RefFunc<T1, T2, TResult>(T1 arg_0, T2 arg_1);
        public delegate ref TResult RefFunc<T1, T2, T3, TResult>(T1 arg_0, T2 arg_1, T3 arg_2);
        public delegate ref TResult RefFunc<T1, T2, T3, T4, TResult>(T1 arg_0, T2 arg_1, T3 arg_2, T4 arg_3);

        public delegate ref TResult FullRefFunc<T, TResult>(ref T arg_0);
        public delegate ref TResult FullRefFunc<T1, T2, TResult>(ref T1 arg_0, ref T2 arg_1);
        public delegate ref TResult FullRefFunc<T1, T2, T3, TResult>(ref T1 arg_0, ref T2 arg_1, ref T3 arg_2);
        public delegate ref TResult FullRefFunc<T1, T2, T3, T4, TResult>(ref T1 arg_0, ref T2 arg_1, ref T3 arg_2, ref T4 arg_3);
    }
}
