using System;
using GameLib;

namespace SolverLib
{
    public interface ISolver
    {
        int Run(Nonogram ng);
        bool Solved();
        TimeSpan BenchTime();
    }
}
