using System;
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

        public int Run(Nonogram ng)
        {
            _ng = ng.Copy();
            _benchTime = TimeSpan.Zero;
            _results = new List<Result>();
            ISolver s = new InitSolver();
            if (s.Run(_ng) > 0)
            {
                _solved = s.Solved();
                Update(s.Results());
            }
            _benchTime = _benchTime.Add(s.BenchTime());
            s = new LineSolver();
            if (s.Run(_ng) > 0)
            {
                Update(s.Results());
                _solved = s.Solved();
            }
            s = new TreeSolver();
            s.Run(_ng);
            _benchTime = _benchTime.Add(s.BenchTime());
            Update(s.Results());
            _solved = true;
            return _results.Count;
        }

        private void Update(List<Result> resQueue)
        {
            while (!resQueue.IsEmpty)
            {
                Result res = resQueue.Dequeue();
                _ng.Set(res.Row, res.Column, res.State);
                _results.Push(res);
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
