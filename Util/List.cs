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
        private int _nexIndex = 0;

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
            if (_nexIndex >= 0X7FEFFFFF) throw new OverflowException("Maximum collection size exeeded");
            if (_nexIndex >= _data.Length) Resize();
            _data[_nexIndex] = t;
            _nexIndex++;
        }

        /// <summary>
        /// Doubles the data capacity of the list or sets maximum allowable capacity.
        /// </summary>
        private void Resize()
        {
            // 0X7FEFFFFF is an internal array size limitation in c#.
            int newSize = 
                2 * (uint)_data.Length > 0X7FEFFFFF ? 0X7FEFFFFF : _data.Length * 2;
            newSize = newSize > 0 ? newSize : 1;
            T[] newData = new T[newSize];
            for (int i = 0; i < _nexIndex; i++)
            {
                newData[i] = _data[i];
            }
            _data = newData;
        }

        /// <summary>
        /// Generate new array from elements in List.
        /// </summary>
        /// <returns>Array of type T</returns>
        public T[] ToArray()
        {
            T[] returnable = new T[_nexIndex];
            for (int i = 0; i < _nexIndex; i++)
            {
                returnable[i] = _data[i];
            }
            return returnable;
        }

        /// <summary>
        /// NUber of elements in the List.
        /// </summary>
        public int Count
        {
            get { return _nexIndex; }
        }
    }
}
