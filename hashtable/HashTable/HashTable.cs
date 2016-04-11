using System;
using System.Collections.Generic;

namespace HashTable
{
    /// <summary>
    ///     A key/value associative collection
    /// </summary>
    /// <typeparam name="TKey">The key type of the key/value pair</typeparam>
    /// <typeparam name="TValue">The value type of the key/value pair</typeparam>
    public class HashTable<TKey, TValue>
    {
        // If the array exceeds this fill percentage it will grow
        // In this example the fill factor is the total number of items
        // regardless of whether they are collisions or not.
        private const double _fillFactor = 0.75;

        // The array where the items are stored.
        private HashTableArray<TKey, TValue> _array;

        // the number of items in the hash table

        // the maximum number of items to store before growing.
        // This is just a cached value of the fill factor calculation
        private int _maxItemsAtCurrentSize;

        /// <summary>
        ///     Constructs a hash table with the default capacity
        /// </summary>
        public HashTable()
            : this(1000)
        {
        }

        /// <summary>
        ///     Constructs a hash table with the specified capacity
        /// </summary>
        public HashTable(int initialCapacity)
        {
            if (initialCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("initialCapacity");
            }

            _array = new HashTableArray<TKey, TValue>(initialCapacity);

            // when the count exceeds this value, the next Add will cause the
            // array to grow
            _maxItemsAtCurrentSize = (int) (initialCapacity*_fillFactor) + 1;
        }

        /// <summary>
        ///     Gets and sets the value with the specified key.  ArgumentException is
        ///     thrown if the key does not already exist in the hash table.
        /// </summary>
        /// <param name="key">The key of the value to retrieve</param>
        /// <returns>The value associated with the specified key</returns>
        public TValue this[TKey key]
        {
            get
            {
                TValue value;
                if (!_array.TryGetValue(key, out value))
                {
                    throw new ArgumentException("key");
                }

                return value;
            }
            set { _array.Update(key, value); }
        }

        /// <summary>
        ///     Returns an enumerator for all of the keys in the hash table
        /// </summary>
        public IEnumerable<TKey> Keys
        {
            get
            {
                foreach (var key in _array.Keys)
                {
                    yield return key;
                }
            }
        }

        /// <summary>
        ///     Returns an enumerator for all of the values in the hash table
        /// </summary>
        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (var value in _array.Values)
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        ///     The number of items currently in the hash table
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        ///     Adds the key/value pair to the hash table.  If the key already exists in the
        ///     hash table an ArgumentException will be thrown
        /// </summary>
        /// <param name="key">The key of the item being added</param>
        /// <param name="value">The value of the item being added</param>
        public void Add(TKey key, TValue value)
        {
            // if we are at capacity, the array needs to grow
            if (Count >= _maxItemsAtCurrentSize)
            {
                // allocate a larger array
                var largerArray = new HashTableArray<TKey, TValue>(_array.Capacity*2);

                // and re-add each item to the new array
                foreach (var node in _array.Items)
                {
                    largerArray.Add(node.Key, node.Value);
                }

                // the larger array is now the hash table storage
                _array = largerArray;

                // update the new max items cached value
                _maxItemsAtCurrentSize = (int) (_array.Capacity*_fillFactor) + 1;
            }

            _array.Add(key, value);
            Count++;
        }

        /// <summary>
        ///     Removes the item from the hash table whose key matches
        ///     the specified key.
        /// </summary>
        /// <param name="key">The key of the item to remove</param>
        /// <returns>True if the item was removed, false otherwise.</returns>
        public bool Remove(TKey key)
        {
            var removed = _array.Remove(key);
            if (removed)
            {
                Count--;
            }

            return removed;
        }

        /// <summary>
        ///     Finds and returns the value for the specified key.
        /// </summary>
        /// <param name="key">The key whose value is sought</param>
        /// <param name="value">The value associated with the specified key</param>
        /// <returns>True if the value was found, false otherwise</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _array.TryGetValue(key, out value);
        }

        /// <summary>
        ///     Returns a Boolean indicating if the hash table contains the specified key.
        /// </summary>
        /// <param name="key">The key whose existence is being tested</param>
        /// <returns>True if the value exists in the hash table, false otherwise.</returns>
        public bool ContainsKey(TKey key)
        {
            TValue value;
            return _array.TryGetValue(key, out value);
        }

        /// <summary>
        ///     Returns a Boolean indicating if the hash table contains the specified value.
        /// </summary>
        /// <param name="value">The value whose existence is being tested</param>
        /// <returns>True if the value exists in the hash table, false otherwise.</returns>
        public bool ContainsValue(TValue value)
        {
            foreach (var foundValue in _array.Values)
            {
                if (value.Equals(foundValue))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Removes all items from the hash table
        /// </summary>
        public void Clear()
        {
            _array.Clear();
            Count = 0;
        }
    }
}