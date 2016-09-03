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
                if (!_ng.Resolved(i, index))
                {
                    solved = false;
                    break;
                }
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
                if (leftInts[i] == rightInts[i] && !_ng.Resolved(i, index))
                {
                    bool state = leftInts[i] % 2 == 0;
                    _rowChanged[i] = true;
                    _colChanged[index] = true;
                    _resultList.Add(new Result(i, index, state));
                    _ng.Set(i, index, state);
                }
            }
            return true;
        }

        private bool ColLeft(int column, int[] leftInts, int clueIndex, int colPos)
        {
            int clue = _ng.GetColumnNum(column, clueIndex);
            if (clue == 0)
            {
                for (int i = colPos; i < _ng.Height; i++)
                {
                    if (_ng.IsTrue(i, column)) return false;
                    leftInts[i] = clueIndex * 2 - 1;
                }
                return true;
            }
            int localPos = colPos;
            bool posChanged;
            while (true)
            {
                posChanged = false;
                for (int i = localPos; i < localPos + clue; i++)
                {
                    if (i >= leftInts.Length) return false;
                    if (_ng.IsFalse(i, column))
                    {
                        for (int j = localPos; j <= i; j++)
                        {
                            if (_ng.IsTrue(j, column)) return false;
                            leftInts[j] = clueIndex * 2 - 1;
                        }
                        localPos = i + 1;
                        posChanged = true;
                        break;
                    }
                    leftInts[i] = clueIndex * 2;
                }
                if (posChanged) continue;
                if (localPos + clue >= leftInts.Length) return true;
                if (ColLeft(column, leftInts, clueIndex + 1, localPos + clue))
                {
                    return true;
                }
                if (_ng.IsTrue(localPos, column))
                {
                    return false;
                }
                leftInts[localPos] = clueIndex * 2 - 1;
                localPos++;
            }
        }

        private bool ColRight(int column, int[] rightInts, int clueIndex, int colPos)
        {
            int clue = _ng.GetColumnNum(column, clueIndex);
            if (clue == 0)
            {
                for (int i = colPos; i >= 0; i--)
                {
                    if (_ng.IsTrue(i, column)) return false;
                    rightInts[i] = -1;
                }
                return true;
            }
            int localPos = colPos;
            bool posChanged;
            while (true)
            {
                posChanged = false;
                for (int i = localPos; i > localPos - clue; i--)
                {
                    if (i < 0) return false;
                    if (_ng.IsFalse(i, column))
                    {
                        for (int j = localPos; j >= i; j--)
                        {
                            if (_ng.IsTrue(j, column)) return false;
                            rightInts[j] = clueIndex * 2 - 1;
                        }
                        localPos = i - 1;
                        posChanged = true;
                        break;
                    }
                    rightInts[i] = clueIndex * 2;
                }
                if (posChanged) continue;
                if (localPos - clue < 0) return true;
                if (ColRight(column, rightInts, clueIndex - 1, localPos - clue))
                {
                    return true;
                }
                if (_ng.IsTrue(localPos, column))
                {
                    return false;
                }
                rightInts[localPos] = clueIndex * 2 + 1;
                localPos--;
            }
        }

        private bool FillRow(int index)
        {
            bool solved = true;
            for (int i = 0; i < _ng.Width; i++)
            {
                if (!_ng.Resolved(index, i))
                {
                    solved = false;
                    break;
                }
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
                if (leftInts[i] == rightInts[i] && !_ng.Resolved(index, i))
                {
                    bool state = leftInts[i] % 2 == 0;
                    _colChanged[i] = true;
                    _rowChanged[index] = true;
                    _resultList.Add(new Result(index, i, state));
                    _ng.Set(index, i, state);
                }
            }
            return true;
        }

        private bool RowLeft(int row, int[] leftInts, int clueIndex, int rowPos)
        {
            int clue = _ng.GetRowNum(row, clueIndex);
            if (clue == 0)
            {
                for (int i = rowPos; i < leftInts.Length; i++)
                {
                    if (_ng.IsTrue(row, i)) return false;
                    leftInts[i] = clueIndex * 2 - 1;
                }
                return true;
            }
            int localPos = rowPos;
            bool posChanged;
            while (true)
            {
                posChanged = false;
                for (int i = localPos; i < localPos + clue; i++)
                {
                    if (i >= leftInts.Length) return false;
                    if (_ng.IsFalse(row, i))
                    {
                        for (int j = localPos; j <= i; j++)
                        {
                            if (_ng.IsTrue(row, j)) return false;
                            leftInts[j] = clueIndex * 2 - 1;
                        }
                        localPos = i + 1;
                        posChanged = true;
                        break;
                    }
                    leftInts[i] = clueIndex * 2;
                }
                if (posChanged) continue;
                if (localPos + clue >= leftInts.Length) return true;
                if (RowLeft(row, leftInts, clueIndex + 1, localPos + clue))
                {
                    return true;
                }
                if (_ng.IsTrue(row, localPos))
                {
                    return false;
                }
                leftInts[localPos] = clueIndex * 2 - 1;
                localPos++;
            }
        }

        private bool RowRight(int row, int[] rightInts, int clueIndex, int rowPos)
        {
            int clue = _ng.GetRowNum(row, clueIndex);
            if (clue == 0)
            {
                for (int i = rowPos; i >= 0; i--)
                {
                    if (_ng.IsTrue(row, i)) return false;
                    rightInts[i] = -1;
                }
                return true;
            }
            int localPos = rowPos;
            bool posChanged = false;
            while (true)
            {
                posChanged = false;
                for (int i = localPos; i > localPos - clue; i--)
                {
                    if (i < 0) return false;
                    if (_ng.IsFalse(row, i))
                    {
                        for (int j = localPos; j >= i; j--)
                        {
                            if (_ng.IsTrue(row, j)) return false;
                            rightInts[j] = clueIndex * 2 - 1;
                        }
                        localPos = i - 1;
                        posChanged = true;
                        break;
                    }
                    rightInts[i] = clueIndex * 2;
                }
                if (posChanged) continue;
                if (localPos - clue < 0) return true;
                if (RowRight(row, rightInts, clueIndex - 1, localPos - clue))
                {
                    return true;
                }
                if (_ng.IsTrue(row, localPos))
                {
                    return false;
                }
                rightInts[localPos] = clueIndex * 2 + 1;
                localPos--;
            }
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
