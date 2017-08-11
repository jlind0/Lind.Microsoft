using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Linq;

namespace Lind.Microsoft.Core
{
    public class ObservableDictionary<K, T> : IDictionary<K, T>, INotifyCollectionChanged
    {
        protected Dictionary<K, T> Holder { get; private set; } = new Dictionary<K, T>();
        public T this[K key]
        {
            get { return Holder[key]; }
            set
            {
                T oldVal;
                Holder.TryGetValue(key, out oldVal);
                Holder[key] = value;
                this.CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value, oldVal));
            }
        }

        public ICollection<K> Keys => Holder.Keys;

        public ICollection<T> Values => Holder.Values;

        public int Count => Holder.Count;

        public bool IsReadOnly => false;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void Add(K key, T value)
        {
            this.Holder.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value, null));
        }

        public void Add(KeyValuePair<K, T> item)
        {
            this.Holder.Add(item.Key, item.Value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item.Value, null));
        }

        public void Clear()
        {
            var items = this.Values;
            this.Holder.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, null, items.ToList()));
        }

        public bool Contains(KeyValuePair<K, T> item)
        {
            return this.Holder.Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return this.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, T>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<K, T>> GetEnumerator()
        {
            return this.Holder.GetEnumerator();
        }

        public bool Remove(K key)
        {
            if (this.Holder.ContainsKey(key))
            {
                var item = this.Holder[key];
                var remove = this.Remove(key);
                this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, null, item));
                return remove;
            }
            return false;
        }

        public bool Remove(KeyValuePair<K, T> item)
        {
            var remove = this.Remove(item);
            if (remove)
                this.CollectionChanged.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, null, item.Value));
            return remove;
        }

        public bool TryGetValue(K key, out T value)
        {
            return this.Holder.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Holder.GetEnumerator();
        }
    }
}
