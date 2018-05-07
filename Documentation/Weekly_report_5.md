# Week 5

[Hour reporting](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Hour_reporting.md)

## Progress

First sort of usable solver done.
Initial benchmark results and general performance analysis.

## Testing

[Test coverage](https://www.cs.helsinki.fi/u/saska/Coverage/ListInitTest.html)

Test coverage is available outside of github so it can be more readily browsed without downloading.
Test coverage has not been updated this week, since no new tests were done.
Test coverage as well as documentation will be improved next.

## Benchmarks

![Benchmark results](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/benchmarks.png)

Solving easy nonograms is now fairly practical with the SerialSolver.

The treesolver is totally impractical even for these.

The [Cherries](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Data/cherries.txt) nonogram is missing from the benchmark since the Treesolver would take days to generate meaningfull benchmark results.

Memory use of the solvers is trivial on modern computers.

## Problems

Who knew designing algorithms for solving NP complete problems was hard?

## Next week

* Documentation
* Tests
* Possibly a line operation solver if there is time.

GUI wil likely not be implemented before the end of the course.
