using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static T As<T>(this MethodInfo method) where T : Delegate
        {
            return (T)method.CreateDelegate(typeof(T));
        }

        public static IDynamicMethodBuilder Emit(this IDynamicMethodBuilder builder, OpCode Instruction, FieldInfo Field)
        {
            builder.Emit((factory, list) => list.Add(factory.Create(Instruction, Field)));

            return builder;
        }

        public static IDynamicMethodBuilder Emit(this IDynamicMethodBuilder builder, OpCode Instruction, MethodInfo Method)
        {
            builder.Emit((factory, list) => list.Add(factory.Create(Instruction, Method)));

            return builder;
        }

        public static IDynamicMethodBuilder Emit(this IDynamicMethodBuilder builder, OpCode Instruction, Type Type)
        {
            builder.Emit((factory, list) => list.Add(factory.Create(Instruction, Type)));

            return builder;
        }

        public static IDynamicMethodBuilder Emit(this IDynamicMethodBuilder builder, OpCode Instruction, string MemberName)
        {
            builder.Emit((factory, list, builder) =>
            {
                if (TryGetMethod(builder, MemberName, out MethodInfo method))
                {
                    list.Add(factory.Create(Instruction, method));
                }
                else if (TryGetField(builder, MemberName, out FieldInfo field))
                {
                    list.Add(factory.Create(Instruction, field));
                }
                else
                {
                    throw Helpers.Exceptions.Generic($"{builder.Name} does not contain a member with the name {MemberName}");
                }
            });

            return builder;
        }

        private static bool TryGetMethod(IDynamicTypeBuilder builder, string MemberName, out MethodInfo info)
        {
            info = builder.Method(MemberName);

            return info is not null;
        }
        private static bool TryGetField(IDynamicTypeBuilder builder, string MemberName, out FieldInfo info)
        {
            info = builder.Field(MemberName);

            return info is not null;
        }
        private static bool TryGetProperty(IDynamicTypeBuilder builder, string MemberName, out PropertyInfo info)
        {
            info = builder.Property(MemberName);

            return info is not null;
        }
    }
}
