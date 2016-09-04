using System;
using GameLib;
using Util;

namespace SolverLib
{
    /// <summary>
    /// Solver that attempts to determine the value of a specific tile. And derivates.
    /// </summary>
    public class TrialSolver : ISolver
    {
        private List<Result> _resultList;
        private bool _solved;
        private TimeSpan _benchTime = TimeSpan.Zero;

        /// <summary>
        /// Not in use.
        /// </summary>
        /// <param name="ng">Not used</param>
        /// <returns>Nothing</returns>
        public int Run(Nonogram ng)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to determine the value of specified tile and derivates.
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <param name="row">Row index of tile</param>
        /// <param name="column">Column index of tile</param>
        /// <returns>Number of solved tiles or -1 if a contradiction was encountered</returns>
        public int Run(Nonogram ng, int row, int column)
        {
            _benchTime = TimeSpan.Zero;
            _solved = false;
            _resultList = new List<Result>();
            Nonogram loc = ng.Copy();
            loc.Set(row, column, true);
            // Tries solving with a value of true.
            LineSolver tSolver = new LineSolver();
            bool[] rb = new bool[loc.Height];
            rb[row] = true;
            bool[] cb = new bool[loc.Width];
            cb[column] = true;
            int tRes = tSolver.Run(loc, rb, cb);
            loc.Set(row, column, false);
            // Tries solving with a value of false.
            LineSolver fSolver = new LineSolver();
            rb = new bool[loc.Height];
            rb[row] = true;
            cb = new bool[loc.Width];
            cb[column] = true;
            int fRes = fSolver.Run(loc, rb, cb);
            _benchTime = _benchTime.Add(tSolver.BenchTime()).Add(fSolver.BenchTime());
            // Determines what to return
            if (fRes == -1 || tRes == -1)
            {
                if (fRes == -1 && tRes == -1)
                {
                    return -1;
                }
                if (fRes == -1)
                {
                    _resultList.Add(new Result(row, column, true));
                    AddRes(tSolver.Results());
                    _solved = tSolver.Solved();
                }
                else
                {
                    _resultList.Add(new Result(row, column, false));
                    AddRes(fSolver.Results());
                    _solved = fSolver.Solved();
                }
            }
            return _resultList.Count;
        }

        /// <summary>
        /// Adds results of an other solver to the results of this solver
        /// </summary>
        /// <param name="results">results from other solver</param>
        private void AddRes(List<Result> results)
        {
            while (!results.IsEmpty)
            {
                _resultList.Push(results.Dequeue());
            }
        }

        /// <summary>
        /// Checks wheter the last run resulted in a solution
        /// </summary>
        /// <returns></returns>
        public bool Solved()
        {
            return _solved;
        }

        /// <summary>
        /// Time spent on the last run
        /// </summary>
        /// <returns></returns>
        public TimeSpan BenchTime()
        {
            return _benchTime;
        }

        /// <summary>
        /// Results of the last run
        /// </summary>
        /// <returns></returns>
        public List<Result> Results()
        {
            return _resultList ?? new List<Result>();
        }
    }
}
