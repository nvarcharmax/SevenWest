using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace SevenWest.Tests.Helpers
{
    public class FakeMemoryCache : IMemoryCache
    {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public void Dispose()
        {
            _values.Clear();
        }

        public bool TryGetValue(object key, out object value)
        {
            return _values.TryGetValue(key.ToString(), out value);
        }

        public ICacheEntry CreateEntry(object key)
        {
            return new FakeCacheEntry((s) => { _values[key.ToString()] = s; });
        }

        public void Remove(object key)
        {
            _values.Remove(key.ToString());
        }

        public class FakeCacheEntry : ICacheEntry
        {
            private Action<object> _setValueAction;
            private object _value;

            public FakeCacheEntry(Action<object> setValueAction)
            {
                _setValueAction = setValueAction;
            }

            public void Dispose()
            {
            }

            public object Key { get; }

            public object Value
            {
                get { return _value; }
                set
                {
                    _value = value;
                    _setValueAction.Invoke(_value);
                }
            }

            public DateTimeOffset? AbsoluteExpiration { get; set; }

            public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

            public TimeSpan? SlidingExpiration { get; set; }

            public IList<IChangeToken> ExpirationTokens { get; } = new List<IChangeToken>();

            public IList<PostEvictionCallbackRegistration> PostEvictionCallbacks { get; } = new List<PostEvictionCallbackRegistration>();

            public CacheItemPriority Priority { get; set; }

            public long? Size { get; set; }
        }
    }
}
