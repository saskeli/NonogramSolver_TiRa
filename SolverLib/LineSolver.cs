using System;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using Util;

namespace SolverLib
{
    public class LineSolver : ISolver
    {
        private Nonogram _ng;
        private List<Result> _resultList;
        private bool _solved;
        private TimeSpan _benchSpan = TimeSpan.Zero;
        private bool[] _rowChanged;
        private bool[] _colChanged;

        public int Run(Nonogram ng)
        {
            _ng = ng.Copy();
            _resultList = new List<Result>();
            _solved = false;
            _benchSpan = TimeSpan.Zero;
            _rowChanged = new bool[_ng.Height];
            _colChanged = new bool[_ng.Width];
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < _ng.Height; i++)
            {
                if (!FillRow(i))
                {
                    _benchSpan = sw.Elapsed;
                    _resultList = new List<Result>();
                    return -1;
                }
            }
            for (int i = 0; i < _ng.Width; i++)
            {
                if (!FillColumn(i))
                {
                    _benchSpan = sw.Elapsed;
                    _resultList = new List<Result>();
                    return -1;
                }
            }
            while (_rowChanged.Any(x => x) || _colChanged.Any(x => x))
            {
                for (int i = 0; i < _ng.Height; i++)
                {
                    if (_rowChanged[i])
                    {
                        _rowChanged[i] = false;
                        if (!FillRow(i))
                        {
                            _benchSpan = sw.Elapsed;
                            _resultList = new List<Result>();
                            return -1;
                        }
                    }
                }
                for (int i = 0; i < _ng.Width; i++)
                {
                    if (_colChanged[i])
                    {
                        _colChanged[i] = false;
                        if (!FillColumn(i))
                        {
                            _benchSpan = sw.Elapsed;
                            _resultList = new List<Result>();
                            return -1;
                        }
                    }
                }
            }
            _solved = _ng.LeftToClear == 0;
            _benchSpan = sw.Elapsed;
            return _resultList.Count;
        }

        private bool FillColumn(int index)
        {
            bool solved = true;
            for (int i = 0; i < _ng.Height; i++)
            {
                if (!_ng.Resolved(i, index)) solved = false;
            }
            if (solved) return true;
            int[] leftInts = new int[_ng.Height];
            if (!ColLeft(index, leftInts, 0, 0))
            {
                return false;
            }
            int[] rightInts = new int[_ng.Height];
            ColRight(index, rightInts, _ng.GetColumnArray(index).Length - 1, _ng.Height - 1);
            for (int i = 0; i < leftInts.Length; i++)
            {
                if (leftInts[i] == rightInts[i])
                {
                    bool state = leftInts[i] % 2 == 0;
                    _rowChanged[i] = true;
                    _resultList.Add(new Result(i, index, state));
                    _ng.Set(i, index, state);
                }
            }
            return true;
        }

        private bool ColLeft(int column, int[] leftInts, int clueIndex, int colPos)
        {
            throw new NotImplementedException();
        }

        private bool ColRight(int column, int[] rightInts, int clueIndex, int colPos)
        {
            throw new NotImplementedException();
        }

        private bool FillRow(int index)
        {
            bool solved = true;
            for (int i = 0; i < _ng.Width; i++)
            {
                if (!_ng.Resolved(index, i)) solved = false;
            }
            if (solved) return true;
            int[] leftInts = new int[_ng.Width];
            if (!RowLeft(index, leftInts, 0, 0))
            {
                return false;
            }
            int[] rightInts = new int[_ng.Width];
            RowRight(index, rightInts, _ng.GetRowArray(index).Length - 1, _ng.Width - 1);
            for (int i = 0; i < leftInts.Length; i++)
            {
                if (leftInts[i] == rightInts[i])
                {
                    bool state = leftInts[i] % 2 == 0;
                    _colChanged[i] = true;
                    _resultList.Add(new Result(index, i, state));
                    _ng.Set(index, i, state);
                }
            }
            return true;
        }

        private bool RowLeft(int row, int[] leftInts, int clueIndex, int rowPos)
        {
            throw new NotImplementedException();
        }

        private bool RowRight(int row, int[] rightInts, int clueIndex, int rowPos)
        {
            throw new NotImplementedException();
        }

        public bool Solved()
        {
            return _solved;
        }

        public TimeSpan BenchTime()
        {
            return _benchSpan;
        }

        public Util.List<Result> Results()
        {
            return _resultList ?? new List<Result>();
        }
    }
}
