namespace Oliviann.Collections.Generic
{
    #region Usings

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Reflection;
    using Oliviann.Properties;

    #endregion Usings

    /// <summary>
    /// Represents a generic collection of key/value pairs that are ordered
    /// independently of the key and value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary
    /// </typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary
    /// </typeparam>
    public sealed class OrderedDictionary<TKey, TValue> : IOrderedDictionary<TKey, TValue>
    {
        #region Fields

        /// <summary>
        /// Default dictionary capacity when no capacity is set.
        /// </summary>
        private const int DefaultInitialCapacity = 0;

        /// <summary>
        /// The name of the TKey object.
        /// </summary>
        private static readonly string _keyTypeName = typeof(TKey).FullName;

        /// <summary>
        /// The name of the TValue object.
        /// </summary>
        private static readonly string _valueTypeName = typeof(TValue).FullName;

        /// <summary>
        /// Place holder for determining if TValue is of a value type.
        /// </summary>
        private static readonly bool _valueTypeIsReferenceType = !typeof(ValueType).IsAssignableFrom(typeof(TValue));

        /// <summary>
        /// Comparer implementor for comparing objects in dictionary.
        /// </summary>
        private readonly IEqualityComparer<TKey> _comparer;

        /// <summary>
        /// Place holder for the initial capacity when set.
        /// </summary>
        private readonly int _initialCapacity;

        /// <summary>
        /// Place holder dictionary for data.
        /// </summary>
        private Dictionary<TKey, TValue> _dictionary;

        /// <summary>
        /// Place holder list of indexes of data.
        /// </summary>
        private List<KeyValuePair<TKey, TValue>> _list;

        /// <summary>
        /// Sync object for comparing across threads.
        /// </summary>
        private object _syncRoot;

        #endregion Fields

        #region Constructor/Destructor

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> class using the
        /// specified initial capacity.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> can contain.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> is less than 0</exception>
        public OrderedDictionary(int capacity = DefaultInitialCapacity) : this(capacity, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> class using the
        /// specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> to
        /// use when comparing keys, or null to use the default
        /// <see cref="EqualityComparer{TKey}"/>for the type of the key.</param>
        public OrderedDictionary(IEqualityComparer<TKey> comparer) : this(DefaultInitialCapacity, comparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> class using the
        /// specified initial capacity and comparer.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection can
        /// contain.
        /// </param>
        /// <param name="comparer">The <see cref="IEqualityComparer{TKey}"/> to
        /// use when comparing keys, or null to use the default
        /// <see cref="EqualityComparer{TKey}"/> for the type of the key.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> is less than 0</exception>
        public OrderedDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), Resources.ERR_NonNegativeValue);
            }

