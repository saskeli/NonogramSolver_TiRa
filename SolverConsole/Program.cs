using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GameLib;
using SolverLib;

namespace SolverConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            TimeSpan ts;
            string path = Environment.CurrentDirectory;
            if (path.Contains("NonogramSolver"))
            {
                path = Regex.Replace(path, "NonogramSolver.*", @"NonogramSolver\\Data");
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("No nonograms found in " + path);
                    path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
                }
            }
            else
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            }
            if (!Directory.Exists(path))
            {
                Console.WriteLine("No nonograms found in " + path);
                Console.WriteLine("Any key to terminate.");
                Console.ReadKey();
            }
            StringBuilder sb = new StringBuilder();
            foreach (string file in Directory.GetFiles(path))
            {
                
                Console.WriteLine("\nBenchmarking file: " + Path.GetFileName(file));
                sb.AppendLine("\nBenchmarking file: " + Path.GetFileName(file));
                Nonogram ng = NonoGramFactory.ParseFromFile(file);
                ISolver s = new SerialSolver(true);
                s.Run(ng);
                Console.WriteLine("Solvable with SerialSolver: " + s.Solved());
                sb.AppendLine("Solvable with SerialSolver: " + s.Solved());
                if (s.Solved())
                {
                    ts = TimeSpan.Zero;
                    for (int i = 0; i < 50; i++)
                    {
                        s.Run(ng);
                        ts = ts.Add(s.BenchTime());
                    }
                    Console.WriteLine("Average solving time: " + (ts.TotalMilliseconds / 50) + "ms");
                    sb.AppendLine("Average solving time: " + (ts.TotalMilliseconds / 50) + "ms");
                }
            }
            using (StreamWriter sw = new StreamWriter("Output.txt", false))
            {
                sw.Write(sb.ToString());
                Console.WriteLine("Output written to Output.txt");
            }
            Console.WriteLine("Any key to terminate.");
            Console.ReadKey();
        }
    }
}
