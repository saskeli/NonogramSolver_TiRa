## Project structure

The initial design envisioned for the project has held fairly well. The conceptual design diagram is still as accurate as ever.

![Conceptual diagram](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Rough_structural_design_diagram.png)

The idea of an ISolver interface implemented by multiple solvers was very good for the structure even if it wasn't strictly needed in the end. Dual entry points was very convenient, since it enabled working on the algorithms with an extremely simple console application as the entry point for most of the project without stressing over the GUI.

## Complexity and performance

The overall time complexity of the solutions stayed at O(2<sup>n</sup>) due to the NP-completeness of Nonograms. However much progress was made over time to the point where the worst case complexity will never be encountered by the final versions at the end of week 6. Sadly this is done by giving up immediately if it looks like significant use of game trees would be needed. Still the current version of the serialSolver (the one used both for benchmarking and the GUI solutions) solves all but the extremely difficult (for computers) Nonograms in what amounts to trivial time. The performance in both speed and reliability is significantly behind ["industry" leaders (Survey on PBN solvers)](http://webpbn.com/survey/#samptime).

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

All of the files mentioned above are available in the project. (This may change in the future.)

DiCap.txt, Sierp.txt, Swing.txt, tragic.txt, Petro.txt, knot.txt, Gettys.txt and dom.txt are all taken from the survey linked above. As these are specifically chosen to be very difficult for automated solving and this solver was never really intended to solve Nonograms with multiple solutions, the results are hardly surprising.

Space complexity is a non-issue. The actual complexity is O(n) with fairly high multiples of n. Still I could not get any meaningful data on memory scaling with nonogram size/complexity. The overhead of the .NET framework dwarfs the space used by the acutal solving algorithms.

Overall the serial solver is faster and more reliable than bad solvers and significantly worse than good solvers.

## Possible improvements

### Optimization

I have not tested, but I suspect that significant improvements could be seen if the recursions in the lineSolver and the treeSolver were flattened into loops. This should save time on memory management when less operations on the call stack would be needed. And the used recursions shouldn't compile out since they aren't tail recursions. Either way this would be intresting to test.

Making both the lineSolver and the trialSolver work with multiple threads should be fairly doable and should lead to significant performance improvements.

### Reliability

Significant improvements re. solvability could be made by introducing limited recursion in the trial solver. This was not done due to time constraints. The time complexity of the solver quickly goes out of hand if the recursion is not very tightly controlled.

The treeSolver could potentially be improved by integrating the lineSolver. But managing dead ends became too hard in this version for me to implement.

## Sources

[Nonogram](https://en.wikipedia.org/wiki/Nonogram). From Wikipedia, the free encyclopedia.

[Survey of Paint-by-Number Puzzle Solvers](http://webpbn.com/survey/). [Jan Wolter](http://unixpapa.com/), webpbn.com.

[http://www.lancaster.ac.uk/~simpsons/nonogram/](http://www.lancaster.ac.uk/~simpsons/nonogram/)