            this._initialCapacity = capacity;
            this._comparer = comparer;
        }

        #endregion Constructor/Destructor

        #region Properties

        /// <summary>
        /// Gets the number of key/values pairs contained in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <value>The number of key/value pairs contained in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.</value>
        public int Count => this.List.Count;

        /// <summary>
        /// Gets a value indicating whether access to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> object is synchronized
        /// (thread-safe).
        /// </summary>
        /// <value>This method always returns false.</value>
        bool ICollection.IsSynchronized => false;

        /// <summary>
        /// Gets an object that can be used to synchronize access to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> object.
        /// </summary>
        /// <value>An object that can be used to synchronize access to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> object.</value>
        object ICollection.SyncRoot
        {
            get
            {
                if (this._syncRoot == null)
                {
                    System.Threading.Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
                }

                return this._syncRoot;
            }
        }

        /// <summary>
        /// Gets an <see cref="System.Collections.Generic.ICollection{TKey}"/>
        /// object containing the keys in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <value>An
        /// <see cref="System.Collections.Generic.ICollection{TKey}"/> object
        /// containing the keys in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</value>
        /// <remarks>The returned
        /// <see cref="System.Collections.Generic.ICollection{TKey}"/> object is
        /// not a static copy; instead, the collection refers back to the keys
        /// in the original
        /// <see cref="OrderedDictionary{TKey,TValue}"/>. Therefore, changes to
        /// the <see cref="OrderedDictionary{TKey,TValue}"/> continue to be
        /// reflected in the key collection.</remarks>
        public ICollection<TKey> Keys => this.Dictionary.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection{TValue}"/> object containing the
        /// values in the <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <value>An <see cref="ICollection{TValue}"/> object containing the
        /// values in the <see cref="OrderedDictionary{TKey,TValue}"/>.</value>
        /// <remarks>The returned <see cref="ICollection{TValue}"/> object is
        /// not a static copy; instead, the collection refers back to the values
        /// in the original <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// Therefore, changes to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> continue to be
        /// reflected in the value collection.</remarks>
        public ICollection<TValue> Values => this.Dictionary.Values;

        /// <summary>
        /// Gets a value indicating whether the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> has a fixed size.
        /// </summary>
        /// <value>True if the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> has a fixed size;
        /// otherwise, false. The default is false.</value>
        bool IDictionary.IsFixedSize => false;

        /// <summary>
        /// Gets a value indicating whether the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection is
        /// read-only.
        /// </summary>
        /// <value>True if the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is read-only;
        /// otherwise, false. The default is false.</value>
        /// <remarks>
        /// A collection that is read-only does not allow the addition, removal,
        /// or modification of elements after the collection is created.
        /// <para>A collection that is read-only is simply a collection with a
        /// wrapper that prevents modification of the collection; therefore, if
        /// changes are made to the underlying collection, the read-only
        /// collection reflects those changes.</para>
        /// </remarks>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets an <see cref="ICollection"/> object containing the keys in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <value>An <see cref="ICollection"/> object containing the keys in
        /// the <see cref="OrderedDictionary{TKey,TValue}"/>.</value>
        /// <remarks>The returned <see cref="ICollection"/> object is not a
        /// static copy; instead, the collection refers back to the keys in the
        /// original <see cref="OrderedDictionary{TKey,TValue}"/>. Therefore,
        /// changes to the <see cref="OrderedDictionary{TKey,TValue}"/> continue
        /// to be reflected in the key collection.</remarks>
        ICollection IDictionary.Keys => (ICollection)this.Keys;

        /// <summary>
        /// Gets an <see cref="ICollection"/> object containing the values in
        /// the <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <value>An <see cref="ICollection"/> object containing the values in
        /// the <see cref="OrderedDictionary{TKey,TValue}"/> collection.</value>
        /// <remarks>The returned <see cref="ICollection"/> object is not a
        /// static copy; instead, the <see cref="ICollection"/> refers back to
        /// the values in the original
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection. Therefore,
        /// changes to the <see cref="OrderedDictionary{TKey,TValue}"/> continue
        /// to be reflected in the <see cref="ICollection"/>.</remarks>
        ICollection IDictionary.Values => (ICollection)this.Values;

        /// <summary>
        /// Gets the dictionary object that stores the keys and values.
        /// </summary>
        /// <value>The dictionary object that stores the keys and values for the
        /// <see cref="OrderedDictionary{TKey,TValue}"/></value>
        /// <remarks>Accessing this property will create the dictionary object
        /// if necessary.</remarks>
        private Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                return this._dictionary ?? (this._dictionary = new Dictionary<TKey, TValue>(this._initialCapacity, this._comparer));
            }
        }

        /// <summary>
        /// Gets the list object that stores the key/value pairs.
        /// </summary>
        /// <value>The list object that stores the key/value pairs for the
        /// <see cref="OrderedDictionary{TKey,TValue}"/></value>
        /// <remarks>Accessing this property will create the list object if
        /// necessary.</remarks>
        private List<KeyValuePair<TKey, TValue>> List
        {
            get
            {
                return this._list ?? (this._list = new List<KeyValuePair<TKey, TValue>>(this._initialCapacity));
            }
        }

        #endregion Properties

        #region Indexes

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.
        /// </param>
        /// <returns>The value of the item at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// index is equal to or greater than <see cref="Count"/>.</exception>
        public TValue this[int index]
        {
            get
            {
                return this.List[index].Value;
            }

            set
            {
                if (index >= this.Count || index < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(index), Resources.ERR_NonNegativeValueAndLessSize);
                }

                TKey key = this.List[index].Key;

                this.List[index] = new KeyValuePair<TKey, TValue>(key, value);
                this.Dictionary[key] = value;
            }
        }

        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.
        /// </param>
        /// <returns>The value of the item at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// index is equal to or greater than <see cref="Count"/>.</exception>
        /// <exception cref="ArgumentNullException">
        /// valueObject is a null reference, and the value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is a value type.
        /// </exception>
        /// <exception cref="ArgumentException">The value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of valueObject.</exception>
        object IOrderedDictionary.this[int index]
        {
            get { return this[index]; }
            set { this[index] = ConvertToValueType(value); }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <value>The value associated with the specified key. If the specified
        /// key is not found, attempting to get it returns null, and attempting
        /// to set it creates a new element using the specified key.
        /// </value>
        /// <returns>The value associated with the specified key.</returns>
        public TValue this[TKey key]
        {
            get
            {
                return this.Dictionary[key];
            }

            set
            {
                if (this.Dictionary.ContainsKey(key))
                {
                    this.Dictionary[key] = value;
                    this.List[this.IndexOfKey(key)] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    this.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get or set.</param>
        /// <value>The value associated with the specified key. If the specified
        /// key is not found, attempting to get it returns null, and attempting
        /// to set it creates a new element using the specified key.
        /// </value>
        /// <returns>
        /// The value associated with the specified key, or null if
        /// <paramref name="key"/> is not in the dictionary or
        /// <paramref name="key"/> is of a type that is not assignable to the
        /// key type <typeparamref name="TKey"/> of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </returns>
        object IDictionary.this[object key]
        {
            get { return this[ConvertToKeyType(key)]; }
            set { this[ConvertToKeyType(key)] = ConvertToValueType(value); }
        }

        #endregion Indexes

        #region Enumerators

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// <see cref="T:System.Collections.Specialized.IOrderedDictionary"/>
        /// collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionaryEnumerator"/> for the
        /// entire
        /// <see cref="T:System.Collections.Specialized.IOrderedDictionary"/>
        /// collection.
        /// </returns>
        IDictionaryEnumerator IOrderedDictionary.GetEnumerator() => this.Dictionary.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the
        /// <see cref="T:System.Collections.Specialized.IOrderedDictionary"/>
        /// collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IDictionaryEnumerator"/> for the
        /// entire
        /// <see cref="T:System.Collections.Specialized.IOrderedDictionary"/>
        /// collection.
        /// </returns>
        IDictionaryEnumerator IDictionary.GetEnumerator() => this.Dictionary.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be
        /// used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator() => this.List.GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that
        /// can be used to iterate through the collection.
        /// </returns>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => this.List.GetEnumerator();

        #endregion Enumerators

        #region Adds

        /// <summary>
        /// Adds the specified value to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> with the specified key.
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/>
        /// structure representing the key and value to add to the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

        /// <summary>
        /// Adds an entry with the specified key and value into the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection with the
        /// lowest available index.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. This value can be
        /// null.</param>
        /// <remarks>A key cannot be null, but a value can be.
        /// <para>You can also use the
        /// <see cref="M:OrderedDictionary{TKey,TValue}.Item(TKey)"/> property to
        /// add new elements by setting the value of a key that does not exist
        /// in the <see cref="OrderedDictionary{TKey,TValue}"/>collection;
        /// however, if the specified key already exists in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>, setting the
        /// <see cref="M:OrderedDictionary{TKey,TValue}.Item(TKey)"/> property
        /// overwrites the old value. In contrast, the <see cref="Add"/> method
        /// does not modify existing elements.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">An element with the same key
        /// already exists in the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// </exception>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value) => this.Add(key, value);

        /// <summary>
        /// Adds an entry with the specified key and value into the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection with the
        /// lowest available index.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. This value can be
        /// null.</param>
        /// <returns>The index of the newly added entry</returns>
        /// <remarks>A key cannot be null, but a value can be.
        /// <para>You can also use the
        /// <see cref="M:OrderedDictionary{TKey,TValue}.Item(TKey)"/> property to
        /// add new elements by setting the value of a key that does not exist
        /// in the <see cref="OrderedDictionary{TKey,TValue}"/> collection;
        /// however, if the specified key already exists in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>, setting the
        /// <see cref="M:OrderedDictionary{TKey,TValue}.Item(TKey)"/> property
        /// overwrites the old value. In contrast, the <see cref="Add"/> method
        /// does not modify existing elements.</para></remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">An element with the same key
        /// already exists in the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// </exception>
        public int Add(TKey key, TValue value)
        {
            this.Dictionary.Add(key, value);
            this.List.Add(new KeyValuePair<TKey, TValue>(key, value));
            return this.Count - 1;
        }

        /// <summary>
        /// Adds an entry with the specified key and value into the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection with the
        /// lowest available index.
        /// </summary>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. This value can be
        /// null.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.<br/>
        /// -or-<br/>
        /// <paramref name="value"/> is null, and the value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is a value type.
        /// </exception>
        /// <exception cref="ArgumentException">The key type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of <paramref name="key"/>.<br/>
        /// -or-<br/>
        /// The value type of the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// is not in the inheritance hierarchy of <paramref name="value"/>.
        /// </exception>
        void IDictionary.Add(object key, object value) => this.Add(ConvertToKeyType(key), ConvertToValueType(value));

        /// <summary>
        /// Inserts a new entry into the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection with the
        /// specified key and value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the element should
        /// be inserted.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. The value can be
        /// null if the type of the values in the dictionary is a reference
        /// type.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// <paramref name="index"/> is greater than <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">An element with the same key
        /// already exists in the <see cref="OrderedDictionary{TKey,TValue}"/>.
        /// </exception>
        public void Insert(int index, TKey key, TValue value)
        {
            if (index > this.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.Dictionary.Add(key, value);
            this.List.Insert(index, new KeyValuePair<TKey, TValue>(key, value));
        }

        /// <summary>
        /// Inserts a new entry into the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection with the
        /// specified key and value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the element should
        /// be inserted.</param>
        /// <param name="key">The key of the entry to add.</param>
        /// <param name="value">The value of the entry to add. The value can be
        /// null if the type of the values in the dictionary is a reference
        /// type.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// <paramref name="index"/> is greater than <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.<br/>
        /// -or-<br/>
        /// <paramref name="value"/> is null, and the value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is a value type.
        /// </exception>
        /// <exception cref="ArgumentException">The key type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of <paramref name="key"/>.<br/>
        /// -or-<br/>
        /// The value type of the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// is not in the inheritance hierarchy of <paramref name="value"/>.
        /// <br/>
        /// -or-<br/>
        /// An element with the same key already exists in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</exception>
        void IOrderedDictionary.Insert(int index, object key, object value)
        {
            this.Insert(index, ConvertToKeyType(key), ConvertToValueType(value));
        }

        #endregion Adds

        #region Removes

        /// <summary>
        /// Removes a key and value from the dictionary.
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/>
        /// structure representing the key and value to remove from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</param>
        /// <returns>True if the key and value represented by
        /// <paramref name="item"/> is successfully found and removed;
        /// otherwise, false. This method returns false if
        /// <paramref name="item"/> is not found in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</returns>
        /// <remarks>Even though you have to pass a
        /// <see cref="KeyValuePair{TKey,TValue}"/> object, only the key is used
        /// to match a specific item.</remarks>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        /// <summary>
        /// Removes the entry with the specified key from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <param name="key">The key of the entry to remove</param>
        /// <returns>True if the key was found and the corresponding element was
        /// removed; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is
        /// null.</exception>
        public bool Remove(TKey key)
        {
            ADP.CheckArgumentNull(key, nameof(key));

            int index = this.IndexOfKey(key);
            if (index >= 0 && this.Dictionary.Remove(key))
            {
                this.List.RemoveAt(index);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the entry with the specified key from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <param name="key">The key of the entry to remove</param>
        void IDictionary.Remove(object key)
        {
            this.Remove(ConvertToKeyType(key));
        }

        /// <summary>
        /// Removes the entry at the specified index from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <param name="index">The zero-based index of the entry to remove.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> is less than 0.<br/>
        /// -or-<br/>
        /// index is equal to or greater than <see cref="Count"/>.</exception>
        public void RemoveAt(int index)
        {
            if (index >= this.Count || index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index), Resources.ERR_NonNegativeValueAndLessSize);
            }

            TKey key = this.List[index].Key;
            this.List.RemoveAt(index);
            this.Dictionary.Remove(key);
        }

        /// <summary>
        /// Removes all elements from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.
        /// </summary>
        /// <remarks>The capacity is not changed as a result of calling this
        /// method.</remarks>
        public void Clear()
        {
            this.Dictionary.Clear();
            this.List.Clear();
        }

        #endregion Removes

        #region Lookups

        /// <summary>
        /// Gets the specified index key/value pair.
        /// </summary>
        /// <param name="index">The zero-based index of the value to get or set.
        /// </param>
        /// <returns>The specified index key/value pair.</returns>
        /// <exception cref="IndexOutOfRangeException">The index was outside the
        /// bounds of the dictionary. [index]</exception>
        public KeyValuePair<TKey, TValue> GetItem(int index)
        {
            if (index < 0 || index >= this.Count)
            {
                throw new IndexOutOfRangeException(Resources.ERR_DictIndexOutBounds + " " + index);
            }

            return this.List[index];
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value
        /// associated with the specified key, if the key is found; otherwise,
        /// the default value for the type of <paramref name="value"/>. This
        /// parameter can be passed uninitialized.</param>
        /// <returns>True if the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// contains an element with the specified key; otherwise, false.
        /// </returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.Dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// Determines whether the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// contains a specific key and value.
        /// </summary>
        /// <param name="item">The <see cref="KeyValuePair{TKey,TValue}"/>
        /// structure to locate in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>.</param>
        /// <returns>True if <paramref name="item"/> is found in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>; otherwise, false.
        /// </returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).Contains(item);
        }

        /// <summary>
        /// Determines whether the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// collection contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.</param>
        /// <returns>True if the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection contains an
        /// element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the <see cref="OrderedDictionary{TKey,TValue}"/>
        /// collection contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection.</param>
        /// <returns>True if the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> collection contains an
        /// element with the specified key; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is
        /// null.</exception>
        /// <exception cref="ArgumentException">The key type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of <paramref name="key"/>.</exception>
        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey(ConvertToKeyType(key));
        }

        /// <summary>
        /// Returns the zero-based index of the specified key in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>
        /// </summary>
        /// <param name="key">The key to locate in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/></param>
        /// <returns>The zero-based index of <paramref name="key"/>, if
        /// <paramref name="key"/> is found in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>; otherwise, -1
        /// </returns>
        /// <remarks>This method performs a linear search; therefore it has a
        /// cost of O(n) at worst.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is
        /// null.</exception>
        public int IndexOfKey(TKey key)
        {
            ADP.CheckArgumentNull(key, nameof(key));

            for (int index = 0; index < this.List.Count; index += 1)
            {
                KeyValuePair<TKey, TValue> entry = this.List[index];
                TKey next = entry.Key;
                if (this._comparer != null)
                {
                    if (this._comparer.Equals(next, key))
                    {
                        return index;
                    }
                }
                else if (next.Equals(key))
                {
                    return index;
                }
            }

            return -1;
        }

        #endregion Lookups

        #region Converts

        /// <summary>
        /// Copies the elements of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> elements to a
        /// one-dimensional <see cref="Array"/> object at the specified index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the
        /// destination of the elements copied from <see cref="ICollection{T}"/>.
        /// The array must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/>
        /// at which copying begins.</param>
        /// <remarks>Preserves the order of the elements in the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>
        /// </remarks>
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.List).CopyTo(array, index);
        }

        /// <summary>
        /// Copies the elements of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> to an array of type
        /// <see cref="KeyValuePair{TKey,TValue}"/>, starting at the specified
        /// index.
        /// </summary>
        /// <param name="array">The one-dimensional array of type
        /// <see cref="KeyValuePair{TKey,TValue}"/> that is the destination of
        /// the
        /// <see cref="KeyValuePair{TKey,TValue}"/> elements copied from the
        /// <see cref="OrderedDictionary{TKey,TValue}"/>. The array must have
        /// zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in
        /// <paramref name="array"/> at which copying begins.</param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.Dictionary).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Converts the object passed as a key to the key type of the
        /// dictionary.
        /// </summary>
        /// <param name="key">The key object to check</param>
        /// <returns>The key object, cast as the key type of the dictionary
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/>
        /// is null.</exception>
        /// <exception cref="ArgumentException">The key type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of <paramref name="key"/>.</exception>
        private static TKey ConvertToKeyType(object key)
        {
            ADP.CheckArgumentNull(key, nameof(key));
            if (key is TKey tk)
            {
                return tk;
            }

            throw ADP.ArgumentEx(nameof(key), Resources.ERR_MustBeType + " " + _keyTypeName);
        }

        /// <summary>
        /// Converts the object passed as a value to the value type of the
        /// dictionary
        /// </summary>
        /// <param name="value">The object to convert to the value type of the
        /// dictionary</param>
        /// <returns>The value object, converted to the value type of the
        /// dictionary</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null, and the value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is a value type.
        /// </exception>
        /// <exception cref="ArgumentException">The value type of the
        /// <see cref="OrderedDictionary{TKey,TValue}"/> is not in the
        /// inheritance hierarchy of <paramref name="value"/>.</exception>
        private static TValue ConvertToValueType(object value)
        {
            if (value == null)
            {
                if (_valueTypeIsReferenceType)
                {
                    return default(TValue);
                }

                throw ADP.ArgumentNull(nameof(value));
            }

            if (value is TValue convertedValue)
            {
                return convertedValue;
            }

            throw ADP.ArgumentEx(nameof(value), Resources.ERR_MustBeType + " " + _valueTypeName);
        }

        #endregion Converts
    }
}