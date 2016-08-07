using System;
using System.Diagnostics;
using System.IO;
using Util;
using System.Linq;
using System.Text;

namespace GameLib
{
    /// <summary>
    /// Factory class for generating nonograms from string or file data
    /// </summary>
    public class NonoGramFactory
    {
        /// <summary>
        /// Generates a nonogram object from CSV file data
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <returns>Nonogram object</returns>
        public static Nonogram ParseFromFile(string path)
        {
            bool interrupted = false;
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
                        interrupted = true;
                        break;
                    }
                }
            }
            // Adds the last line if there was no line break after the last relevant data.
            if (!interrupted && (workingList != null || currInt.HasValue))
            {
                if (workingList == null) workingList = new List<int>();
                if (currInt.HasValue && currInt.Value > 0) workingList.Add(currInt.Value);
                Debug.Assert(rows != null, "rows != null"); // At this poing parseable file would be highly invalid anyway.
                if (workingIndex < rows.Length)
                {
                    int[] arr = workingList.ToArray();
                    Debug.Assert(columns != null, "columns != null"); // At this poing parseable file would be highly invalid anyway.
                    if (InvalidSum(arr, columns.Length)) throw new ArgumentException(
                        "Invalid row definition for row " + (workingIndex + 1), nameof(path));
                    rows[workingIndex] = arr;
                }
                else
                {
                    int[] arr = workingList.ToArray();
                    if (InvalidSum(arr, rows.Length)) throw new ArgumentException(
                        "Invalid column definition for column" + (workingIndex - rows.Length + 1), nameof(path));
                    Debug.Assert(columns != null, "columns != null"); // At this poing parseable file would be highly invalid anyway.
                    columns[workingIndex - rows.Length] = arr;
                }
            }
            if (MissingData(columns, rows)) throw new ArgumentException(
                "Too few data rows", nameof(path));
            if (InvalidTotal(columns, rows)) throw new ArgumentException(
                "Invalid tile totals", nameof(path));
            return new Nonogram(columns, rows);
        }

        /// <summary>
        /// Generates a nonogram object based on string CSV data
        /// </summary>
        /// <param name="parseable">Strign data</param>
        /// <returns>Nonogram object</returns>
        public static Nonogram ParseFromString(string parseable)
        {
            int[][] rows = null;
            int[][] columns = null;
            int workingIndex = 0;
            List<int> workingList = null;
            int? currInt = null;
            foreach (char c in parseable)
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

        /// <summary>
        /// Check for null rows or columns
        /// </summary>
        /// <param name="columns">Jagged array of column clues</param>
        /// <param name="rows">Jagged array of row clues</param>
        /// <returns>True if row or column data contains null arrays</returns>
        private static bool MissingData(int[][] columns, int[][] rows)
        {
            return columns.Any(column => column == null) || rows.Any(row => row == null);
        }

        /// <summary>
        /// Checks that the total of row clues matches the total of column clues
        /// </summary>
        /// <param name="columns">Jagged array of column clues</param>
        /// <param name="rows">Jagged array of row clues</param>
        /// <returns>True if totals do not match</returns>
        private static bool InvalidTotal(int[][] columns, int[][] rows)
        {
            int cTot = columns.Sum(column => column.Sum());
            int rTot = rows.Sum(row => row.Sum());
            return rTot != cTot;
        }

        /// <summary>
        /// Checks if the array has a total higher than possible for nonograms
        /// </summary>
        /// <param name="arr">Array of row or column clues.</param>
        /// <param name="maxSUm">Width or hight of nonogram as applicable.</param>
        /// <returns>True if the array contains invalid clue.</returns>
        private static bool InvalidSum(int[] arr, int maxSUm)
        {
            return arr.Length - 1 + arr.Sum() > maxSUm;
        }
    }
}
