using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class TileHeap
    {
        private readonly int[][] _locations;
        private readonly List<PrioCord> _heapList;

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

        public bool IsEmpty => _heapList.IsEmpty;
        public int Count => _heapList.Count;

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

        private PrioCord Parent(int i)
        {
            return _heapList[i / 2];
        }

        public Coordinate Peek()
        {
            return new Coordinate(_heapList[0].Row, _heapList[0].Column);
        }
        
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

        private void Swap(int a, int b)
        {
            PrioCord c = _heapList[a];
            _heapList[a] = _heapList[b];
            _heapList[b] = c;
            _locations[c.Row][c.Column] = b;
            c = _heapList[a];
            _locations[c.Row][c.Column] = a;
        }
        
        public Coordinate Poll()
        {
            Coordinate coord = new Coordinate(_heapList[0].Row, _heapList[0].Column);
            _heapList[0] = _heapList[-1];
            _locations[coord.Row][coord.Column] = -1;
            _heapList.Pop();
            _locations[_heapList[0].Row][_heapList[0].Column] = 0;
            Heapify(0);
            return coord;
        }
    }
}
