using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DevGuideToCollections
{
    [DebuggerDisplay("Count={Count}")]
    [DebuggerTypeProxy(typeof(ArrayDebugView))]
    public class ArrayEx<T>
    {
        // Fields
        private const int GROW_BY = 10;
        private int m_count;
        private T[] m_data;
        private int m_updateCode;

        // Constructors
        public ArrayEx()
        {
            Initialize(GROW_BY);
        }

        public ArrayEx(IEnumerable<T> items)
        {
            Initialize(GROW_BY);
            foreach (T item in items)
            {
                Add(item);
            }
        }

        public ArrayEx(int capacity)
        {
            Initialize(capacity);
        }

        // Methods
        public void Add(T item)
        {
            if (m_data.Length <= m_count)
            {
                Capacity += GROW_BY;
            }
            // We will need to assign the item to the last element and then increment
            // the count variable
            m_data[m_count++] = item;
            ++m_updateCode;
        }

        public void Clear()
        {
            Array.Clear(m_data, 0, m_count);
            m_count = 0;
            ++m_updateCode;
        }

        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < m_count; i++)
            {
                if (comparer.Equals(m_data[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        public int IndexOf(T item)
        {
            return Array.IndexOf<T>(m_data, item, 0, m_count);
        }

        void Initialize(int capacity)
        {
            m_data = new T[capacity];
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= m_count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            if (m_count + 1 >= Capacity)
            {
                Capacity = m_count + GROW_BY;
            }
            // First we need to shift all elements at the location up by one
            for (int i = m_count; i > index && i > 0; --i)
            {
                m_data[i] = m_data[i - 1];
            }
            m_data[index] = item;
            ++m_count;
            ++m_updateCode;
        }

        public bool Remove(T item)
        {
            return Remove(item, false);
        }

        public bool Remove(T item, bool allOccurrences)
        {
            int shiftto = 0;
            bool shiftmode = false;
            bool removed = false;
            int count = m_count;
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < count; ++i)
            {
                if (comparer.Equals(m_data[i], item) && (allOccurrences || !shiftmode))
                {
                    // Decrement the count since we have found an instance
                    --m_count;
                    removed = true;
                    // Check to see if we have already found one occurrence of the
                    // item we are removing
                    if (!shiftmode)
                    {
                        // We will start shifting to the position of the first occurrence.
                        shiftto = i;
                        // Enable shifting
                        shiftmode = true;
                    }
                    continue;
                }
                if (shiftmode)
                {
                    // Since we are shifting elements we need to shift the element
                    // down and then update the shiftto index to the next element.
                    m_data[shiftto++] = m_data[i];
                }
            }
            for (int i = m_count; i < count; ++i)
            {
                m_data[i] = default(T);
            }
            if (removed)
            {
                ++m_updateCode;
            }
            return removed;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= m_count)
            {
                // Item has already been removed.
                return;
            }
            int count = Count;
            // Shift all of the elements after the specified index down one.
            for (int i = index + 1; i < count; ++i)
            {
                m_data[i - 1] = m_data[i];
            }
            // Decrement the count to reflect the item being removed.
            --m_count;
            ++m_updateCode;
            m_data[m_count] = default(T);
        }

        public T[] ToArray()
        {
            T[] tmp = new T[Count];
            for (int i = 0; i < Count; ++i)
            {
                tmp[i] = m_data[i];
            }
            return tmp;
        }

        // Properties
        public int Capacity
        {
            get { return m_data.Length; }
            set
            {
                // We do not support truncating the stored array.
                // So throw an exception if the array is less than Count.
                if (value < Count)
                {
                    throw new ArgumentOutOfRangeException("value", "The value is less than Count");
                }
                // We do not need to do anything if the newly specified capacity
                // is the same as the old one.
                if (value == Capacity)
                {
                    return;
                }
                // We will need to create a new array and move all of the
                // values in the old array to the new one
                T[] tmp = new T[value];
                for (int i = 0; i < Count; ++i)
                {
                    tmp[i] = m_data[i];
                }
                m_data = tmp;
                ++m_updateCode;
            }
        }

        public int Count
        {
            get { return m_count; }
        }

        public bool IsEmpty
        {
            get { return m_count <= 0; }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= m_count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                return m_data[index];
            }
            set
            {
                if (index < 0 || index >= m_count)
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                m_data[index] = value;
                ++m_updateCode;
            }
        }
    }
}