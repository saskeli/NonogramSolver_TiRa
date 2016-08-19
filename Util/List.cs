using System;

namespace Util
{
    /// <summary>
    /// Generic very simple dynamic collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class List<T>
    {
        private T[] _data;
        private int _nextIndex;
        private int _startIndex;

        /// <summary>
        /// Create a new List object with a default initial capacity.
        /// </summary>
        public List()
        {
            _data = new T[4];
        }

        /// <summary>
        /// Create a new List object with a specified capacity.
        /// </summary>
        /// <param name="init">Desired initial capacity.</param>
        public List(int init)
        {
            if (init < 0) throw  new ArgumentOutOfRangeException(nameof(init), 
                    "Object can not be initialized with negative value");
            _data = new T[init];
        }

        /// <summary>
        /// Appends the specified element to the collection. 
        /// List size is increased dynamically as needed.
        /// </summary>
        /// <param name="t">Element to append to collection.</param>
        public void Add(T t)
        {
            // 0X7FEFFFFF is an internal array size limitation in c#.
            if (_nextIndex >= 0X7FEFFFFF) throw new OverflowException("Maximum collection size exeeded");
            if (_nextIndex >= _data.Length) Resize();
            _data[_nextIndex] = t;
            _nextIndex++;
        }

        /// <summary>
        /// Adds the specified element to the top of the stack.
        /// </summary>
        /// <param name="t">Elment to add</param>
        public void Push(T t)
        {
            Add(t);
        }

        /// <summary>
        /// Adds the specified element to the back of the Queue.
        /// </summary>
        /// <param name="t">Element to add</param>
        public void Enqueue(T t)
        {
            Add(t);
        }

        /// <summary>
        /// Returns and removes the element at the top of the stack
        /// </summary>
        /// <returns>Element at the top of the stack</returns>
        public T Pop()
        {
            T t = this[-1];
            _nextIndex--;
            return t;
        }

        /// <summary>
        /// Returns and removes the element at the front of the queue
        /// </summary>
        /// <returns>Element at the front of the queue</returns>
        public T Dequeue()
        {
            T t = this[0];
            _startIndex++;
            return t;
        }

        /// <summary>
        /// Gets element at the top of the stack
        /// </summary>
        /// <returns>Element at the top of the stack</returns>
        public T Peek()
        {
            return this[-1];
        }

        /// <summary>
        /// Gets element at the front of the queue
        /// </summary>
        /// <returns>Element at the front of the queue</returns>
        public T Poll()
        {
            return this[0];
        }

        /// <summary>
        /// Returns true if the collection is empty
        /// </summary>
        public bool IsEmpty => _startIndex == _nextIndex;

        /// <summary>
        /// Doubles the data capacity of the list or sets maximum allowable capacity.
        /// </summary>
        private void Resize()
        {
            if (_nextIndex - _startIndex <= _data.Length / 2 && _data.Length >= 4)
            {
                for (int i = 0; i < _nextIndex - _startIndex; i++)
                {
                    _data[i] = _data[_startIndex + i];
                }
            }
            else
            {
                // 0X7FEFFFFF is an internal array size limitation in c#.
                int newSize =
                    2 * (uint)_data.Length > 0X7FEFFFFF ? 0X7FEFFFFF : _data.Length * 2;
                newSize = newSize > 0 ? newSize : 1;
                T[] newData = new T[newSize];
                for (int i = 0; i < _nextIndex - _startIndex; i++)
                {
                    newData[i] = _data[_startIndex + i];
                }
                _data = newData;
            }
            _nextIndex -= _startIndex;
            _startIndex = 0;
        }

        /// <summary>
        /// Generate new array from elements in List.
        /// </summary>
        /// <returns>Array of type T</returns>
        public T[] ToArray()
        {
            T[] returnable = new T[_nextIndex - _startIndex];
            for (int i = _startIndex; i < _nextIndex; i++)
            {
                returnable[i] = _data[i];
            }
            return returnable;
        }

        /// <summary>
        /// Get specified index in list
        /// </summary>
        /// <param name="index">Index of element</param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                if (index < -Count || index >= _nextIndex - _startIndex) throw new ArgumentOutOfRangeException();
                return index < 0 ? _data[_nextIndex + index] : _data[index + _startIndex];
            }
        }

        /// <summary>
        /// Number of elements in the List.
        /// </summary>
        public int Count => _nextIndex - _startIndex;
    }
}
