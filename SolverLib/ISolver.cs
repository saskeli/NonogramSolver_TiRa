using System;
using GameLib;
using Util;

namespace SolverLib
{
    public interface ISolver
    {
        int Run(Nonogram ng);
        bool Solved();
        TimeSpan BenchTime();
        List<Result> Results();
    }
}
