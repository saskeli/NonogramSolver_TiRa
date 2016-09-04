namespace Util
{
    /// <summary>
    /// Simple heap implementation for fast handling of tile priorities
    /// </summary>
    public class TileHeap
    {
        private readonly int[][] _locations;
        private readonly List<PrioCord> _heapList;

        /// <summary>
        /// Heap constructor. Initializes underlying structure based on Nonogram size.
        /// </summary>
        /// <param name="width">Width of grid (Nonogram)</param>
        /// <param name="height">Height of grid (Nonogram)</param>
        public TileHeap(int width, int height)
        {
            _locations = new int[height][];
            for (int i = 0; i < height; i++)
            {
                _locations[i] = new int[width];
                for (int j = 0; j < width; j++)
                {
                    _locations[i][j] = -1;
                }
            }
            _heapList = new List<PrioCord>(2 * (width + height));
        }

        /// <summary>
        /// True if the heap is empty
        /// </summary>
        public bool IsEmpty => _heapList.IsEmpty;

        /// <summary>
        /// Number of elements in heap
        /// </summary>
        public int Count => _heapList.Count;

        /// <summary>
        /// Adds an element to the heap. Or updates the elements priority and 
        /// position if it is already present
        /// </summary>
        /// <param name="row">row index of element</param>
        /// <param name="column">column index of element</param>
        /// <param name="priority">priority of element</param>
        public void Add(int row, int column, int priority)
        {
            if (_locations[row][column] == -1)
            {
                _locations[row][column] = _heapList.Count;
                _heapList.Add(new PrioCord(row, column, priority));
            }
            else
            {
                _heapList[_locations[row][column]].Priority = priority;
            }
            Float(_locations[row][column]);
        }

        /// <summary>
        /// Raises the specified element to it's place
        /// </summary>
        /// <param name="i">index of element</param>
        private void Float(int i)
        {
            PrioCord pk = _heapList[i];
            while (i > 0 && Parent(i).Priority < pk.Priority)
            {
                _heapList[i] = Parent(i);
                _locations[_heapList[i].Row][_heapList[i].Column] = i;
                i = i / 2;
            }
            _heapList[i] = pk;
            _locations[pk.Row][pk.Column] = i;
        }

        /// <summary>
        /// Gets the parent coordinate of a specified index
        /// </summary>
        /// <param name="i">index of element to find parent of</param>
        /// <returns>Parent coordinate</returns>
        private PrioCord Parent(int i)
        {
            return _heapList[i / 2];
        }

        /// <summary>
        /// Returns the element at the top of the heap without consuming it.
        /// </summary>
        /// <returns>Coordinate at the top of the heap</returns>
        public Coordinate Peek()
        {
            return new Coordinate(_heapList[0].Row, _heapList[0].Column);
        }
        
        /// <summary>
        /// Moves the specified element downward to it's correct position.
        /// </summary>
        /// <param name="i">index of element to move</param>
        private void Heapify(int i)
        {
            while (true)
            {
                int r = i * 2 + 2;
                int l = i * 2 + 1;
                if (r < _heapList.Count && _heapList[r].Priority > _heapList[i].Priority)
                {
                    if (_heapList[r].Priority < _heapList[l].Priority)
                    {
                        Swap(i, l);
                        i = l;
                    }
                    else
                    {
                        Swap(i, r);
                        i = r;
                    }
                }
                else if (l < _heapList.Count && _heapList[l].Priority > _heapList[i].Priority)
                {
                    Swap(i, l);
                    break;
                }
                else break;
            }
        }

        /// <summary>
        /// Swaps 2 elements in the heap
        /// </summary>
        /// <param name="a">index of first element</param>
        /// <param name="b">index of second element</param>
        private void Swap(int a, int b)
        {
            PrioCord c = _heapList[a];
            _heapList[a] = _heapList[b];
            _heapList[b] = c;
            _locations[c.Row][c.Column] = b;
            c = _heapList[a];
            _locations[c.Row][c.Column] = a;
        }
        
        /// <summary>
        /// Returns and consumes the element at the top of the heap.
        /// </summary>
        /// <returns>Coordinate with the highest priority</returns>
        public Coordinate Poll()
        {
            Coordinate coord = new Coordinate(_heapList[0].Row, _heapList[0].Column);
            _heapList[0] = _heapList[-1];
            _locations[coord.Row][coord.Column] = -1;
            _heapList.Pop();
            if (!IsEmpty)
            {
                _locations[_heapList[0].Row][_heapList[0].Column] = 0;
                Heapify(0);
            }
            return coord;
        }
    }
}
