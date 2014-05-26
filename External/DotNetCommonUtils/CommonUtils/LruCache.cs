using System.Collections.Generic;

namespace CommonUtils
{
    public class LruCache<TK, TV>
    {
        public LruCache(int capacity)
        {
            _capacity = capacity;
        }

        public TV Get(TK key)
        {
            LinkedListNode<KeyValuePair<TK, TV>> node;
            if (_cacheMap.TryGetValue(key, out node))
            {
                TV value = node.Value.Value;

                _lruList.Remove(node);
                _lruList.AddLast(node);
                return value;
            }
            return default(TV);
        }

        public void Add(TK key, TV val)
        {
            if (_cacheMap.Count >= _capacity)
            {
                RemoveFirst();
            }
            var cacheItem = new KeyValuePair<TK, TV>(key, val);
            var node = new LinkedListNode<KeyValuePair<TK, TV>>(cacheItem);
            _lruList.AddLast(node);
            _cacheMap.Add(key, node);
        }


        protected void RemoveFirst()
        {
            // Remove from LRUPriority
            var node = _lruList.First;
            _lruList.RemoveFirst();
            // Remove from cache
            _cacheMap.Remove(node.Value.Key);
        }

        readonly int _capacity;
        readonly Dictionary<TK, LinkedListNode<KeyValuePair<TK, TV>>> _cacheMap = new Dictionary<TK, LinkedListNode<KeyValuePair<TK, TV>>>();
        readonly LinkedList<KeyValuePair<TK, TV>> _lruList = new LinkedList<KeyValuePair<TK, TV>>();
    }
}
