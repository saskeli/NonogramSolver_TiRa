using System;
using System.Linq;
using System.Text;

namespace GameLib
{
    /// <summary>
    /// Class depicting a nonogram game.
    /// </summary>
    public class Nonogram
    {
        private readonly Tile[][] _tiles;
        private readonly int[][] _columnNumbers;
        private readonly int[][] _rowNumbers;

        /// <summary>
        /// Number of rows in the nonogram.
        /// </summary>
        public int Height { get; }
        /// <summary>
        /// Number of columns in the nonogram.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Number of tiles left to resolve.
        /// </summary>
        public int LeftToClear { get; private set; }

        /// <summary>
        /// Nonogram class constructor
        /// </summary>
        /// <param name="columns">Column clues</param>
        /// <param name="rows">Row clues</param>
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

        private Nonogram(int[][] columns, int[][] rows, Tile[][] ts) : this(columns, rows)
        {
            _tiles = ts;
        }

        public int GetPrio(int row, int column)
        {
            return _tiles[row][column].Priority;
        }

        /// <summary>
        /// Generates an ugly string representation of the nonogrma
        /// </summary>
        /// <returns>AN ugly string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Height; i++)
            {
                sb.AppendLine(RowString(i));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates a string reperesentation of the state of a row of the nonogram
        /// </summary>
        /// <param name="i">Row index</param>
        /// <returns>Ugly string</returns>
        public string RowString(int i)
        {
            return String.Join("", _tiles[i].Select(t => t.ToChar()));
        }

        /// <summary>
        /// Checks wether a specified tile has been given a value.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <returns>boolean true for "has a value", false for no value.</returns>
        public bool Resolved(int row, int column)
        {
            return _tiles[row][column].State.HasValue;
        }

        /// <summary>
        /// Sets the value of a specified tile
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <param name="val">Value to set</param>
        public void Set(int row, int column, bool val)
        {
            if (!Resolved(row, column)) LeftToClear--;
            _tiles[row][column].State = val;
        }

        /// <summary>
        /// Removes the value from the specified tile
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        public void Clear(int row, int column)
        {
            if (Resolved(row, column))
            {
                LeftToClear++;
                _tiles[row][column].State = null;
            }
        }

        /// <summary>
        /// Checks if the specified tile has been set to true
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">Column index</param>
        /// <returns>True if tile is set to true, false if tile is not set or set to false.</returns>
        public bool IsTrue(int row, int column)
        {
            return _tiles[row][column].State.HasValue && _tiles[row][column].State.Value;
        }

        /// <summary>
        /// Checks if the specified tile has been set to false
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="column">column index</param>
        /// <returns>True if tile is set to false, false if set to true or not set.</returns>
        public bool IsFalse(int row, int column)
        {
            return _tiles[row][column].State.HasValue && !_tiles[row][column].State.Value;
        }

        /// <summary>
        /// Gets one clue number for one row.
        /// </summary>
        /// <param name="row">Row index</param>
        /// <param name="idx">Clue index on row</param>
        /// <returns>Specified clue int</returns>
        public int GetRowNum(int row, int idx)
        {
            if (idx < 0) return 0;
            return idx < _rowNumbers[row].Length ? _rowNumbers[row][idx] : 0;
        }

        /// <summary>
        /// Gets on eclue number for one column
        /// </summary>
        /// <param name="column">Column index</param>
        /// <param name="idx">Clue index on column</param>
        /// <returns></returns>
        public int GetColumnNum(int column, int idx)
        {
            if (idx < 0) return 0;
            return idx < _columnNumbers[column].Length ? _columnNumbers[column][idx] : 0;
        }

        /// <summary>
        /// Gets total of clues for a given row
        /// </summary>
        /// <param name="row">Row index</param>
        /// <returns>Sum of clues for given row</returns>
        public int RowSum(int row)
        {
            return _rowNumbers[row].Sum();
        }

        /// <summary>
        /// Gets total of clues for given column
        /// </summary>
        /// <param name="column">Column index</param>
        /// <returns>Sum of clues for given column</returns>
        public int ColumnSum(int column)
        {
            return _columnNumbers[column].Sum();
        }

        /// <summary>
        /// Gets clue array for given row
        /// </summary>
        /// <param name="row">index of row</param>
        /// <returns>Clue array</returns>
        public int[] GetRowArray(int row)
        {
            return row < _rowNumbers.Length ? _rowNumbers[row] : new int[0];
        }

        /// <summary>
        /// Gets clue array for given column
        /// </summary>
        /// <param name="column">Index of column</param>
        /// <returns>Clue array</returns>
        public int[] GetColumnArray(int column)
        {
            return column < _columnNumbers.Length ? _columnNumbers[column] : new int[0];
        }

        /// <summary>
        /// Creates a new copy of the nonogram
        /// </summary>
        /// <returns>A new nonogram with the same values</returns>
        public Nonogram Copy()
        {
            int[][] cNums = new int[_columnNumbers.Length][];
            int[][] rNums = new int[_rowNumbers.Length][];
            Tile[][] ts = new Tile[_tiles.Length][];
            for (int i = 0; i < _columnNumbers.Length; i++)
            {
                cNums[i] = new int[_columnNumbers[i].Length];
                for (int j = 0; j < _columnNumbers[i].Length; j++)
                {
                    cNums[i][j] = _columnNumbers[i][j];
                }
            }
            for (int i = 0; i < _rowNumbers.Length; i++)
            {
                rNums[i] = new int[_rowNumbers[i].Length];
                for (int j = 0; j < _rowNumbers[i].Length; j++)
                {
                    rNums[i][j] = _rowNumbers[i][j];
                }
            }
            for (int i = 0; i < _tiles.Length; i++)
            {
                ts[i] = new Tile[_tiles[i].Length];
                for (int j = 0; j < _tiles[i].Length; j++)
                {
                    ts[i][j] = _tiles[i][j].Copy();
                }
            }
            return new Nonogram(cNums, rNums, ts);
        }

        /// <summary>
        /// Check if the entire row has been resolved.
        /// </summary>
        /// <param name="row">row index</param>
        /// <returns>true if all tiles on the row are set to true or false</returns>
        public bool FullRow(int row)
        {
            return _tiles[row].All(x => x.State.HasValue);
        }

        /// <summary>
        /// Check if the entire column has been resolved.
        /// </summary>
        /// <param name="column">column index</param>
        /// <returns>true if all tiles in the column are set to true or false</returns>
        public bool FullColumn(int column)
        {
            return _tiles.All(x => x[column].State.HasValue);
        }

        /// <summary>
        /// Adds 1 to the priority of the specified tile.
        /// </summary>
        /// <param name="row">row index</param>
        /// <param name="column">column index</param>
        /// <returns>True if incrementing priority was possible</returns>
        public bool AddPrio(int row, int column)
        {
            if (row < 0 || column < 0 || row >= Height || column >= Width)
            {
                return false;
            }
            _tiles[row][column].Priority++;
            return true;
        }
    }
}
