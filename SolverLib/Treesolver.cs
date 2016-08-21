using System;
using System.Diagnostics;
using GameLib;
using Util;

namespace SolverLib
{
    /// <summary>
    /// Gametree based nonogram solver class. Scales 2^n.
    /// </summary>
    public class TreeSolver : ISolver
    {
        private bool _solved;
        private TimeSpan _benchTime = TimeSpan.Zero;
        private List<Result> _resultStack = new List<Result>();
        private bool?[][] _grid;
        private Nonogram _ng;

        /// <summary>
        /// Attempt to solve the given nonogram
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <returns>Number of resolved tiles</returns>
        public int Run(Nonogram ng)
        {
            _ng = ng;
            _solved = false;
            _grid = new bool?[ng.Height][];
            for (int i = 0; i < ng.Height; i++)
            {
                _grid[i] = new bool?[ng.Width];
                for (int j = 0; j < ng.Width; j++)
                {
                    if (ng.Resolved(i, j))
                    {
                        _grid[i][j] = ng.IsTrue(i, j);
                    }
                    else
                    {
                        _grid[i][j] = null;
                    }
                }
            }
            Stopwatch sw = Stopwatch.StartNew();
            int resolved = Treesolve(0, 0);
            sw.Stop();
            _benchTime = sw.Elapsed;
            if (resolved == -1) return 0;
            _solved = true;
            for (int i = 0; i < ng.Height; i++)
            {
                for (int j = 0; j < ng.Width; j++)
                {
                    if (!ng.Resolved(i, j) && _grid[i][j].HasValue)
                    {
                        _resultStack.Push(new Result(i, j, _grid[i][j].Value));
                    }
                }
            }
            return resolved;
        }

        /// <summary>
        /// Recursive gamtree for nonotgrams
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <param name="row">Row index process</param>
        /// <param name="column">Column index to process</param>
        /// <returns>Number of resolved tiles. -1 if there was a previous error</returns>
        private int Treesolve(int row, int column)
        {
            int nColumn = column + 1 < _ng.Width ? column + 1 : 0;
            int nRow = nColumn == 0 ? row + 1 : row;
            if (!_grid[row][column].HasValue)
            {
                _grid[row][column] = true;
                if (!Error(row, column))
                {
                    int res = nRow < _ng.Height ? Treesolve(nRow, nColumn) : 0;
                    if (res >= 0)
                    {
                        return res + 1;
                    }
                }
                _grid[row][column] = false;
                if (!Error(row, column))
                {
                    int res = nRow < _ng.Height ? Treesolve(nRow, nColumn) : 0;
                    if (res >= 0)
                    {
                        return res + 1;
                    }
                }
                _grid[row][column] = null;
                return -1;
            }
            return nRow < _ng.Height ? Treesolve(nRow, nColumn) : 0;
        }

        /// <summary>
        /// Checks nonogram for obvious errors related to specified tile.
        /// </summary>
        /// <param name="ng">Nonogram to check</param>
        /// <param name="row">Row index of tile</param>
        /// <param name="column">Column index of tile</param>
        /// <returns>True if error was found.</returns>
        private bool Error(int row, int column)
        {
            int lineSum = _ng.RowSum(row);
            for (int i = 0; i < _ng.Width; i++)
            {
                if (_grid[row][i].HasValue && _grid[row][i].Value) lineSum--;
                if (lineSum < 0) return true;
            }
            lineSum = _ng.ColumnSum(column);
            for (int i = 0; i < _ng.Height; i++)
            {
                if (_grid[i][column].HasValue && _grid[i][column].Value) lineSum--;
                if (lineSum < 0) return true;
            }
            if (column == _ng.Width - 1)
            {
                int num = 0;
                List<int> nums = new List<int>();
                for (int i = 0; i < _ng.Width; i++)
                {
                    if (_grid[row][i].HasValue && _grid[row][i].Value)
                    {
                        num++;
                    }
                    else
                    {
                        if (num != 0) nums.Add(num);
                        num = 0;
                    }
                }
                if (num != 0) nums.Add(num);
                for (int i = 0; i < nums.Count; i++)
                {
                    if (nums[i] != _ng.GetRowNum(row, i)) return true;
                }
                if (_ng.GetRowNum(row, nums.Count) != 0) return true;
            }
            if (row == _ng.Height - 1)
            {
                int num = 0;
                List<int> nums = new List<int>();
                for (int i = 0; i < _ng.Height; i++)
                {
                    if (_grid[i][column].HasValue && _grid[i][column].Value)
                    {
                        num++;
                    }
                    else
                    {
                        if (num != 0) nums.Add(num);
                        num = 0;
                    }
                }
                if (num != 0) nums.Add(num);
                for (int i = 0; i < nums.Count; i++)
                {
                    if (nums[i] != _ng.GetColumnNum(column, i)) return true;
                }
                if (_ng.GetColumnNum(column, nums.Count) != 0) return true;
            }
            return false;
        }

        /// <summary>
        /// Checks wether a nonogram was solved
        /// </summary>
        /// <returns>True if the solver has been run and the last run resulted in a solved nonogram</returns>
        public bool Solved()
        {
            return _solved;
        }

        /// <summary>
        /// Time spent on number crunching.
        /// </summary>
        /// <returns>Timespan representing the time spent on last Run method</returns>
        public TimeSpan BenchTime()
        {
            return _benchTime;
        }

        /// <summary>
        /// Returns a list of tiles resolved by the solver.
        /// </summary>
        /// <returns>List of resolved tile data.</returns>
        public List<Result> Results()
        {
            return _resultStack;
        }
    }
}
