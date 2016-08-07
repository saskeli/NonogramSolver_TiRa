#Week 2

[Hour reporting](https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Hour_reporting.md)

##Progress on predifined goals

Week 2 course goals:

* Documentation - All code written has c# xml documentation.
* Progress on program - very bad solver done
* Testing - See testing

Goals from week 1:

* Class diagram done
* Bunch of coding done
* Working (very bad) solver can solve trivial nonograms

##Testing

![Test coverage] (https://github.com/saskeli/NonogramSolver_TiRa/blob/master/Documentation/Data/Coverage.png)

Test coverage is actually better than it looks. Neither "SolverConsole" not "NonogramSolver" contain testable code. The lacking coverage on GameLib is due to not being able to test the file parser yet and upcoming changes to ToString and similar methods that would make writing tests now useless.

##What has happened during week 2

* Refining of project structure
* Writing of first "working" solver
* Creation of a dynamic "List" class
* Finished string and file parsers
* Created tests for currently testable content

Majority of time this week has been spent on writing the first (very bad) solver and test writing.

##Problems

None significant. Though initial solver is even worse than expected

##What I learned this week

Never really got into writing unit tests in c# before. Especially with ReShaper.

##Next week

* More solvers
* Optimize current TreeSolver a bit
* Keep documentation up to date
