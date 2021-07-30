using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper
{
    public static class DynamicBuilderExtenstions
    {
        /// <summary>
        /// Defines the parameters that should be accepted by the <see cref="DynamicMethod"/> when it is built
        /// </summary>
        /// <returns>The same object</returns>
        public static IDynamicMethodBuilder Accepts<T>(this IDynamicMethodBuilder builder)
        {
            return builder.Accepts(typeof(T));
        }

        /// <summary>
        /// Defines the parameters that should be accepted by the <see cref="DynamicMethod"/> when it is built
        /// </summary>
        /// <returns>The same object</returns>
        public static IDynamicMethodBuilder Accepts<T1, T2>(this IDynamicMethodBuilder builder)
        {
            return builder.Accepts(typeof(T1), typeof(T2));
        }

        /// <summary>
        /// Defines the parameters that should be accepted by the <see cref="DynamicMethod"/> when it is built
        /// </summary>
        /// <returns>The same object</returns>
        public static IDynamicMethodBuilder Accepts<T1, T2, T3>(this IDynamicMethodBuilder builder)
        {
            return builder.Accepts(typeof(T1), typeof(T2), typeof(T3));
        }

        /// <summary>
        /// Defines the parameters that should be accepted by the <see cref="DynamicMethod"/> when it is built
        /// </summary>
        /// <returns>The same object</returns>
        public static IDynamicMethodBuilder Accepts<T1, T2, T3, T4>(this IDynamicMethodBuilder builder)
        {
            return builder.Accepts(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
        }

        /// <summary>
        /// Defines the parameters that should be accepted by the <see cref="DynamicMethod"/> when it is built
        /// </summary>
        /// <returns>The same object</returns>
        public static IDynamicMethodBuilder Accepts<T1, T2, T3, T4, T5>(this IDynamicMethodBuilder builder)
        {
            return builder.Accepts(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
        }

        /// <summary>
        /// Assigns the <see cref="Type"/>  that the builder should return when it builds a <see cref="DynamicMethod"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDynamicMethodBuilder Returns<T>(this IDynamicMethodBuilder builder)
        {
            return builder.Returns(typeof(T));
        }

        /// <summary>
        /// Creates a delegate of <see cref="Type"/> &lt;<typeparamref name="T"/>&gt; from the <see cref="DynamicMethod"/> provided.
        /// <para>
        /// This is a short for
        /// </para>
        /// <code>
        /// (T)method.CreateDelegate(typeof(T));
        /// </code>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <returns></returns>
        public static T As<T>(this DynamicMethod method) where T : Delegate
        {
            return (T)method.CreateDelegate(typeof(T));
        }
    }
}
