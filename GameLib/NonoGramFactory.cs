using System;
using System.IO;
using Util;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class NonoGramFactory
    {
        public static Nonogram ParseFromFile(string path)
        {
            int[][] rows = null;
            int[][] columns = null;
            int workingIndex = 0;
            List<int> workingList = null;
            int? currInt = null;
            using (StreamReader sr = new StreamReader(path, Encoding.UTF8, true))
            {
                while (!sr.EndOfStream)
                {
                    char c = (char)sr.Read();
                    if (c >= '0' && c <= '9')
                    {
                        currInt = currInt == null ? c - '0' : currInt * 10 + (c - '0');
                    }
                    else if (c == ',' && currInt.HasValue)
                    {
                        if (columns == null)
                        {
                            columns = new int[currInt.Value][];
                        }
                        else
                        {
                            if (workingList == null)
                            {
                                workingList = new List<int>();
                            }
                            if (currInt.Value != 0)
                            {
                                workingList.Add(currInt.Value);
                            }
                        }
                        currInt = null;
                    }
                    else if ((c == '\r' || c == '\n') && currInt.HasValue)
                    {
                        if (columns == null)
                        {
                            columns = new int[currInt.Value][];
                        }
                        else if (rows == null)
                        {
                            rows = new int[currInt.Value][];
                        }
                        else
                        {
                            if (workingList == null)
                            {
                                workingList = new List<int>();
                            }
                            if (currInt.Value > 0) workingList.Add(currInt.Value);
                            if (workingIndex < rows.Length)
                            {
                                int[] arr = workingList.ToArray();
                                if (InvalidSum(arr, columns.Length)) throw new ArgumentException(
                                    "Invalid row definition for row " + (workingIndex + 1), nameof(path));
                                rows[workingIndex] = arr;
                            }
                            else
                            {
                                int[] arr = workingList.ToArray();
                                if (InvalidSum(arr, rows.Length)) throw new ArgumentException(
                                    "Invalid column definition for column" + (workingIndex - rows.Length + 1), nameof(path));
                                columns[workingIndex - rows.Length] = arr;
                            }
                            workingIndex++;
                        }
                        workingList = null;
                        currInt = null;
                    }
                    if (columns != null && rows != null && workingIndex >= columns.Length + rows.Length)
                    {
                        break;
                    }
                }
            }
            if (MissingData(columns, rows)) throw new ArgumentException(
                "Too few data rows", nameof(path));
            if (InvalidTotal(columns, rows)) throw new ArgumentException(
                "Invalid tile totals", nameof(path));
            return new Nonogram(columns, rows);
        }

        public static Nonogram ParseFromString(string parseable)
        {
            int[][] rows = null;
            int[][] columns = null;
            int workingIndex = 0;
            List<int> workingList = null;
            int? currInt = null;
            foreach (char c in parseable.ToCharArray())
            {
                if (c >= '0' && c <= '9')
                {
                    currInt = currInt == null ? c - '0' : currInt * 10 + (c - '0');
                }
                else if (c == ',' && currInt.HasValue)
                {
                    if (columns == null)
                    {
                        columns = new int[currInt.Value][];
                    }
                    else
                    {
                        if (workingList == null)
                        {
                            workingList = new List<int>();
                        }
                        if (currInt.Value != 0)
                        {
                            workingList.Add(currInt.Value);
                        }
                    }
                    currInt = null;
                }
                else if ((c == '\r' || c == '\n') && currInt.HasValue)
                {
                    if (columns == null)
                    {
                        columns = new int[currInt.Value][];
                    }
                    else if (rows == null)
                    {
                        rows = new int[currInt.Value][];
                    }
                    else 
                    {
                        if (workingList == null)
                        {
                            workingList = new List<int>();
                        }
                        if (currInt.Value > 0) workingList.Add(currInt.Value);
                        if (workingIndex < rows.Length)
                        {
                            int[] arr = workingList.ToArray();
                            if (InvalidSum(arr, columns.Length)) throw new ArgumentException(
                                "Invalid row definition for row " + (workingIndex + 1), nameof(parseable));
                            rows[workingIndex] = arr;
                        }
                        else
                        {
                            int[] arr = workingList.ToArray();
                            if (InvalidSum(arr, rows.Length)) throw  new ArgumentException(
                                "Invalid column definition for column" + (workingIndex - rows.Length + 1), nameof(parseable));
                            columns[workingIndex - rows.Length] = arr;
                        }
                        workingIndex++;
                    }
                    workingList = null;
                    currInt = null;
                }
                if (columns != null && rows != null && workingIndex >= columns.Length + rows.Length)
                {
                    break;
                }
            }
            if (MissingData(columns, rows)) throw  new ArgumentException(
                "Too few data rows", nameof(parseable));
            if (InvalidTotal(columns, rows)) throw new ArgumentException(
                "Invalid tile totals", nameof(parseable));
            return new Nonogram(columns, rows);
        }

        private static bool MissingData(int[][] columns, int[][] rows)
        {
            return columns.Any(column => column == null) || rows.Any(row => row == null);
        }

        private static bool InvalidTotal(int[][] columns, int[][] rows)
        {
            int cTot = columns.Sum(column => column.Sum());
            int rTot = rows.Sum(row => row.Sum());
            return rTot != cTot;
        }

        private static bool InvalidSum(int[] arr, int maxSUm)
        {
            return arr.Length - 1 + arr.Sum() > maxSUm;
        }
    }
}
