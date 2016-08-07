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

        /// <summary>
        /// Attempt to solve the given nonogram
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <returns>Number of resolved tiles</returns>
        public int Run(Nonogram ng)
        {
            _solved = false;
            Stopwatch sw = Stopwatch.StartNew();
            int resolved = Treesolve(ng, 0, 0);
            sw.Stop();
            _benchTime = sw.Elapsed;
            if (resolved == -1) return 0;
            _solved = true;
            return resolved;
        }

        /// <summary>
        /// Recursive gamtree for nonotgrams
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <param name="row">Row index process</param>
        /// <param name="column">Column index to process</param>
        /// <returns>Number of resolved tiles. -1 if there was a previous error</returns>
        private int Treesolve(Nonogram ng, int row, int column)
        {
            int nColumn = column + 1 < ng.Width ? column + 1 : 0;
            int nRow = nColumn == 0 ? row + 1 : row;
            if (!ng.Resolved(row, column))
            {
                ng.Set(row, column, true);
                if (!Error(ng, row, column))
                {
                    int res = nRow < ng.Height ? Treesolve(ng, nRow, nColumn) : 0;
                    if (res >= 0)
                    {
                        return res + 1;
                    }
                }
                ng.Set(row, column, false);
                if (!Error(ng, row, column))
                {
                    int res = nRow < ng.Height ? Treesolve(ng, nRow, nColumn) : 0;
                    if (res >= 0)
                    {
                        return res + 1;
                    }
                }
                ng.Clear(row, column);
                return -1;
            }
            return nRow < ng.Height ? Treesolve(ng, nRow, nColumn) : 0;
        }

        /// <summary>
        /// Checks nonogram for obvious errors related to specified tile.
        /// </summary>
        /// <param name="ng">Nonogram to check</param>
        /// <param name="row">Row index of tile</param>
        /// <param name="column">Column index of tile</param>
        /// <returns>True if error was found.</returns>
        private bool Error(Nonogram ng, int row, int column)
        {
            int lineSum = ng.RowSum(row);
            for (int i = 0; i < ng.Width; i++)
            {
                if (ng.IsTrue(row, i)) lineSum--;
                if (lineSum < 0) return true;
            }
            lineSum = ng.ColumnSum(column);
            for (int i = 0; i < ng.Height; i++)
            {
                if (ng.IsTrue(i, column)) lineSum--;
                if (lineSum < 0) return true;
            }
            if (column == ng.Width - 1)
            {
                int num = 0;
                List<int> nums = new List<int>();
                for (int i = 0; i < ng.Width; i++)
                {
                    if (ng.IsTrue(row, i))
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
                    if (nums[i] != ng.GetRowNum(row, i)) return true;
                }
                if (ng.GetRowNum(row, nums.Count) != 0) return true;
            }
            if (row == ng.Height - 1)
            {
                int num = 0;
                List<int> nums = new List<int>();
                for (int i = 0; i < ng.Height; i++)
                {
                    if (ng.IsTrue(i, column))
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
                    if (nums[i] != ng.GetColumnNum(column, i)) return true;
                }
                if (ng.GetColumnNum(column, nums.Count) != 0) return true;
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
    }
}
