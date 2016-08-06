using System;
using System.Linq;

namespace GameLib
{
    internal class NumberData
    {
        private readonly Number[][] _columnNumbers;
        private readonly Number[][] _rowNumbers;
        private int _colCleared = 0;
        private int _rowCleared = 0;

        public NumberData(int[][] columns, int[][] rows)
        {
            _columnNumbers = new Number[columns.Length][];
            _rowNumbers = new Number[rows.Length][];
            for (int i = 0; i < columns.Length; i++)
            {
                _columnNumbers[i] = new Number[columns[i].Length];
                for (int j = 0; j < columns[i].Length; j++)
                {
                    _columnNumbers[i][j] = new Number(columns[i][j]);
                    _colCleared++;
                }
            }
            for (int i = 0; i < rows.Length; i++)
            {
                _rowNumbers[i] = new Number[rows[i].Length];
                for (int j = 0; j < rows[i].Length; j++)
                {
                    _rowNumbers[i][j] = new Number(rows[i][j]);
                    _rowCleared++;
                }
            }
        }

        public string RowsString(int i)
        {
            return String.Join(", ", _rowNumbers[i].Select(n => n.ToString()));
        }

        public string ColumnString(int i)
        {
            return String.Join(", ", _columnNumbers[i].Select(n => n.ToString()));
        }
    }
}