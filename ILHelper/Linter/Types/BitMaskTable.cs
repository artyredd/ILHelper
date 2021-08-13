using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class BitMaskTable<TKey, TSubkey>
    {
        /// <summary>
        /// Mask Table
        /// Key: U Value: [ 010101 ]
        /// 
        /// Int
        /// Key: Add Value [ true, false, true, false ]
        /// 
        /// Float
        /// Key: Add Value [ true, false, true, false ]
        /// </summary>
        private readonly Dictionary<TKey, Dictionary<TSubkey, BitMask>> BackingDictionary = new();

        public void AddValue(TKey Key, TSubkey Subkey, int Value)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                AddValue(BackingDictionary[Key], Subkey, Value);
                return;
            }

            Dictionary<TSubkey, BitMask> newDict = new();

            AddValue(newDict, Subkey, Value);

            BackingDictionary.Add(Key, newDict);
        }

        public void AddIndex(TKey Key, TSubkey Subkey, int Index)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                AddIndex(BackingDictionary[Key], Subkey, Index);
                return;
            }

            Dictionary<TSubkey, BitMask> newDict = new();

            AddIndex(newDict, Subkey, Index);

            BackingDictionary.Add(Key, newDict);
        }

        public void AddValue<V>(TKey Key, TSubkey Subkey, V Value) where V : struct, System.Enum, IConvertible
        {
            AddValue(Key, Subkey, Value.ToInt32(null));
        }

        public void AddIndex<TEnum>(TKey Key, TSubkey Subkey, TEnum Index) where TEnum : struct, System.Enum, IConvertible
        {
            AddIndex(Key, Subkey, Index.ToInt32(null));
        }

        public bool ContainsValue(TKey Key, TSubkey Subkey, int Value)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                if (BackingDictionary[Key].ContainsKey(Subkey))
                {
                    return BackingDictionary[Key][Subkey].ContainsValue(Value);
                }
            }
            return false;
        }

        public bool ContainsIndex(TKey Key, TSubkey Subkey, int Index)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                if (BackingDictionary[Key].ContainsKey(Subkey))
                {
                    return BackingDictionary[Key][Subkey].ContainsIndex(Index);
                }
            }
            return false;
        }

        public bool ContainsValue<TEnum>(TKey Key, TSubkey Subkey, TEnum Value) where TEnum : struct, System.Enum, IConvertible
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                if (BackingDictionary[Key].ContainsKey(Subkey))
                {
                    return BackingDictionary[Key][Subkey].ContainsValue(Value.ToInt32(null));
                }
            }
            return false;
        }

        public bool ContainsIndex<TEnum>(TKey Key, TSubkey Subkey, TEnum Index) where TEnum : struct, System.Enum, IConvertible
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                if (BackingDictionary[Key].ContainsKey(Subkey))
                {
                    return BackingDictionary[Key][Subkey].ContainsIndex(Index.ToInt32(null));
                }
            }
            return false;
        }

        private void AddValue(Dictionary<TSubkey, BitMask> dict, TSubkey Key, int Value)
        {
            if (dict.ContainsKey(Key))
            {
                dict[Key].AddValue(Value);
                return;
            }
            dict.Add(Key, new(Value));
        }

        private void AddIndex(Dictionary<TSubkey, BitMask> dict, TSubkey Key, int Index)
        {
            if (dict.ContainsKey(Key))
            {
                dict[Key].AddIndex(Index);
                return;
            }
            dict.Add(Key, new(1 << Index));
        }
    }
}
