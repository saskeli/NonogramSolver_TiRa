using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using Util;

namespace SolverLib
{
    class InitSolver : ISolver
    {
        private bool _solved;
        private TimeSpan _time = TimeSpan.Zero;
        private List<Result>  _results = new List<Result>();
        private bool[][] _marked;
        private Nonogram _ng = null;

        public int Run(Nonogram ng)
        {
            _ng = ng;
            _results = new List<Result>();
            _solved = false;
            _marked = new bool[ng.Height][];
            for (int i = 0; i < ng.Height; i++)
            {
                _marked[i] = new bool[ng.Width];
            }
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < ng.Height; i++)
            {
                InitRow(i);
            }
            for (int i = 0; i < ng.Width; i++)
            {
                InitColumn(i);
            }
            _solved = _marked.All(x => x.All(y => y));
            sw.Stop();
            _time = sw.Elapsed;
            return _results.Count;
        }

        private void InitRow(int row)
        {
            int[] rowArr = _ng.GetRowArray(row);
            if (_ng.Width - (rowArr.Sum() + rowArr.Length - 1) == 0)
            {
                Fillrow(row, rowArr);
                return;
            }
            int[] rowInts = new int[_ng.Width];
            int idx = 0;
            for (int i = 0; i < rowArr.Length; i++)
            {
                for (int j = 0; j < rowArr[i]; j++)
                {
                    rowInts[idx] = i;
                    idx++;
                }
                idx++;
            }
            idx = _ng.Width - 1;
            for (int i = rowArr.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < rowArr[i]; j++)
                {
                    if (rowInts[idx] == i)
                    {
                        _results.Add(new Result(row, idx, true));
                        _marked[row][i] = true;
                    }
                    idx--;
                }
                idx--;
            }
        }

        private void Fillrow(int row, int[] rowArr)
        {
            int idx = 0;
            foreach (int i in rowArr)
            {
                for (int j = 0; j < i; j++)
                {
                    _results.Add(new Result(row, idx, true));
                    _marked[row][idx] = true;
                    idx++;
                }
                _results.Add(new Result(row, idx, false));
                _marked[row][idx] = true;
                idx++;
            }
        }

        private void InitColumn(int column)
        {
            int[] colArr = _ng.GetColumnArray(column);
            if (_ng.Height - (colArr.Sum() + colArr.Length - 1) == 0)
            {
                FullColumn(column, colArr);
                return;
            }
            int[] colInts = new int[_ng.Height];
            int idx = 0;
            for (int i = 0; i < colArr.Length; i++)
            {
                for (int j = 0; j < colArr[i]; j++)
                {
                    colInts[idx] = i;
                    idx++;
                }
                idx++;
            }
            idx = _ng.Height - 1;
            for (int i = colArr.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < colArr[i]; j++)
                {
                    if (colArr[idx] == i && !_marked[idx][column])
                    {
                        _results.Add(new Result(idx, column, true));
                    }
                    idx--;
                }
                idx--;
            }
        }

        private void FullColumn(int column, int[] colArr)
        {
            int idx = 0;
            foreach (int i in colArr)
            {
                for (int j = 0; j < i; j++)
                {
                    if (!_marked[idx][column])
                    {
                        _results.Add(new Result(idx, column, true));
                    }
                    idx++;
                }
                _results.Add(new Result(idx, column, false));
                idx++;
            }
        }

        public bool Solved()
        {
            return _solved;
        }

        public TimeSpan BenchTime()
        {
            return _time;
        }

        public Util.List<Result> Results()
        {
            return _results;
        }
    }
}
