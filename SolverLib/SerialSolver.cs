using System;
using System.Diagnostics;
using GameLib;
using Util;

namespace SolverLib
{
    /// <summary>
    /// Solver that uses LineSolver, Trialsolver and Treesolver to efficiently solve nonograms
    /// </summary>
    public class SerialSolver : ISolver
    {
        private bool _solved;
        private TimeSpan _benchTime = TimeSpan.Zero;
        private List<Result> _results = new List<Result>();
        private Nonogram _ng;
        private readonly bool _smalltree;
        private TileHeap _th;

        /// <summary>
        /// Serialsolver constructor with option to skip treesolving for big unknowns.
        /// </summary>
        /// <param name="smallTree">if true, treesolving will only be done when less than 200 unknowns exist.</param>
        public SerialSolver(bool smallTree)
        {
            _smalltree = smallTree;
        }

        /// <summary>
        /// SerialSolver constructor that skips treesolving for big unknowns.
        /// </summary>
        public SerialSolver()
        {
            _smalltree = true;
        }

        /// <summary>
        /// Runs the SerialSolver
        /// </summary>
        /// <param name="ng">Nonogram to solve</param>
        /// <returns>Number of solver tiles or -1 if a contradiction was encountered</returns>
        public int Run(Nonogram ng)
        {
            _ng = ng.Copy();
            _benchTime = TimeSpan.Zero;
            _results = new List<Result>();
            LineSolver ls = new LineSolver();
            if (ls.Run(_ng) > 0)
            {
                if (ls.Solved())
                {
                    _results = ls.Results();
                    _benchTime = ls.BenchTime();
                    _solved = true;
                    return _results.Count;
                }
            }
            _benchTime = ls.BenchTime();
            Stopwatch sw = Stopwatch.StartNew();
            _th = new TileHeap(_ng.Width, ng.Height);
            Update(ls.Results());
            for (int i = 0; i < _ng.Width; i++)
            {
                _th.Add(0, i, _ng.GetPrio(0, i));
                _th.Add(_ng.Height - 1, i, _ng.GetPrio(_ng.Height - 1, i));
            }
            for (int i = 1; i < _ng.Height - 1; i++)
            {
                _th.Add(i, 0, _ng.GetPrio(i, 0));
                _th.Add(i, _ng.Width - 1, _ng.GetPrio(i, _ng.Width - 1));
            }
            _benchTime = _benchTime.Add(sw.Elapsed);
            TrialSolver trS = new TrialSolver();
            while (!_th.IsEmpty)
            {
                Coordinate c = _th.Poll();
                if (!_ng.Resolved(c.Row, c.Column))
                {
                    int res = trS.Run(_ng, c.Row, c.Column);
                    if (res == -1)
                    {
                        return _results.Count;
                    }
                    _benchTime = _benchTime.Add(trS.BenchTime());
                    sw = Stopwatch.StartNew();
                    Update(trS.Results());
                    _benchTime = _benchTime.Add(sw.Elapsed);
                    if (_ng.LeftToClear == 0)
                    {
                        _solved = true;
                        return _results.Count;
                    }
                }
            }
            if (_ng.LeftToClear < 201 || !_smalltree)
            {
                ISolver ts = new TreeSolver();
                ts.Run(_ng);
                _benchTime = _benchTime.Add(ts.BenchTime());
                Update(ts.Results());
                _solved = true;
            }
            return _results.Count;
        }

        /// <summary>
        /// Updates this solver with results from another solver
        /// </summary>
        /// <param name="resQueue">results from another solver</param>
        private void Update(List<Result> resQueue)
        {
            while (!resQueue.IsEmpty)
            {
                Result res = resQueue.Dequeue();
                _ng.Set(res.Row, res.Column, res.State);
                UpdatePrioHeap(res.Row, res.Column);
                _results.Push(res);
            }
        }

        /// <summary>
        /// Updates (adds 1 to) priority of specified tile
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="column">column index</param>
        private void UpdatePrioHeap(int row, int column)
        {
            for (int i = row - 1; i < row + 2; i++)
            {
                for (int j = column - 1; j < column + 2; j++)
                {
                    if (_ng.AddPrio(i, j))
                    {
                        _th.Add(i, j, _ng.GetPrio(i, j));
                    }
                }
            }
        }

        /// <summary>
        /// True if the last run resulted in a solution
        /// </summary>
        /// <returns>true if solved</returns>
        public bool Solved()
        {
            return _solved;
        }

        /// <summary>
        /// Time spent on the last run of the solver
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
            return _results;
        }
    }
}
