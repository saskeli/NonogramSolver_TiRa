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
        private readonly TileData _tiles;
        private readonly NumberData _numbers;
        public Nonogram(int[][] colums, int[][] rows)
        {
            _tiles = new TileData(colums.Length, rows.Length);
            _numbers = new NumberData(colums, rows);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < _tiles.Height; i++)
            {
                sb.Append(_tiles.RowString(i));
                sb.Append("| ");
                sb.AppendLine(_numbers.RowsString(i));
            }
            sb.AppendLine("Columndata:");
            for (int i = 0; i < _tiles.Width; i++)
            {
                sb.Append("c" + i + ": ");
                sb.AppendLine(_numbers.ColumnString(i));
            }

            return sb.ToString();
        }
    }
}
