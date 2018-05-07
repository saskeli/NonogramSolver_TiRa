# Design document

The nonogram solver is meant to be an application for solving nonograms with the computers.

And for benchmarking different approaches and optimizations re. solving algorithms and perhaps parsing.

## Algorithm design and Time complexity

Initial information on solution methods has been taken from [wikipedia](https://en.wikipedia.org/wiki/Nonogram#Solution_techniques)

The idea is to create multiple solving algorithms with complexities ranging essentially from O(n) to O(2^n). 
Where the Low complexity algorithms will be very naive and only able to solve trivial nonograms, and the high complexity algorithms will essentially be game trees that are guaranteed to find all possible solutions. Additionally the aim is to also be able to run multiple algorithms both sequantially, recursively and/or in parallell to hopefully make efficient use of computing power and to hopefully solve very complex nonograms efficiently. 

## Data structures and Space complexity

Fairly few advanced data structures should be needed, due to the grid based design of nonograms.
Square and jagged arrays of integers and structs will be sufficient for most of the algorithms.
Linked lists and array-lists will find limited use.
The only tricky data structure that I know I'll need to do at this time is a "hashing" Max-heap. This is simplified however by the fact that no hashing will actually be needed since the used data sets are well defined.

Space requirements will generally be O(n) even some of the algorithms could potentially require fairly high multiples of n.

## Input / Output

### Input (applies to both entry points):  
* Loading from CSV data (strings or files)
* built in templates.

### Output:  
Console application:
* diagnostic data 
* benchmark results

GUI:
* finished nonogram
* algorith simulation (Slow fill)
* limited benchmarks

## Conseptual design diagram

![Conceptual diagram](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Rough_structural_design_diagram.png)

The diagram above shows the broad strokes of the structural design. 

The main take-aways would probably be:

* the 2 separate entry-points, one intended for benchmarking as a console application and one for drawing pretty pictures and showing the work of the algorithms.
 
* The idea of having a generic solver interface that allows a neat modular design and lets the solvers do whatever they want.
 
## Class diagram

![Class diagram](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Class_diagram.png)

The diagram above shows current class structure of the project, excluding test classes
