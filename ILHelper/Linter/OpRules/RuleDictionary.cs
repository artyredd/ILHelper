using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class RuleDictionary<TKey, TValue> : IDictionary<TKey, RuleCollection<TValue>>
    {
        private readonly ConcurrentDictionary<TKey, RuleCollection<TValue>> BackingDictionary = new();

        public RuleCollection<TValue> this[TKey key]
        {
            get => BackingDictionary[key];
            set => BackingDictionary[key] = value;
        }

        public ICollection<TKey> Keys => BackingDictionary.Keys;

        public ICollection<RuleCollection<TValue>> Values => BackingDictionary.Values;

        public int Count => BackingDictionary.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, RuleCollection<TValue>>>)BackingDictionary).IsReadOnly;

        public RuleCollection<TValue> Case(TKey Key)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                return BackingDictionary[Key];
            }

            var newCollection = new RuleCollection<TValue>();

            if (BackingDictionary.TryAdd(Key, newCollection))
            {
                return newCollection;
            }

            throw Helpers.Exceptions.Generic($"Failed to create a new case for {Key} in {nameof(RuleDictionary<TKey, TValue>)}");
        }

        public bool Evaluate(TKey Key, TValue State)
        {
            return Evaluate(Key, State, out _);
        }

        public bool Evaluate(TKey Key, TValue State, out string Message)
        {
            if (BackingDictionary.ContainsKey(Key))
            {
                return BackingDictionary[Key].Evaluate(State, out Message);
            }

            Message = string.Empty;
            return false;
        }

        // Overrides
        public void Add(TKey key, RuleCollection<TValue> value)
        {
            ((IDictionary<TKey, RuleCollection<TValue>>)BackingDictionary).Add(key, value);
        }

        public void Add(KeyValuePair<TKey, RuleCollection<TValue>> item)
        {
            ((ICollection<KeyValuePair<TKey, RuleCollection<TValue>>>)BackingDictionary).Add(item);
        }

        public void Clear()
        {
            BackingDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, RuleCollection<TValue>> item)
        {
            return BackingDictionary.Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return BackingDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, RuleCollection<TValue>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, RuleCollection<TValue>>>)BackingDictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, RuleCollection<TValue>>> GetEnumerator()
        {
            return BackingDictionary.GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, RuleCollection<TValue>>)BackingDictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, RuleCollection<TValue>> item)
        {
            return ((ICollection<KeyValuePair<TKey, RuleCollection<TValue>>>)BackingDictionary).Remove(item);
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out RuleCollection<TValue> value)
        {
            return BackingDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BackingDictionary.GetEnumerator();
        }
    }
}
