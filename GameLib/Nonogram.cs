using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameLib
{
    public class Nonogram
    {
        private readonly Tile[][] _tiles;
        private readonly int[][] _columnNumbers;
        private readonly int[][] _rowNumbers;
        public int Height { get; }
        public int Width { get; }
        public int LeftToClear { get; private set; }

        public Nonogram(int[][] columns, int[][] rows)
        {
            Width = columns.Length;
            Height = rows.Length;
            LeftToClear = Height * Width;
            _tiles = new Tile[Height][];
            int prio;
            for (int i = 0; i < Height; i++)
            {
                _tiles[i] = new Tile[Width];
                for (int j = 0; j < Width; j++)
                {
                    prio = 0;
                    if (i == 0 || i == Height - 1) prio++;
                    if (j == 0 || j == Width - 1) prio++;
                    _tiles[i][j] = new Tile(prio);
                }
            }
            _columnNumbers = columns;
            _rowNumbers = rows;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                sb.Append(RowString(i));
                sb.Append("| ");
                sb.AppendLine(RowsNumString(i));
            }
            sb.AppendLine("Columndata:");
            for (int i = 0; i < Width; i++)
            {
                sb.Append("c" + i + ": ");
                sb.AppendLine(ColumnNumString(i));
            }

            return sb.ToString();
        }

        private string ColumnNumString(int i)
        {
            return String.Join(", ", _columnNumbers[i].Select(n => n.ToString()));
        }

        private string RowsNumString(int i)
        {
            return String.Join(", ", _rowNumbers[i].Select(n => n.ToString()));
        }

        public string RowString(int i)
        {
            return String.Join("", _tiles[i].Select(t => t.ToChar()));
        }

        public bool Resolved(int row, int column)
        {
            return _tiles[row][column].State.HasValue;
        }

        public void Set(int row, int column, bool val)
        {
            _tiles[row][column].State = val;
            LeftToClear--;
        }

        public void Clear(int row, int column)
        {
            if (Resolved(row, column))
            {
                LeftToClear++;
                _tiles[row][column].State = null;
            }
        }

        public bool IsTrue(int row, int column)
        {
            return _tiles[row][column].State.HasValue && _tiles[row][column].State.Value;
        }

        public int GetRowNum(int row, int idx)
        {
            return idx < _rowNumbers[row].Length ? _rowNumbers[row][idx] : 0;
        }

        public int GetColumnNum(int column, int idx)
        {
            return idx < _columnNumbers[column].Length ? _columnNumbers[column][idx] : 0;
        }

        public int RowSum(int row)
        {
            return _rowNumbers[row].Sum();
        }

        public int ColumnSum(int column)
        {
            return _columnNumbers[column].Sum();
        }
    }
}
