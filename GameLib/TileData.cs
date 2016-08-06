using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace GameLib
{
    internal class TileData
    {
        private readonly Tile[][] _tiles;
        public int Height { get; }
        public int Width { get; }

        public TileData(int colCount, int rowCount)
        {
            Width = colCount;
            Height = rowCount;
            _tiles = new Tile[rowCount][];
            int prio;
            for (int i = 0; i < rowCount; i++)
            {
                _tiles[i] = new Tile[colCount];
                for (int j = 0; j < colCount; j++)
                {
                    prio = 0;
                    if (i == 0 || i == rowCount - 1) prio++;
                    if (j == 0 || j == colCount - 1) prio++;
                    _tiles[i][j] = new Tile(prio);
                }
            }
        }

        public string RowString(int i)
        {
            return String.Join("", _tiles[i].Select(t => t.ToChar()));
        }
    }
}