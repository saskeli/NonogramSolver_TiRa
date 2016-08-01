using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib
{
    public class NonoGramFactory
    {
        public static Nonogram ParseFromString(string simple)
        {
            int[][] rows = null;
            int[][] columns = null;
            int workingIndex = 0;
            List<int> workingList = null;
            int? currInt = null;
            foreach (char c in simple.ToCharArray())
            {
                if (c >= '0' && c <= '9')
                {
                    currInt = currInt == null ? c - '0' : currInt * 10 + (c - '0');
                }
                if (c == ',' && currInt.HasValue)
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
                if ((c == '\r' || c == '\n') && currInt.HasValue)
                {
                    if (rows == null)
                    {
                        rows = new int[currInt.Value][];
                    }
                    else if (workingList != null)
                    {
                        if (workingIndex < rows.Length)
                        {
                            rows[workingIndex] = workingList.ToArray();
                        }
                        else
                        {
                            rows[workingIndex - rows.Length] = workingList.ToArray();
                        }
                        workingIndex++;
                    }
                }
                if (columns != null && rows != null && workingIndex >= columns.Length + rows.Length)
                {
                    break;
                }
            }
            return new Nonogram(columns, rows);
        }
    }
}
