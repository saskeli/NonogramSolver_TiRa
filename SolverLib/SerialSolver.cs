using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using Util;
using System.Windows;

namespace SolverLib
{
    public class SerialSolver : ISolver
    {
        private bool _solved;
        private TimeSpan _benchTime = TimeSpan.Zero;
        private List<Result> _results = new List<Result>();
        private Nonogram _ng = null;
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

        public SerialSolver()
        {
            _smalltree = true;
        }

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

        public bool Solved()
        {
            return _solved;
        }

        public TimeSpan BenchTime()
        {
            return _benchTime;
        }

        public List<Result> Results()
        {
            return _results;
        }
    }
}
