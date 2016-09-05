## Unit testing

Unit testing coverage can be found [here](https://www.cs.helsinki.fi/u/saska/Coverage/Coverage.html).

Test coverage is not as good as it could be. But looks even worse than it is.

### NonogramSolver, SolverConsole

Nothing to test really. Entry points work or don't. These have been extensively tested manually.

### SolverLib

InitSolver is no longer used so no need to test it.

TreeSolver Coverage is OK.

All the others could really use more tests. These have been tested manually tho.

### GameLib

I could not get tests to work propery for the ParseFromFile method for the NonogramFactory. Has been very extensively tested manually.

The Nonogram and Tile classes should have better coverage. And if the project is ever updated these are top priority. At this time they clearly work since they are used for testing the GUI and benchmarking.

### Util

Has fairly good test coverage. Could still be improved. No real reason not to have 100% coverage here.

## Manual testing

Loads of manual testing have been done with the GUI application, the benchmarking console application and scripting (before the GUI and console applcations were done).

![GUI](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/swing.png)
![console](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/benchmarks.png)

These tests have been run with all the nonogram files in the table below as well as several others that have been lost to time.

Name | Solvable | average time|
-----|----------|-------------|
Band.txt | True | 16,223248ms |
camera.txt | True | 0,481822ms |
cherries.txt | True | 0,01934ms |
cuppa.txt | True | 0,011688ms |
DiCap.txt | False | |
diff.txt | True | 1,923444ms |
dom.txt | False | |
duck.txt | True | 0,035526ms |
Gettys.txt | False | |
home.txt | True | 0,015142ms |
home2.txt | True | 0,047454ms |
joker.txt | True | 3,918756ms |
knot.txt | True | 1,138628ms |
pelican.txt | True | 0,27851ms |
Petro.txt | True | 13,35829ms |
Sierp.txt | False | |
Swing.txt | True | 3,791022ms |
tragic.txt | False | |
unk.txt | True | 0,427136ms |
wake_up.txt | True | 0,012482ms |

I couldn't really run any meaningfull comparisons between solver algorithms since the serialSolver was the only reliable one besides the lineSolver. The lineSolver and the serialSolver perform essentially identically for Nonograms the lineSolver can solve.

## How to re-test

The manual tests can be run by using the [compiled binaries](https://github.com/saskeli/NonogramSolver_TiRa/tree/master/Binaries) (or by compiling the source). See [User Guide](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/User_Guide.md) for details
