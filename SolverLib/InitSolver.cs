using System;
using System.Diagnostics;
using System.Linq;
using GameLib;
using Util;

namespace SolverLib
{
    /// <summary>
    /// calculates simple initial positions for tiles in an empty nonogram. Depreceated
    /// </summary>
    public class InitSolver : ISolver
    {
        private bool _solved;
        private TimeSpan _time = TimeSpan.Zero;
        private List<Result>  _results = new List<Result>();
        private bool?[][] _marked;
        private Nonogram _ng;

        /// <summary>
        /// Runs the InitSolver on the specified nonogram
        /// </summary>
        /// <param name="ng">Nonogram to work on</param>
        /// <returns>Number of solved tiles or -1 if an error is detected</returns>
        public int Run(Nonogram ng)
        {
            _ng = ng;
            _results = new List<Result>();
            _solved = false;
            _marked = new bool?[ng.Height][];
            for (int i = 0; i < ng.Height; i++)
            {
                _marked[i] = new bool?[ng.Width];
            }
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < ng.Height; i++)
            {
                if (!InitRow(i))
                {
                    _results = new List<Result>();
                    return -1;
                }
            }
            for (int i = 0; i < ng.Width; i++)
            {
                if (!InitColumn(i))
                {
                    _results = new List<Result>();
                    return -1;
                }
            }
            _solved = _marked.All(x => x.All(y => y.HasValue));
            sw.Stop();
            _time = sw.Elapsed;
            return _results.Count;
        }

        /// <summary>
        /// Attempt to generate results for a specific row
        /// </summary>
        /// <param name="row">Row index</param>
        /// <returns>False if an error was found. else true</returns>
        private bool InitRow(int row)
        {
            int[] rowArr = _ng.GetRowArray(row);
            if (_ng.Width - (rowArr.Sum() + rowArr.Length - 1) == 0)
            {
                return Fillrow(row, rowArr);
            }
            int[] rowInts = new int[_ng.Width];
            int idx = 0;
            for (int i = 0; i < rowArr.Length; i++)
            {
                for (int j = 0; j < rowArr[i]; j++)
                {
                    rowInts[idx] = i + 1;
                    idx++;
                }
                idx++;
            }
            idx = _ng.Width - 1;
            for (int i = rowArr.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < rowArr[i]; j++)
                {
                    if (rowInts[idx] == i + 1)
                    {
                        _results.Add(new Result(row, idx, true));
                        _marked[row][i] = true;
                    }
                    idx--;
                }
                idx--;
            }
            return true;
        }

        /// <summary>
        /// Completely fills a row that can be completely filled
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="rowArr">clue array for the row</param>
        /// <returns>False if an error was found. Else true</returns>
        private bool Fillrow(int row, int[] rowArr)
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
                if (idx >= _marked[0].Length) return true;
                _results.Add(new Result(row, idx, false));
                _marked[row][idx] = false;
                idx++;
            }
            return true;
        }

        /// <summary>
        /// Attempts to generate results for a specific column
        /// </summary>
        /// <param name="column">column index</param>
        /// <returns>False if an error was found. Else true</returns>
        private bool InitColumn(int column)
        {
            int[] colArr = _ng.GetColumnArray(column);
            if (_ng.Height - (colArr.Sum() + colArr.Length - 1) == 0)
            {
                return FullColumn(column, colArr);
            }
            int[] colInts = new int[_ng.Height];
            int idx = 0;
            for (int i = 0; i < colArr.Length; i++)
            {
                for (int j = 0; j < colArr[i]; j++)
                {
                    colInts[idx] = i + 1;
                    idx++;
                }
                idx++;
            }
            idx = _ng.Height - 1;
            for (int i = colArr.Length - 1; i >= 0; i--)
            {
                for (int j = 0; j < colArr[i]; j++)
                {
                    if (colInts[idx] == i + 1 && !_marked[idx][column].HasValue)
                    {
                        _results.Add(new Result(idx, column, true));
                    }
                    else if (_marked[idx][column].HasValue && !_marked[idx][column].Value)
                    {
                        return false;
                    }
                    idx--;
                }
                idx--;
            }
            return true;
        }

        /// <summary>
        /// Attempts to completely fill a row that can be completely filled
        /// </summary>
        /// <param name="column">Column index</param>
        /// <param name="colArr">Column clues</param>
        /// <returns>False if an error was found. Else true</returns>
        private bool FullColumn(int column, int[] colArr)
        {
            int idx = 0;
            foreach (int i in colArr)
            {
                for (int j = 0; j < i; j++)
                {
                    if (!_marked[idx][column].HasValue)
                    {
                        _results.Add(new Result(idx, column, true));
                    }
                    else if (!_marked[idx][column].Value)
                    {
                        return false;
                    }
                    idx++;
                }
                if (idx >= _marked.Length) return true;
                if (_marked[idx][column].HasValue && _marked[idx][column].Value)
                {
                    return false;
                }
                _results.Add(new Result(idx, column, false));
                idx++;
            }
            // This statement should never be reached since 
            // a full column always ends with a black tile.
            return true;
        }

        /// <summary>
        /// True if the nonogram was completely solved on the last run
        /// </summary>
        /// <returns>True if solved</returns>
        public bool Solved()
        {
            return _solved;
        }

        /// <summary>
        /// Time spent on the previous run
        /// </summary>
        /// <returns></returns>
        public TimeSpan BenchTime()
        {
            return _time;
        }

        /// <summary>
        /// List of results for the last run
        /// </summary>
        /// <returns>List of results</returns>
        public List<Result> Results()
        {
            return _results;
        }
    }
}
