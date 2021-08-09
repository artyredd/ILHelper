using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILHelper.Linter
{
    public class GenericMapper<T, U> : IMapper<T, U>
    {
        public IDictionary<T, U> Mappings => _mappings;
        protected readonly ConcurrentDictionary<T, U> _mappings = new();


        public GenericMapper<T, U> Map(T From, U To)
        {
            AddOrUpdate(From, To);

            return this;
        }

        public U GetMapping(T From)
        {
            if (TryGet(From, out U result))
            {
                return result;
            }
            return default;
        }

        public bool TryGet(T From, out U To)
        {
            return _mappings.TryGetValue(From, out To);
        }

        private void AddOrUpdate(T From, U To)
        {
            if (_mappings.ContainsKey(From))
            {
                _mappings[From] = To;
            }
            else
            {
                _mappings.TryAdd(From, To);
            }
        }
    }
}
