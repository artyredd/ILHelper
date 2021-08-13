using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class BitMask
    {
        public BitMask()
        {

        }

        /// <summary>
        /// You can avoid dynamically creating a mask at runtime by providing one
        /// </summary>
        /// <param name="Mask"></param>
        public BitMask(int Mask)
        {
            Value = Mask;
        }

        public int Value { get; private set; } = 0b_0;

        public void AddValue<T>(T Value) where T : struct, System.Enum, IConvertible
        {
            AddValue(Value.ToInt32(null));
        }

        public bool ContainsValue<T>(T Value) where T : struct, System.Enum, IConvertible
        {
            return ContainsValue(Value.ToInt32(null));
        }

        public void AddIndex<T>(T Index) where T : struct, System.Enum, IConvertible
        {
            AddIndex(Index.ToInt32(null));
        }

        public bool ContainsIndex<T>(T Index) where T : struct, System.Enum, IConvertible
        {
            return ContainsIndex(Index.ToInt32(null));
        }

        public void AddIndex(int Index)
        {
            Value |= 1 << Index;
        }

        public void AddValue(int Value)
        {
            this.Value |= Value;
        }

        public bool ContainsIndex(int Index)
        {
            return (Value & (1 << Index)) != 0;
        }

        public bool ContainsValue(int Value)
        {
            return (this.Value & Value) != 0;
        }
    }
}
