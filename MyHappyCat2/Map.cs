using SplashKitSDK;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHappyCAt
{
    public class Map
    {
        private int _tileSize;
        private List<string> _mapData;
        private Bitmap _brickTile;
        private Bitmap _waterTile;
        private Bitmap _wood;
        

        public int TileSize => _tileSize;

        public Map(int tileSize, string filename)
        {
            _mapData = new List<string>();
            _tileSize = tileSize;
            LoadMap(filename);
            _brickTile = SplashKit.LoadBitmap("BrickTile", "brick_tile.png");
            _waterTile = SplashKit.LoadBitmap("WaterTile", "water_tile.png");
            _wood = SplashKit.LoadBitmap("Wood", "wood.png");
        }

        public void LoadMap(string filename)
        {
            try
            {
                using (var reader = new System.IO.StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        _mapData.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading map: {ex.Message}");
                _mapData.Clear();
            }
        }

        public void Draw()
        {
            for (int row = 0; row < _mapData.Count; row++)
            {
                string currentRow = _mapData[row];
                for (int col = 0; col < currentRow.Length; col++)
                {
                    char tileChar = currentRow[col];
                    double x = col * _tileSize;
                    double y = row * _tileSize;

                    // if the char is "#" then draw brick, the char is "." then no brick, empty space
                    if (tileChar == '#')
                    {
                        _brickTile.Draw(x, y);
                    }
                    else if (tileChar == '.')
                    {
                        // do nothing, emty space
                    }
                    else if (tileChar == 'w')
                    {
                        _waterTile.Draw(x, y);  // draw water tile
                    }
                    else if (tileChar == '-')
                    {
                        _wood.Draw(x, y + 50);  // draw wood tile
                    }                    
                    else
                    {
                        Console.WriteLine($"Unknown tile character '{tileChar}' at ({col}, {row})");  // log unknown characters
                    }
                }
            }
        }

        public bool IsSolid(Point2D position)
        {
            int tileCol = (int)(position.X / _tileSize);
            int tileRow = (int)(position.Y / _tileSize);

            if (tileCol < 0 || tileCol >= WidthTiles() || tileRow < 0 || tileRow >= HeightTiles())
                return false;

            char tileChar = _mapData[tileRow][tileCol];

            if (tileChar == '#')
            {
                // Brick: full solid
                return true;
            }
            else if (tileChar == '-')
            {
                // Wood: only solid if within the top 15 pixels of the tile
                double tileTop = tileRow * _tileSize + 50; // wood is drawn at y + 50
                double woodTop = tileTop;
                double woodBottom = woodTop + 15;
                if (position.Y >= woodTop && position.Y <= woodBottom)
                    return true;
                return false;
            }
            else
            {
                // Not solid for other tiles
                return false;
            }
        }

        public int WidthTiles()
        {
            if (_mapData == null || _mapData.Count == 0)
            {
                return 0;
            }
            else
            {
                return _mapData[0].Length;
            }
        }

        public int HeightTiles()
        {
            return _mapData.Count;
        }
    }
}
