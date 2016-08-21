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
            _solved = _results.Count == ng.Width * ng.Height;
            sw.Stop();
            _time = sw.Elapsed;
            return _results.Count;
        }

        private void InitRow(int row)
        {
            int[] rowArr = _ng.GetRowArray(row);
            int delta = _ng.Width - (rowArr.Sum() + rowArr.Length - 1);

        }

        private void InitColumn(int column)
        {
            throw new NotImplementedException();
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
