## Console benchmarking

### From zip package

After extracting the contents of the Package. On running. The console application "SolverConsole.exe" will look for files in the "Data" subfolder and run benchmarks.

### From project

Set SolverConsole as startup application and start. The application will look for files in the "Data" folder under the project root.

The benchmark will try to parse all files found and if they are valid nonograms try to solve them. If the Nonogram is solvable, the applicatino will run the solver 50 times for the file and report the average time to solve.

In both cases. After running the benchmarks data is dumped into ["Output.txt"](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Output.txt) to the folder containing the executable.

## Using the GUI

After starting the application you will see the following (or equivalent):

![Startup](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Startup.png)

To solve a nonogram, Drag and Drop a nonogram txt file onto the textbox at the top of the window:

![Drag](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Drag.png)

The application will try to solve the nonogram and show the results. Note that so far the longest time to wait has been over 10 minutes for the "Gettys.txt" nonogram. If you get tired of waiting simply restart the application. After the result simulation is done. Another nonogram can be dragged and dropped in the same way to solve more.

## Compatibility

These applications should run on any machine that has .NET 4.0 or newer (4.5 or newer tested). And should work on Windows vista or newer.
