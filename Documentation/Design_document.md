#Design document

The nonogram solver is meant to be an application for solving nonograms with the computers.

And for benchmarking different approaches and optimizations re. solving algorithms and perhaps parsing.

##Very rough design diagram

![Conceptual diagram] (https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Rough_structural_design_diagram.png)

The diagram above shows the broad strokes of the structural design. 

The main take-aways would probably be:

* the 2 separate entry-points, one intended for benchmarking as a console application and one for drawing pretty pictures and showing the work of the algorithms.
 
* The idea of having a generic solver interface that allows a neat modular design and lets the solvers do whatever they want.
 

... This document shall be updated as this project gets going.
