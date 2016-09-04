using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using GameLib;
using Util;

namespace SolverLib
{
    public class TrialSolver : ISolver
    {
        private List<Result> _resultList;
        private bool _solved;
        private TimeSpan _benchTime = TimeSpan.Zero;

        public int Run(Nonogram ng)
        {
            return 0;
        }

        public int Run(Nonogram ng, int row, int column)
        {
            _benchTime = TimeSpan.Zero;
            _solved = false;
            _resultList = new List<Result>();
            Nonogram loc = ng.Copy();
            loc.Set(row, column, true);
            LineSolver tSolver = new LineSolver();
            bool[] rb = new bool[loc.Height];
            rb[row] = true;
            bool[] cb = new bool[loc.Width];
            cb[column] = true;
            int tRes = tSolver.Run(loc, rb, cb);
            loc.Set(row, column, false);
            LineSolver fSolver = new LineSolver();
            rb = new bool[loc.Height];
            rb[row] = true;
            cb = new bool[loc.Width];
            cb[column] = true;
            int fRes = fSolver.Run(loc, rb, cb);
            _benchTime = _benchTime.Add(tSolver.BenchTime()).Add(fSolver.BenchTime());
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
            Console.WriteLine("trialsolver returns: " + _resultList.Count);
            return _resultList.Count;
        }

        private void AddRes(List<Result> results)
        {
            while (!results.IsEmpty)
            {
                _resultList.Push(results.Dequeue());
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
            return _resultList ?? new List<Result>();
        }
    }
}
